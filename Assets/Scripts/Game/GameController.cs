using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayField _playField;
    [SerializeField] private float growthSpeed = 1.0f;
    [SerializeField] private PlayerBalanceController _playerBalanceController;
    [SerializeField] private PlayerCoefficientController _playerCoefficientController;
    [SerializeField] private Button _makeBetButton;
    [SerializeField] private BetController _betController;
    [SerializeField] private CoefficientHolder _coefficientHolder;
    [SerializeField] private TopWinsPlane _topWinsPlane;
    [SerializeField] private NotEnoughScreen _notEnoughScreen;
    [SerializeField] private GameView _view;
    [SerializeField] private AudioSource _winSound;
    [SerializeField] private AudioSource _loseSound;
    [SerializeField] private SettingsScreen _settingsScreen;
    [SerializeField] private BuyScreen _buyScreen;

    private float _initialMultiplier = 0f;
    private bool _isCrashed = false;
    private bool _betPlaced = false;
    private int _currentPlayerBet;
    private bool _fromSettings = false;

    private Coroutine _countdownCoroutine;
    private Coroutine _gameCoroutine;

    public event Action MenuOpened;

    private void OnEnable()
    {
        _makeBetButton.onClick.AddListener(PlaceBet);
        _notEnoughScreen.CloseButtonClicked += ResetGame;
        _notEnoughScreen.BuyButtonClicked += OnBuyCrystalsClicked;

        _view.BuyClicked += OnBuyCrystalsClicked;
        _view.MenuOpened += OnMenuClicked;
        _settingsScreen.BackButtonClicked += CheckEnablement;
        _settingsScreen.WindowOpened += ValidateDisable;
        _settingsScreen.SettingsOpen += ValidateSetTransparent;
    }

    private void OnDisable()
    {
        _makeBetButton.onClick.RemoveListener(PlaceBet);
        _notEnoughScreen.CloseButtonClicked -= ResetGame;
        _notEnoughScreen.BuyButtonClicked -= OnBuyCrystalsClicked;

        _view.BuyClicked -= OnBuyCrystalsClicked;
        _view.MenuOpened -= OnMenuClicked;
        _settingsScreen.BackButtonClicked -= CheckEnablement;
        _settingsScreen.WindowOpened -= ValidateDisable;
        _settingsScreen.SettingsOpen -= ValidateSetTransparent;
    }

    private void Start()
    {
        ResetGame();
    }

    private void ValidateSetTransparent()
    {
        if (!_fromSettings)
            return;

        _view.SetTransparent();
    }

    private void ValidateDisable()
    {
        if (!_fromSettings)
            return;

        _view.Diasble();
    }

    private void StartCountdown()
    {
        if (_countdownCoroutine != null) StopCoroutine(_countdownCoroutine);

        _countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }

    private void StartGame()
    {
        if (_gameCoroutine != null) StopCoroutine(_gameCoroutine);

        _gameCoroutine = StartCoroutine(IncreaseMultiplierCoroutine());
    }

    private void PlaceBet()
    {
        if (_betController.CurrentBet > _playerBalanceController.Balance)
        {
            _notEnoughScreen.Enable();
            _view.SetTransparent();
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

            if (_betPlaced) break;
        }

        StartGame();
    }

    private IEnumerator IncreaseMultiplierCoroutine()
    {
        float currentMultiplier = _initialMultiplier;
        _playField.StartRotating();
        _isCrashed = false;

        while (!_isCrashed)
        {
            currentMultiplier += growthSpeed * Time.deltaTime;
            _playField.SetCoefficient(currentMultiplier);

            if (ShouldCrash(currentMultiplier))
            {
                HandleCrash(currentMultiplier);
                yield break;
            }

            yield return null;
        }
    }

    private bool ShouldCrash(float currentMultiplier)
    {
        const float graceMultiplier = 0.3f;
        const float baseCrashProbability = 0.005f;
        const float probabilityGrowthRate = 1.0f;
        const float maxCrashProbability = 0.95f;

        if (currentMultiplier < graceMultiplier)
            return false;

        float adjustedMultiplier = currentMultiplier - graceMultiplier;
        float crashProbability =
            Mathf.Clamp01(baseCrashProbability * Mathf.Pow(adjustedMultiplier, probabilityGrowthRate));

        crashProbability = Mathf.Min(crashProbability, maxCrashProbability);

        return Random.value < crashProbability;
    }

    private void HandleCrash(float currentMultiplier)
    {
        if (_betPlaced)
        {
            if (currentMultiplier >= _playerCoefficientController.CurrentCoefficient)
            {
                ProcessWin(currentMultiplier);
            }
            else
            {
                ProcessLoss();
            }
        }

        _isCrashed = true;
        _playField.StopRotating();
        _coefficientHolder.EnableElement(currentMultiplier);
        _playField.ReturnToDefaultRotation();
        ResetGame();
    }

    private void ProcessWin(float winMultiplier)
    {
        int winnings = Mathf.RoundToInt(_currentPlayerBet * winMultiplier);
        _playerBalanceController.IncreaseBalance(winnings);

        _playField.ToggleWinImage(true);
        ValidateInput();

        var winData = new WinData(
            _playerBalanceController.Balance.ToString(),
            _currentPlayerBet.ToString(),
            winMultiplier.ToString("F1")
        );

        _topWinsPlane.EnableElement(winData);
        _winSound.Play();

        //  ResetGame();
    }

    private void ProcessLoss()
    {
        _playField.ToggleLoseImage(true);
        _loseSound.Play();
        //  ResetGame();
    }

    private void ResetGame()
    {
        StopCountdown();
        StopGame();

        _betPlaced = false;
        _isCrashed = false;
        ValidateInput();
        StartCountdown();
        _view.Enable();
    }

    private void StopCountdown()
    {
        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }
    }

    private void StopGame()
    {
        if (_gameCoroutine != null)
        {
            StopCoroutine(_gameCoroutine);
            _gameCoroutine = null;
        }
    }

    private void OnMenuClicked()
    {
        MenuOpened?.Invoke();
        _fromSettings = true;
        _view.SetTransparent();
        StopCountdown();
        StopGame();
    }

    private void OnBuyCrystalsClicked()
    {
        _buyScreen.Enable();
        _view.Diasble();
        StopCountdown();
        StopGame();
    }

    private void CheckEnablement()
    {
        if (!_fromSettings)
            return;

        _view.Enable();
        _fromSettings = false;

        if (_gameCoroutine != null)
        {
            StartGame();
        }
        else
        {
            StartCountdown();
        }
    }

    public void ContinueGame()
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