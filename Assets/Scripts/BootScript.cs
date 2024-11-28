using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootScript : MonoBehaviour
{
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Onboarding"))
        {
            SceneManager.LoadScene("Onboarding");
        }
        else
        {
            SceneManager.LoadScene("MainGame");
        }
    }
}
