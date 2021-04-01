using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MSES_MainScreenController : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject settingsScreen;

    void Start()
    {
        BackToMain_OnClick();
    }

    public void Start_OnClick()
    {
        SceneManager.LoadScene("2_GameScreenSingleton");
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
