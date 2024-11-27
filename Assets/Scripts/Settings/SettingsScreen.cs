using System;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class SettingsScreen : MonoBehaviour
{
    [SerializeField] private SettingsScreenView _view;
    [SerializeField] private VersionView _versionView;
    [SerializeField] private PrivacyPolicyView _privacyPolicyView;
    [SerializeField] private TermsOfUseView _termsOfUseView;
    [SerializeField] private GameController _gameController;
    
    private string _email = "w63044073@gmail.com";

    public event Action BackButtonClicked;
    public event Action WindowOpened;
    public event Action SettingsOpen;

    private void Start()
    {
        _view.Disable();
    }

    private void OnEnable()
    {
        _view.FeedbackButtonClicked += ProcessFeedbackButtonClicked;
        _view.VersionButtonClicked += ProcessVersionButtonClicked;
        _view.TermsOfUseButtonClicked += ProcessTermsOfUseButtonClicked;
        _view.PrivacyPolicyButtonClicked += ProcessPrivicyPolicyButtonClicked;
        _view.BackButtonClicked += ProcessBackButtonClicked;
        
        _view.ContactUsButtonClicked += ProcessContactUsButtonClicked;

        _privacyPolicyView.BackButtonClicked += ShowScreen;
        _versionView.BackButtonClicked += ShowScreen;
        _termsOfUseView.BackButtonClicked += ShowScreen;
        
        _gameController.MenuOpened += ShowScreen;
    }

    private void OnDisable()
    {
        _view.FeedbackButtonClicked -= ProcessFeedbackButtonClicked;
        _view.VersionButtonClicked -= ProcessVersionButtonClicked;
        _view.TermsOfUseButtonClicked -= ProcessTermsOfUseButtonClicked;
        _view.PrivacyPolicyButtonClicked -= ProcessPrivicyPolicyButtonClicked;
        _view.BackButtonClicked -= ProcessBackButtonClicked;
        
        _view.ContactUsButtonClicked -= ProcessContactUsButtonClicked;
        
        _privacyPolicyView.BackButtonClicked -= ShowScreen;
        _versionView.BackButtonClicked -= ShowScreen;
        _termsOfUseView.BackButtonClicked -= ShowScreen;
        
        _gameController.MenuOpened -= ShowScreen;
    }

    private void ProcessBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        _view.Disable();
    }

    public void ShowScreen()
    {
        _view.Enable();
        SettingsOpen?.Invoke();
    }

    private void ProcessPrivicyPolicyButtonClicked()
    {
        _privacyPolicyView.Enable();
        _view.Disable();
        WindowOpened?.Invoke();
    }

    private void ProcessTermsOfUseButtonClicked()
    {
       _termsOfUseView.Enable();
        _view.Disable();
        WindowOpened?.Invoke();
    }

    private void ProcessVersionButtonClicked()
    {
        _versionView.Enable();
        _view.Disable();
        WindowOpened?.Invoke();
    }

    private void ProcessFeedbackButtonClicked()
    {
#if UNITY_IOS
        Device.RequestStoreReview();
#endif
    }

    private void ProcessContactUsButtonClicked()
    {
        Application.OpenURL("mailto:" + _email + "?subject=Mail to developer");
    }
}
