using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VerificationButtonHandler : MonoBehaviour
{
    [SerializeField]
    private VerificationHandler handler;
    [SerializeField]
    private Button button;
    [SerializeField]
    private Button TogglePasswordHideButton;
    [SerializeField]
    private TMP_InputField displayNameField; 
    [SerializeField]
    private TMP_InputField passwordField;
    [SerializeField]
    private TMP_InputField confirmField;
    [SerializeField]
    private Image HideButtonImage;
    [SerializeField]
    private Sprite HiddenSprite;
    [SerializeField]
    private Sprite ShownSprite;
    void Start() 
    {
        button.onClick.AddListener(OnLoginPress);
        TogglePasswordHideButton.onClick.AddListener(OnRevealPress);
    }
    void OnRevealPress()
    {
        if (passwordField.contentType == TMP_InputField.ContentType.Password)
        {
            passwordField.contentType = TMP_InputField.ContentType.Standard;
            confirmField.contentType = TMP_InputField.ContentType.Standard;
            passwordField.ForceLabelUpdate();
            confirmField.ForceLabelUpdate();
            HideButtonImage.sprite = ShownSprite;
        } else
        {
            passwordField.contentType = TMP_InputField.ContentType.Password;
            confirmField.contentType = TMP_InputField.ContentType.Password;
            passwordField.ForceLabelUpdate();
            confirmField.ForceLabelUpdate();
            HideButtonImage.sprite = HiddenSprite;
        }
    }
    void OnLoginPress()
    {
        if (displayNameField.text.Length < 3) return;
        StartCoroutine(PrimaryEnumerator());
    }
    IEnumerator PrimaryEnumerator()
    {
        if (confirmField.text != passwordField.text) 
        {
            handler.ReportError("Пароли не совпадают.");
            yield break; 
        }
        if (displayNameField.text.Length < 3 || passwordField.text.Length < 3)
        {
            handler.ReportError("Логин/Пароль слишком маленький.\nМинимум 3 символа.");
            yield break;
        }
        button.interactable = false;
        if (handler.IsLoggedIn)
        {
            handler.TryEnterPage();
            yield break;
        }
        yield return StartCoroutine(handler.GetPlayerCoroutine(displayNameField.text, passwordField.text));
        if (handler.IsLoggedIn)
        {
            handler.TryEnterPage();
            yield break;
        }
        yield return StartCoroutine(handler.RegisterEnumerator(displayNameField.text, passwordField.text));
        if (handler.IsLoggedIn)
        {
            handler.TryEnterPage();
            yield break;
        }
        yield return new WaitForSecondsRealtime(0.2f);
        button.interactable = true;
    }
}
