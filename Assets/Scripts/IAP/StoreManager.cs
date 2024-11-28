using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    private const string InitializationFailMessage =
        "We couldn't connect to our purchase services. Please check your internet connection and try again.";

    private const string PurchaseFailMessage =
        "Something went wrong with your purchase. Please check your payment details and try again.";
    
    [SerializeField] private List<int> _products = new List<int>();
    [SerializeField] private AppHudManager _apphudManager;
    [SerializeField] private PlayerBalanceController _playerBalanceController;
    [SerializeField] private ErrorScreen _errorScreen;

    private void OnEnable()
    {
        _apphudManager.PurchaseComplete += OnPurchaseComplete;
        _apphudManager.PurchaseFailed += OnPurchaseFailed;
        _apphudManager.InitializationFailed += OnInitializationFailed;
    }

    private void OnDisable()
    {
        _apphudManager.PurchaseComplete -= OnPurchaseComplete;
        _apphudManager.PurchaseFailed -= OnPurchaseFailed;
        _apphudManager.InitializationFailed -= OnInitializationFailed;
    }

    private void OnPurchaseComplete(int index)
    {
        _playerBalanceController.IncreaseBalance(_products[index]);
    }

    private void OnInitializationFailed()
    {
        _errorScreen.Enable(InitializationFailMessage);
    }

    private void OnPurchaseFailed()
    {
        _errorScreen.Enable(PurchaseFailMessage);
    }
}
