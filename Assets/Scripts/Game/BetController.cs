using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetController : MonoBehaviour
{
    private const int InitBet = 50;

    [SerializeField] private PlayerBalanceController _balanceController;

    [SerializeField] private Button _increaseButton;
    [SerializeField] private Button _decreaceButton;
    [SerializeField] private TMP_InputField _betInput;
    [SerializeField] private Button _halfButton;
    [SerializeField] private Button _allInButton;
    [SerializeField] private Button _doubleButton;

    private int _currentBet;

    public int CurrentBet => _currentBet;

    private void OnEnable()
    {
        _allInButton.onClick.AddListener(OnAllInButtonClicked);
        _decreaceButton.onClick.AddListener(OnDecreaseButtonClicked);
        _increaseButton.onClick.AddListener(OnIncreaceButtonClicked);
        _doubleButton.onClick.AddListener(OnDoubleButtonClicked);
        _halfButton.onClick.AddListener(OnHalfButtonClicked);

        _betInput.onValueChanged.AddListener(ProcessBetInput);
    }

    private void OnDisable()
    {
        _allInButton.onClick.RemoveListener(OnAllInButtonClicked);
        _decreaceButton.onClick.RemoveListener(OnDecreaseButtonClicked);
        _increaseButton.onClick.RemoveListener(OnIncreaceButtonClicked);
        _doubleButton.onClick.RemoveListener(OnDoubleButtonClicked);
        _halfButton.onClick.RemoveListener(OnHalfButtonClicked);

        _betInput.onValueChanged.RemoveListener(ProcessBetInput);
    }

    private void Start()
    {
        SetDefaultBet();
    }

    private void SetDefaultBet()
    {
        _currentBet = InitBet;
        UpdateText();
        ValidateDecreaseButton();
    }

    public void ToggleAllButtons(bool status)
    {
        _allInButton.interactable = status;
        _halfButton.interactable = status;
        _doubleButton.interactable = status;
        _betInput.interactable = status;
    }

    private void ProcessBetInput(string input)
    {
        int parsedBet;

        if (int.TryParse(input, out parsedBet))
        {
            parsedBet = Mathf.Clamp(parsedBet, InitBet, int.MaxValue);
            _currentBet = parsedBet;
        }
        else
        {
            _betInput.text = _currentBet.ToString();
        }

        UpdateText();
        ValidateDecreaseButton();
    }

    private void OnDecreaseButtonClicked()
    {
        if (_currentBet - 50 < InitBet)
            return;

        _currentBet -= 50;
        UpdateText();
        ValidateDecreaseButton();
    }

    private void OnIncreaceButtonClicked()
    {
        _currentBet += 50;
        UpdateText();
        ValidateDecreaseButton();
    }

    private void OnAllInButtonClicked()
    {
        if (_balanceController.Balance <= InitBet)
        {
            return;
        }
        
        _currentBet = _balanceController.Balance;
        UpdateText();
        ValidateDecreaseButton();
    }

    private void ValidateDecreaseButton()
    {
        _decreaceButton.interactable = _currentBet > InitBet;
    }

    private void OnHalfButtonClicked()
    {
        if (InitBet > _currentBet / 2)
        {
            _currentBet = InitBet;
            UpdateText();
            return;
        }

        _currentBet /= 2;
        UpdateText();
        ValidateDecreaseButton();
    }

    private void OnDoubleButtonClicked()
    {
        _currentBet *= 2;
        UpdateText();
        ValidateDecreaseButton();
    }

    private void UpdateText()
    {
        _betInput.text = _currentBet.ToString();
    }
}