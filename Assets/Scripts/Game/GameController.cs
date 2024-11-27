using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayField _playField;
    [SerializeField] private float growthSpeed = 0.01f;
    [SerializeField] private PlayerBalanceController _playerBalanceController;
    [SerializeField] private PlayerCoefficientController _playerCoefficientController;
    [SerializeField] private Button _makeBetButton;
    [SerializeField] private Button _cashOutButton;
    [SerializeField] private BetController _betController;
    [SerializeField] private CoefficientHolder _coefficientHolder;
    [SerializeField] private TopWinsPlane _topWinsPlane;
    [SerializeField] private NotEnoughScreen _notEnoughScreen;
    [SerializeField] private GameView _view;
    [SerializeField] private AudioSource _winSound;
    [SerializeField] private AudioSource _loseSound;
    [SerializeField] private SettingsScreen _settingsScreen;
    
    private float _initialMultiplier = 1.0f;
    private bool _isCrashed = false;
    private bool _betPlaced = false;
    private bool _manualCashOut = false;
    private int _currentPlayerBet;
    private float _toleranceRange = 0.05f;

    private IEnumerator _countdownCoroutine;
    private IEnumerator _gameCoroutine;

    public event Action MenuOpened;

    private void OnEnable()
    {
        _makeBetButton.onClick.AddListener(PlaceBet);
        _notEnoughScreen.CloseButtonClicked += ResetGame;
        
        _cashOutButton.onClick.AddListener(CashOut);

        _view.MenuOpened += OnMenuClicked;
        _settingsScreen.BackButtonClicked += ContinueGame;
        _settingsScreen.WindowOpened += _view.Diasble;
        _settingsScreen.SettingsOpen += _view.SetTransparent;
    }

    private void OnDisable()
    {
        _makeBetButton.onClick.RemoveListener(PlaceBet);
        _notEnoughScreen.CloseButtonClicked -= ResetGame;
        
        _cashOutButton.onClick.RemoveListener(CashOut);
        
        _view.MenuOpened -= OnMenuClicked;
        _settingsScreen.BackButtonClicked -= ContinueGame;
        _settingsScreen.WindowOpened -= _view.Diasble;
        _settingsScreen.SettingsOpen -= _view.SetTransparent;
    }

    private void Start()
    {
        StartCountdown();
        ValidateInput();
    }

    private void StartCountdown()
    {
        StopCountdown();

        _countdownCoroutine = CountdownCoroutine();
        StartCoroutine(_countdownCoroutine);
    }

    private void StopCountdown()
    {
        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }
    }

    private void StartGame()
    {
        StopGame();
        _gameCoroutine = IncreaseMultiplierCoroutine();
        StartCoroutine(_gameCoroutine);
    }

    private void StopGame()
    {
        if (_gameCoroutine != null)
        {
            StopCoroutine(_gameCoroutine);
            _gameCoroutine = null;
        }
    }

    private void PlaceBet()
    {
        if (_betController.CurrentBet > _playerBalanceController.Balance)
        {
            _notEnoughScreen.Enable();
            _view.SetTransparent();
            StopCountdown();
            StopGame();
            return;
        }

        _currentPlayerBet = _betController.CurrentBet;
        _playerBalanceController.DecreaseBalance(_currentPlayerBet);

        _betPlaced = true;
        ValidateInput();
    }

    private void ValidateInput()
    {
        bool buttonsInteractable = !_betPlaced;
        _betController.ToggleAllButtons(buttonsInteractable);
        _playerCoefficientController.ToggleAllButtons(buttonsInteractable);
        _makeBetButton.interactable = buttonsInteractable;
        _cashOutButton.gameObject.SetActive(_betPlaced);
    }
    
    private void CashOut()
    {
        if (_betPlaced && !_isCrashed)
        {
            _manualCashOut = true;
            ProcessWin(_playerCoefficientController.CurrentCoefficient);
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        int countdown = 10;
        WaitForSeconds interval = new WaitForSeconds(1);

        while (countdown > 0)
        {
            _playField.SetCountdown(countdown.ToString());
            yield return interval;
            countdown--;

            if (_betPlaced)
            {
                break;
            }
        }

        StartGame();
    }

    private IEnumerator IncreaseMultiplierCoroutine()
    {
        float currentMultiplier = _initialMultiplier;
        _playField.StartRotating();
        _isCrashed = false;

        while (!_isCrashed && currentMultiplier < _playerCoefficientController.MaxPossibleCoefficient)
        {
            currentMultiplier += Time.deltaTime * growthSpeed;
            _playField.SetCoefficient(currentMultiplier);
            
            if (_manualCashOut)
            {
                ProcessWin(currentMultiplier);
                yield break;
            }
            
            /*if (_betPlaced && Mathf.Abs(currentMultiplier - _playerCoefficientController.CurrentCoefficient) <= _toleranceRange)
            {
                ProcessWin(currentMultiplier);
                StopGame();
                yield break;
            }*/
            
            if (ShouldCrash(currentMultiplier))
            {
                if (_betPlaced)
                {
                    _isCrashed = true;
                    _playField.StopRotating();
                    _coefficientHolder.EnableElement(currentMultiplier);
                    ProcessLoss();
                }
                else
                {
                    _isCrashed = true;
                    _playField.StopRotating();
                    _coefficientHolder.EnableElement(currentMultiplier);
                    _playField.ReturnToDefaultRotation();
                    ResetGame();
                }

                StopGame();
                yield break;
            }

            yield return null;
        }
    }

    private bool ShouldCrash(float currentMultiplier)
    {
        float baseCrashProbability = Mathf.Clamp01(0.005f * Mathf.Pow(currentMultiplier - 1f, 1.05f));

        return Random.value < baseCrashProbability;
    }

    private void ProcessWin(float winMultiplier)
    {
        int winnings = Mathf.RoundToInt(_currentPlayerBet * winMultiplier);
        _playerBalanceController.IncreaseBalance(winnings);
        _playField.ToggleWinImage(true);
        ValidateInput();

        var winData = new WinData(_playerBalanceController.Balance.ToString(),
            _betController.CurrentBet.ToString(), winMultiplier.ToString("F1"));
        
        _topWinsPlane.EnableElement(winData);
        _coefficientHolder.EnableElement(winMultiplier);
        _winSound.Play();
        StopGame();
        ResetGame();
    }

    private void ProcessLoss()
    {
        _playField.ToggleLoseImage(true);
        _loseSound.Play();
        ResetGame();
    }

    private void ResetGame()
    {
        _betPlaced = false;
        _manualCashOut = false;
        ValidateInput();
        StartCountdown();
        _view.Enable();
    }

    private void OnMenuClicked()
    {
        MenuOpened?.Invoke();
        _view.SetTransparent();
        StopCountdown();
        StopGame();
    }

    private void ContinueGame()
    {
        _view.Enable();
        
        if (_gameCoroutine != null)
        {
            StartGame();
        }
        else
        {
            StartCountdown();
        }
    }
}