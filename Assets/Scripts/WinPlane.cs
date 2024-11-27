using System;
using TMPro;
using UnityEngine;

public class WinPlane : MonoBehaviour
{
    [SerializeField] private TMP_Text _winText;
    [SerializeField] private TMP_Text _betText;
    [SerializeField] private TMP_Text _coeffText;

    public WinData WinData { get; private set; }
    public bool IsActive { get; private set; }

    public void Enable(WinData data)
    {
        if (data == null)
        {
            Debug.LogError("null");
        }
        
        gameObject.SetActive(true);
        WinData = data;

        _betText.text = WinData.Bet;
        _winText.text = WinData.Win;
        _coeffText.text = WinData.Coefficient;
        IsActive = true;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        IsActive = false;
    }

    public void ResetData()
    {
        if (WinData != null)
            WinData = null;
        
        _winText.text = string.Empty;
        _betText.text = string.Empty;
        _coeffText.text = string.Empty;
    }

    public void UpdateUI()
    {
        if(WinData == null)
            return;
        
        _betText.text = WinData.Bet;
        _winText.text = WinData.Win;
        _coeffText.text = WinData.Coefficient;
    }
}

[Serializable]
public class WinData
{
    public string Win;
    public string Bet;
    public string Coefficient;

    public WinData(string win, string bet, string coefficient)
    {
        Win = win;
        Bet = bet;
        Coefficient = coefficient;
    }
}