using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MSES_GameController : MonoBehaviour
{

    public void Back_OnClick()
    {
        SceneManager.LoadScene("1_MainScreenSingleton");
    }

}
