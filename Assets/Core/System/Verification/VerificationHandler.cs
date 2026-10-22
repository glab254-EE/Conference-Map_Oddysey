using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class VerificationHandler : MonoBehaviour
{
    [SerializeField]
    private QuestionsHandler QuestionsBase;
    [SerializeField]
    private InterfacesHandler handler;
    [SerializeField]
    private int TargetIIDOnFail = 0;
    [SerializeField]
    private int TargetIIDOnLoginSuceed = 0;
    [SerializeField]
    private int TargetIIDOnLoginSuceedWithNoCaptainTutorial = 0;
    [Serializable]
    private class ScoreRequest
    {
        public string Username;
        public float Score;
    }
    [Serializable]
    private class RegisterRequest
    {
        public string username;
        public string displayname;
        public string password;
    }
    [Serializable]
    private class LoginRequest
    {
        public string username;
        public string password;
    }
    [SerializeField]
    private string URLInfoGather = "http://localhost:5000/api/player/";
    [SerializeField]
    private string URLScoreSetter = "http://localhost:5000/api/score";
    [SerializeField]
    private string URLRegisterer = "http://localhost:5000/api/register";
    public event Action<PlayerData> OnPlayerLoaded;
    public event Action<string> OnError;
    public PlayerData CurrentPlayer { get; private set; }
    public bool IsLoggedIn
    {
        get
        {
            if (DataManager.GData != null && DataManager.GData.playerData != null && CurrentPlayer == null) CurrentPlayer = DataManager.GData.playerData;
            return CurrentPlayer != null;
        }
    }
    void Start()
    {
        QuestionsBase.OnDataLoaded += () =>
                         {
                             CurrentPlayer = DataManager.GData.playerData;
                             float total = 0;
                             if (DataManager.GData.IsleInformation.Count > 0)
                             {
                                 foreach (var values in DataManager.GData.IsleInformation)
                                 {
                                     if (values.Value.Item1 >= 0)
                                     {
                                         total += values.Value.Item1;
                                     }
                                 }
                                 SetScore(total);
                             }
                         };
    }

    public void CompleteTutorial()
    {
        DataManager.UpdateTutorialStatus();
    }
    public void TryEnterPage()
    {
#if !UNITY_EDITOR
        if (IsLoggedIn == false)
        {
            handler.OpenFrameByIndex(TargetIIDOnFail);
        } else
        {
            if (DataManager.GData == null || DataManager.GData.HaveDoneTutorial == false)
            {
                handler.OpenFrameByIndex(TargetIIDOnLoginSuceed);
            }
            else
            {
                handler.OpenFrameByIndex(TargetIIDOnLoginSuceedWithNoCaptainTutorial);
            }
        }
#else
        handler.OpenFrameByIndex(TargetIIDOnLoginSuceed);
#endif
    }
    public IEnumerator GetPlayerCoroutine(string username,string _password)
    {

        string encodedUsername =
            UnityWebRequest.EscapeURL(UsernameUtils.MakeUsername(username.Trim(), 30));

        string url = URLInfoGather;

        LoginRequest data = new LoginRequest
        {
            username = encodedUsername,
            password = _password,
        };

        string json = JsonUtility.ToJson(data);

        byte[] body =
            Encoding.UTF8.GetBytes(json);

        using UnityWebRequest request = new(url, "POST");

        request.uploadHandler =
            new UploadHandlerRaw(body);
        request.downloadHandler =
            new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/wasm; charset=utf-8");
        yield return request.SendWebRequest();

        if (request.responseCode == 404) yield break;

        if (request.result != UnityWebRequest.Result.Success)
        {
            HandleError(request);
            yield break;
        }

        PlayerData player =
            JsonUtility.FromJson<PlayerData>(
                request.downloadHandler.text
            );

        if (player == null) yield break;

        if (player.password != null && player.password != _password)
        {
            ReportError("Не верный пароль.");
            yield break;
        }

        CurrentPlayer = player;

        OnPlayerLoaded?.Invoke(player);
        DataManager.TrySetPlayer(player);
        handler.OpenFrameByIndex(TargetIIDOnLoginSuceed);
        float total = 0;
        if (DataManager.GData.IsleInformation.Count > 0)
        {
            foreach (var values in DataManager.GData.IsleInformation)
            {
                if (values.Value.Item1 >= 0)
                {
                    total += values.Value.Item1;
                }
            }
            SetScore(total);
        }
    }
    public IEnumerator RegisterEnumerator(string displayName, string password)
    {
        if (displayName == null || ValidateString(displayName) == false)
        {
            ReportError("Не верный формат имени.");
            yield break;
        }
        if (IsLoggedIn)
        {
            TryEnterPage();
            yield break;
        }

        string encodedUsername =
            UnityWebRequest.EscapeURL(UsernameUtils.MakeUsername(displayName.Trim(),30));
        RegisterRequest data = new RegisterRequest
        {
            username = encodedUsername,
            displayname = displayName,
            password = password,
        };

        string json = JsonUtility.ToJson(data);

        byte[] body =
            Encoding.UTF8.GetBytes(json);

        using UnityWebRequest request =new(
                   URLRegisterer,
                   "POST");
        request.uploadHandler =
            new UploadHandlerRaw(body);

        request.downloadHandler =
            new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/wasm; charset=utf-8");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            HandleError(request);
            yield break;
        }

        yield return StartCoroutine(GetPlayerCoroutine(displayName, password));
    }
    public bool ValidateString(string username)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username))
            return false;
        if (username.Length < 3 || username.Length >= 30)
            return false;
        return true;
    }
    public void SetScore(float score)
    {
        if (CurrentPlayer == null)
        {
            return;
        }

        StartCoroutine(SetScoreCoroutine(CurrentPlayer.username, score));
    }
    private IEnumerator SetScoreCoroutine(string username,float score)
    {

        if (score < 0)
        {
            yield break;
        }

        ScoreRequest data = new()
        {
            Username = username,
            Score = score
        };

        string json = JsonUtility.ToJson(data);

        byte[] body =
            Encoding.UTF8.GetBytes(json);

        using UnityWebRequest request =
               new(
                   URLScoreSetter,
                   "POST");
        request.uploadHandler =
            new UploadHandlerRaw(body);

        request.downloadHandler =
            new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/wasm; charset=utf-8");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            HandleError(request);
            yield break;
        }

        Debug.Log("Saved!");
    }
    private void HandleError(UnityWebRequest request)
    {
        string message;

        if (request.responseCode == 409)
        {
            message = "Имя уже используется.";
        }
        else if (request.responseCode == 400)
        {
            message = "Не верный запрос.";
        }
        else if (request.responseCode == 401)
        {
            message = "Не верный пароль.";
        }
        else if (request.responseCode == 400)
        {
            return;
        }
        else if (request.result ==
                 UnityWebRequest.Result.ConnectionError)
        {
            message =
                "Сервер не доступен.";
        }
        else
        {
            message =
                $"Ошибка сервера ({request.responseCode}): " +
                request.error;
        }

        ReportError(message);
    }
    public void ReportError(string message)
    {
        Debug.LogError(
            "[VerificationAPI] " + message
        );

        OnError?.Invoke(message);
    }
}


[Serializable,HideInInspector]
public class PlayerData
{
    public int id;
    public string username;
    public string displayname;
    public int score;
    public string password;
}