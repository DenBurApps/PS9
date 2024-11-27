using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayField : MonoBehaviour
{
    [SerializeField] private TMP_Text _valueText;
    [SerializeField] private Image _coin;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _winImage;
    [SerializeField] private Image _loseImage;

    private IEnumerator _coinRotationCoroutine;

    private Quaternion _initCoinRotation;

    private void Start()
    {
        ToggleLoseImage(false);
        ToggleWinImage(false);

        _initCoinRotation = _coin.transform.rotation;
    }

    public void StartRotating()
    {
        ToggleLoseImage(false);
        ToggleWinImage(false);
        StopRotating();

        _coinRotationCoroutine = RotateCoinCoroutine();
        StartCoroutine(_coinRotationCoroutine);
    }

    public void StopRotating()
    {
        if (_coinRotationCoroutine != null)
        {
            StopCoroutine(_coinRotationCoroutine);
            _coinRotationCoroutine = null;
        }
    }

    public void SetCountdown(string value)
    {
        _valueText.text = value + "s";
    }

    public void SetCoefficient(float value)
    {
        _valueText.text = value.ToString("F1") + "x";
    }

    public void ToggleLoseImage(bool status)
    {
        _loseImage.gameObject.SetActive(status);
    }

    public void ToggleWinImage(bool status)
    {
        _winImage.gameObject.SetActive(status);
    }

    public void ReturnToDefaultRotation()
    {
        _coin.gameObject.transform.rotation = _initCoinRotation;
    }

    private IEnumerator RotateCoinCoroutine()
    {
        while (true)
        {
            _coin.transform.Rotate(0, 0, 200 * Time.deltaTime);
            yield return null;
        }
    }
}
