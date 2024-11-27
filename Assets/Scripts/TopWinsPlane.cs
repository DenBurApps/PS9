using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class TopWinsPlane : MonoBehaviour
{
    [SerializeField] private List<WinPlane> _winPlanes;
    [SerializeField] private GameObject _emptyHistoryPlane;
    [SerializeField] private GameObject _winScroll;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    private readonly List<int> _availableIndexes = new List<int>();
    private List<WinData> _lastSortedData = new List<WinData>();
    
    private string SaveFilePath => Path.Combine(Application.persistentDataPath, "winPlanesData.json");

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void Start()
    {
        DisableAllElements();
        
        LoadData();
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
        ToggleEmptyPlane();
        SortAllElements();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }
    
    public void EnableElement(WinData data)
    {
        if (data == null)
        {
            Debug.LogWarning("Attempted to enable with null WinData.");
            return;
        }

        bool dataExists = _winPlanes.Any(plane => plane.IsActive && plane.WinData != null && plane.WinData.Win.Equals(data.Win, StringComparison.CurrentCultureIgnoreCase));
        if (dataExists)
        {
            return;
        }

        if (_availableIndexes.Count > 0)
        {
            int indexToEnable = _availableIndexes[0];
            _availableIndexes.RemoveAt(0);

            if (indexToEnable < _winPlanes.Count)
            {
                _winPlanes[indexToEnable].Enable(data);
            }

            ToggleEmptyPlane();
            SortAllElements();
        }
        
        SaveData();
    }

    private void SortAllElements()
    {
        if (_availableIndexes.Count == _winPlanes.Count)
            return;

        var sortedWinData = _winPlanes
            .Where(plane => plane.WinData != null)
            .OrderByDescending(plane => int.TryParse(plane.WinData.Win, out int win) ? win : 0)
            .Select(plane => plane.WinData)
            .ToList();

       if (_lastSortedData.SequenceEqual(sortedWinData))
            return;

        _lastSortedData = new List<WinData>(sortedWinData);

        for (int i = 0; i < _winPlanes.Count; i++)
        {
            if (i < sortedWinData.Count)
            {
                if (_winPlanes[i].WinData != sortedWinData[i])
                {
                    _winPlanes[i].Enable(sortedWinData[i]);
                    _winPlanes[i].UpdateUI();
                }
            }
            else
            {
                _winPlanes[i].Disable();
            }
        }
        
        SaveData();
    }

    private void ToggleEmptyPlane()
    {
        bool isActive = _availableIndexes.Count == _winPlanes.Count;

        _winScroll.SetActive(!isActive);
        _emptyHistoryPlane.SetActive(isActive);
    }

    private void DisableAllElements()
    {
        _availableIndexes.Clear();

        for (int i = 0; i < _winPlanes.Count; i++)
        {
            _winPlanes[i].ResetData();
            _winPlanes[i].Disable();
            _availableIndexes.Add(i);
        }
    }

    private void SaveData()
    {
        List<WinData> winDataList = _winPlanes
            .Where(plane => plane.WinData != null)
            .Select(plane => plane.WinData)
            .ToList();

        string json = JsonUtility.ToJson(new WinDataListWrapper(winDataList), true);
        File.WriteAllText(SaveFilePath, json);
    }
    
    private void LoadData()
    {
        if (!File.Exists(SaveFilePath))
        {
            return;
        }

        string json = File.ReadAllText(SaveFilePath);
        WinDataListWrapper dataWrapper = JsonUtility.FromJson<WinDataListWrapper>(json);

        if (dataWrapper?.WinDataList != null)
        {
            DisableAllElements();
            foreach (WinData data in dataWrapper.WinDataList)
            {
                EnableElement(data);
            }
        }
    }
    
    [Serializable]
    private class WinDataListWrapper
    {
        public List<WinData> WinDataList;

        public WinDataListWrapper(List<WinData> winDataList)
        {
            WinDataList = winDataList;
        }
    }
}