using UnityEngine;
using UnityEngine.SceneManagement;

public class MSE_MainScreenController : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject settingsScreen;

    void Start()
    {
        BackToMain_OnClick();
    }

    public void Start_OnClick()
    {
        SceneManager.LoadScene("2_GameScreen");
    }

    public void Settings_OnClick()
    {
        mainScreen.SetActive(false);
        settingsScreen.SetActive(true);
        FindObjectOfType<MSE_LanguageDropdown>().Refresh();
    }

    public void BackToMain_OnClick()
    {
        mainScreen.SetActive(true);
        settingsScreen.SetActive(false);
    }
}
