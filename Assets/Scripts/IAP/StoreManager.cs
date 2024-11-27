using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private List<int> _products = new List<int>();
    [SerializeField] private AppHudManager _apphudManager;
    [SerializeField] private PlayerBalanceController _playerBalanceController;

    private void OnEnable()
    {
        _apphudManager.PurchaseComplete += OnPurchaseComplete;
    }

    private void OnDisable()
    {
        _apphudManager.PurchaseComplete -= OnPurchaseComplete;
    }

    private void OnPurchaseComplete(int index)
    {
        _playerBalanceController.IncreaseBalance(_products[index]);
    }
}
