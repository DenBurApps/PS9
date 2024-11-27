using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class PlayerBalanceController : MonoBehaviour
{
    private const string SavePath = "Balance";
    private const int InitBalance = 500;

    [SerializeField] private TMP_Text _balance;

    private int _currentBalance;

    public int Balance => _currentBalance;

    private string _saveFilePath => Path.Combine(Application.persistentDataPath, SavePath);

    private void Start()
    {
        //LoadBalanceData();
        _currentBalance = InitBalance;
        UpdateBalanceText();
    }

    public void IncreaseBalance(int count)
    {
        _currentBalance += count;
        SaveBalanceData();
        UpdateBalanceText();
    }

    public void DecreaseBalance(int count)
    {
        if (_currentBalance - count >= 0)
        {
            _currentBalance -= count;
            SaveBalanceData();
        }
        else
        {
            Debug.Log("Not enough balance");
        }

        UpdateBalanceText();
        SaveBalanceData();
    }

    private void LoadBalanceData()
    {
        if (File.Exists(_saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(_saveFilePath);

                PlayerBalanceWrapper playerBalanceWrapper = JsonUtility.FromJson<PlayerBalanceWrapper>(json);

                _currentBalance = playerBalanceWrapper.Balance;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load balance: " + e);
                _currentBalance = InitBalance;
            }
        }
        else
        {
            _currentBalance = InitBalance;
        }
    }

    private void SaveBalanceData()
    {
        try
        {
            PlayerBalanceWrapper playerBalanceWrapper = new PlayerBalanceWrapper(_currentBalance);
            string json = JsonUtility.ToJson(playerBalanceWrapper);

            File.WriteAllText(_saveFilePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save balance: " + e);
        }
    }

    private void UpdateBalanceText()
    {
        _balance.text = _currentBalance.ToString();
    }
}

[Serializable]
public class PlayerBalanceWrapper
{
    public int Balance;

    public PlayerBalanceWrapper(int balance)
    {
        Balance = balance;
    }
}