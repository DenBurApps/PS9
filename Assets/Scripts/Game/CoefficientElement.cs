using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoefficientElement : MonoBehaviour
{
    [SerializeField] private Gradient[] gradients;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image;

    private float _gradientPosition = 0.5f;
    
    [SerializeField] private float[] coefficientThresholds = { 1.1f, 2.0f, 3.0f, 5.0f };

    public void Enable(float coefficient)
    {
        _text.text = coefficient.ToString("F1") + "x";
        
        Gradient selectedGradient = SelectGradient(coefficient);
        if (selectedGradient != null)
        {
            ApplyGradient(selectedGradient);
        }

        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private Gradient SelectGradient(float coefficient)
    {
        for (int i = 0; i < coefficientThresholds.Length; i++)
        {
            if (coefficient < coefficientThresholds[i])
            {
                return gradients[i];
            }
        }
        
        return coefficient > coefficientThresholds[coefficientThresholds.Length - 1] ? gradients[gradients.Length - 1] : null;
    }

    private void ApplyGradient(Gradient gradient)
    {
        _image.color = gradient.Evaluate(_gradientPosition);
    }
}