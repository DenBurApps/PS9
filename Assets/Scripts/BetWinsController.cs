using UnityEngine;
using UnityEngine.UI;

public class BetWinsController : MonoBehaviour
{
   [SerializeField] private TopWinsPlane _topWinsPlane;
   [SerializeField] private GameObject _betPlane;
   [SerializeField] private GameObject _betButtonPlane;
   [SerializeField] private GameObject _winButtonPlane;
   [SerializeField] private Button _betButton;
   [SerializeField] private Button _topWinsButton;

   private void OnEnable()
   {
      _betButton.onClick.AddListener(OnBetClicked);
      _topWinsButton.onClick.AddListener(OnTopWinsClicked);
   }

   private void OnDisable()
   {
      _betButton.onClick.RemoveListener(OnBetClicked);
      _topWinsButton.onClick.RemoveListener(OnTopWinsClicked);
   }

   private void Start()
   {
      OnBetClicked();
   }

   private void OnBetClicked()
   {
      _betButtonPlane.gameObject.SetActive(true);
      _winButtonPlane.gameObject.SetActive(false);
      
      _topWinsPlane.Disable();
      _betPlane.gameObject.SetActive(true);
   }

   private void OnTopWinsClicked()
   {
      _betButtonPlane.gameObject.SetActive(false);
      _winButtonPlane.gameObject.SetActive(true);
      
      _topWinsPlane.Enable();
      _betPlane.gameObject.SetActive(false);
   }
}
