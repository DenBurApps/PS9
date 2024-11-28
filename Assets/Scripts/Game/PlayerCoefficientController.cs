using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCoefficientController : MonoBehaviour
{
    private const float InitCoefficient = 2f;
    private const float MinCoefficient = 1.1f;
    private const float MaxCoefficient = 50.0f;

    [SerializeField] private Button _button11;
    [SerializeField] private Button _button12;
    [SerializeField] private Button _button15;
    [SerializeField] private Button _button2;
    [SerializeField] private TMP_InputField _input;

    private float _currentCoefficient;

    public float CurrentCoefficient => _currentCoefficient;

    private void OnEnable()
    {
        _button11.onClick.AddListener(() => SetCoefficient(1.1f));
        _button12.onClick.AddListener(() => SetCoefficient(1.2f));
        _button15.onClick.AddListener(() => SetCoefficient(1.5f));
        _button2.onClick.AddListener(() => SetCoefficient(2f));

        _input.onValueChanged.AddListener(ValidateAndSetInputCoefficient);
    }

    private void OnDisable()
    {
        _button11.onClick.RemoveListener(() => SetCoefficient(1.1f));
        _button12.onClick.RemoveListener(() => SetCoefficient(1.2f));
        _button15.onClick.RemoveListener(() => SetCoefficient(1.5f));
        _button2.onClick.RemoveListener(() => SetCoefficient(2f));

        _input.onValueChanged.RemoveListener(ValidateAndSetInputCoefficient);
    }

    private void Start()
    {
        SetToDefault();
    }

    public void ToggleAllButtons(bool status)
    {
        _button11.interactable = status;
        _button12.interactable = status;
        _button15.interactable = status;
        _button2.interactable = status;
        _input.interactable = status;
    }
    
    public void SetToDefault()
    {
        _currentCoefficient = InitCoefficient;
        UpdateCoefficientText();
    }

    private void SetCoefficient(float coefficient)
    {
        _currentCoefficient = Mathf.Clamp(coefficient, MinCoefficient, MaxCoefficient);
        UpdateCoefficientText();
    }

    private void UpdateCoefficientText()
    {
        _input.text = _currentCoefficient.ToString("F1");
    }

    private void ValidateAndSetInputCoefficient(string input)
    {
        float parsedCoefficient;

        if (float.TryParse(input, out parsedCoefficient))
        {
            _currentCoefficient = Mathf.Clamp(parsedCoefficient, MinCoefficient, MaxCoefficient);
        }
        else
        {
            _input.text = _currentCoefficient.ToString("F1");
        }

        UpdateCoefficientText();
    }
}
