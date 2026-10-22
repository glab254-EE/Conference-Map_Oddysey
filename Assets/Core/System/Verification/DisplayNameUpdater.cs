using TMPro;
using UnityEngine;
[RequireComponent(typeof(TMP_Text))]
public class DisplayNameUpdater : MonoBehaviour
{
    [SerializeField]
    private VerificationHandler Handler;
    private TMP_Text textField;
    void Start()
    {
        textField = GetComponent<TMP_Text>();
        Handler.OnPlayerLoaded += OnUpdate;
        OnUpdate(Handler.CurrentPlayer);
    }
    void OnUpdate(PlayerData data)
    {
        if (data == null) return;
        if (data.displayname == null || data.displayname.Length <= 1)
        {
            textField.text = data.username;
        } else
        {
            textField.text = data.displayname;
        }
    }
}
