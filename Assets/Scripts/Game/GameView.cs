using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class GameView : MonoBehaviour
{
    [SerializeField] private Button _menuButton;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action MenuOpened;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _menuButton.onClick.AddListener(OnMenuClicked);
    }

    private void OnDisable()
    {
        _menuButton.onClick.RemoveListener(OnMenuClicked);
    }

    public void SetTransparent()
    {
        _screenVisabilityHandler.SetTransperent();
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Diasble()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnMenuClicked() => MenuOpened?.Invoke();
}
