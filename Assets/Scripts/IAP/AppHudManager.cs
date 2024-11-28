using System;
using System.Collections;
using System.Collections.Generic;
using Apphud.Unity.Domain;
using Apphud.Unity.SDK;
using UnityEngine;

public class AppHudManager : MonoBehaviour
{
    private List<ApphudProduct> _apphudProducts = new List<ApphudProduct>();
    private int _currentIndex;
    public Action<int> PurchaseComplete;

    public event Action InitializationFailed;
    public event Action PurchaseFailed;

    void Start()
    {
        // ������������� AppHud
        ApphudSDK.Start("app_LwrNMNxwh5F9KfyfB4GHpBRdC9jeHK", OnApphudInitialize);
    }

    public void OnApphudInitialize(ApphudUser user)
    {
        ApphudSDK.LoadFallbackPaywalls(OnPaywallReceived);
    }

    private void OnPaywallReceived(List<ApphudPaywall> paywalls, ApphudError error)
    {
        if(error != null)
        {
            Debug.Log(error.Message);
            InitializationFailed?.Invoke();
            return;
        }
        _apphudProducts.Clear();
        _apphudProducts = paywalls[0].Products;
    }

    public void PurchaseProduct(int id)
    {
        _currentIndex = id;
        ApphudSDK.Purchase(_apphudProducts[id],null, null, null, false, OnPurchaseCompleted);
    }

    private void OnPurchaseCompleted(ApphudPurchaseResult purchase)
    {
        if (purchase != null)
        {
            Debug.Log($"Purchase successful");
            // ��������� �������� �������
            PurchaseComplete?.Invoke(_currentIndex);
        }
        else
        {
            PurchaseFailed?.Invoke();
            Debug.Log("Purchase failed");
        }
    }
}
