using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoefficientHolder : MonoBehaviour
{
    [SerializeField] private List<CoefficientElement> _coefficientElements;

    private List<int> _availableIndexes = new List<int>();
    
    private void Start()
    {
        for (int i = 0; i < _coefficientElements.Count; i++)
        {
            _coefficientElements[i].Disable();
            _availableIndexes.Add(i);
        }
    }
    
    public void EnableElement(float coefficient)
    {
        if (_availableIndexes.Count == 0)
        {
            DisableAllElements();
        }
        
        if (_availableIndexes.Count > 0)
        {
            int indexToEnable = _availableIndexes[_availableIndexes.Count - 1];
            _availableIndexes.RemoveAt(_availableIndexes.Count - 1);

            if (indexToEnable >= 0 && indexToEnable < _coefficientElements.Count)
            {
                _coefficientElements[indexToEnable].Enable(coefficient);
            }
        }
    }
    
    private void DisableAllElements()
    {
        for (int i = 0; i < _coefficientElements.Count; i++)
        {
            _coefficientElements[i].Disable();
            if (!_availableIndexes.Contains(i))
            {
                _availableIndexes.Add(i);
            }
        }
    }
}
