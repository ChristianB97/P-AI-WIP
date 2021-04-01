using UnityEngine;
using UnityEngine.UI;

public class MSE_Game_Clickeable : MonoBehaviour
{
    private int clicks = 0;

    public void OnMouseDown()
    {
        GameObject.Find("Clicks_Counter").GetComponent<Text>().text = (++clicks).ToString();
    }
}
