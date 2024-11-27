using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class NotEnoughScreen : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _buyButton;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action CloseButtonClicked;
    public event Action BuyButtonClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(OnCloseClicked);
        _buyButton.onClick.AddListener(OnBuyClicked);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(OnCloseClicked);
        _buyButton.onClick.RemoveListener(OnBuyClicked);
    }

    private void Start()
    {
        Disable();
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnCloseClicked()
    {
        CloseButtonClicked?.Invoke();
        Disable();
    }

    private void OnBuyClicked()
    {
        BuyButtonClicked?.Invoke();
        Disable();
    }
}
