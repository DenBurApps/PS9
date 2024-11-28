using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenVisabilityHandler : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    public bool IsTransparent = false;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void DisableScreen()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        IsTransparent = false;
    }

    public void EnableScreen()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        IsTransparent = false;
    }

    public void SetTransperent()
    {
        IsTransparent = true;
        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0.01f;
        _canvasGroup.blocksRaycasts = false;
    }
}