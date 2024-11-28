using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _errorTextField;
    [SerializeField] private Button _closeButton;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(Disable);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Disable);
    }

    private void Start()
    {
        Disable();
    }

    public void Enable(string errorText)
    {
        gameObject.SetActive(true);
        _errorTextField.text = errorText;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        _errorTextField.text = string.Empty;
    }
}
