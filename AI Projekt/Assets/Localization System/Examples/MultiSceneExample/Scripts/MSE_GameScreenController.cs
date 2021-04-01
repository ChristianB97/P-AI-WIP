using UnityEngine;
using UnityEngine.SceneManagement;

public class MSE_GameScreenController : MonoBehaviour
{

    public void Back_OnClick()
    {
        SceneManager.LoadScene("1_MainScreen");
    }

}
