using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class SettingsAudioButton : MonoBehaviour
{
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    
    private Transform _transform;
    private float _initZRotation;
    private float _targetZRotation = 180f;
    private Button _button;
    private Image _image;
    
    public bool IsClicked { get; private set; }

    private void Awake()
    {
        _transform = transform;
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    private void Start()
    {
        _initZRotation = _transform.rotation.z;
        IsClicked = false;
        _image.color = _defaultColor;
        _audioMixerGroup.audioMixer.SetFloat("Sound", -20);
    }

    public void SetDefault()
    {
        _transform.rotation = Quaternion.Euler(_transform.eulerAngles.x, _transform.eulerAngles.y, _initZRotation);
    }

    public void SetRotated()
    {
        _transform.rotation = Quaternion.Euler(_transform.eulerAngles.x, _transform.eulerAngles.y, _targetZRotation);
    }

    private void OnButtonClicked()
    {
        if (!IsClicked)
        {
            SetRotated();
            _image.color = _selectedColor;
            IsClicked = true;
            _audioMixerGroup.audioMixer.SetFloat("Sound", -80);
        }
        else
        {
            SetDefault();
            _image.color = _defaultColor;
            IsClicked = false;
            _audioMixerGroup.audioMixer.SetFloat("Sound", -20);
        }
        
    }
}