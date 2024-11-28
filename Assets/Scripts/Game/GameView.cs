using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class GameView : MonoBehaviour
{
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _buyButton;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action MenuOpened;
    public event Action BuyClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _menuButton.onClick.AddListener(OnMenuClicked);
        _buyButton.onClick.AddListener(OnBuyClicked);
    }

    private void OnDisable()
    {
        _menuButton.onClick.RemoveListener(OnMenuClicked);
        _buyButton.onClick.RemoveListener(OnBuyClicked);
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

    public bool CheckEnablement()
    {
        return _screenVisabilityHandler.IsTransparent;
    }

    private void OnMenuClicked() => MenuOpened?.Invoke();
    private void OnBuyClicked() => BuyClicked?.Invoke();
}
