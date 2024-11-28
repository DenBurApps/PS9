using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class BuyScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerBalanceText;
    [SerializeField] private PlayerBalanceController _playerBalanceController;
    [SerializeField] private SettingsScreen _settingsScreen;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    private bool _fromSettings;

    public event Action ShowMenu;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void Start()
    {
        Disable();
    }

    private void OnEnable()
    {
        _settingsScreen.BackButtonClicked += CheckEnablement;
        _settingsScreen.SettingsOpen += ValidateSetTransparent;
        _settingsScreen.WindowOpened += ValidateDisable;
    }

    private void OnDisable()
    {
        _settingsScreen.BackButtonClicked -= CheckEnablement;
        _settingsScreen.SettingsOpen -= ValidateSetTransparent;
        _settingsScreen.WindowOpened -= ValidateDisable;
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
        _playerBalanceText.text = _playerBalanceController.Balance.ToString();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void OnShowMenu()
    {
        _screenVisabilityHandler.SetTransperent();
        ShowMenu?.Invoke();
        _fromSettings = true;
    }

    public void CheckEnablement()
    {
        if (_fromSettings)
            Enable();

        _fromSettings = false;
    }
    
    private void ValidateSetTransparent()
    {
        if (!_fromSettings)
            return;

        _screenVisabilityHandler.SetTransperent();
    }

    private void ValidateDisable()
    {
        if (!_fromSettings)
            return;

        Disable();
    }
}