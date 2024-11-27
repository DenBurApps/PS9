using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class OnboardingPresenter : MonoBehaviour
{
    [SerializeField] private OnboardingView[] _screens;
    private int _currentScreenIndex = 0;

    private void Start()
    {
        for (int i = 0; i < _screens.Length; i++)
        {
            if (i == _currentScreenIndex)
                _screens[i].EnableScreen();
            else
                _screens[i].DisableScreen();
        }
    }

    private void OnEnable()
    {
        foreach (var screen in _screens)
        {
            screen.InteractableButtonClicked += OnScreenButtonClicked;
        }
    }

    private void OnDisable()
    {
        foreach (var screen in _screens)
        {
            screen.InteractableButtonClicked -= OnScreenButtonClicked;
        }
    }

    private void OnScreenButtonClicked()
    {
        _screens[_currentScreenIndex].DisableScreen();
        _currentScreenIndex++;

        if (_currentScreenIndex < _screens.Length)
        {
            _screens[_currentScreenIndex].EnableScreen();
        }
        else
        {
            PlayerPrefs.SetInt("Onboarding", 1);
            SceneManager.LoadScene("MainGame");
        }

        if (_currentScreenIndex == 2)
        {
#if UNITY_IOS
        Device.RequestStoreReview();
#endif
        }
    }
}