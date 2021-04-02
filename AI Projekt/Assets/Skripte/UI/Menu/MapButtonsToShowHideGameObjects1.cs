using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButtonsToShowHideGameObjects1 : MonoBehaviour
{
    [SerializeField] private UIToButtonObject[] mapper;

    void Start()
    {
        foreach (UIToButtonObject map in mapper)
        {
            map.button.onClick.AddListener(delegate { Show(map.ui); });
        }
    }


    private void Show(GameObject UIObject)
    {
        Flush();
        UIObject.SetActive(true);
    }

    private void Flush()
    {
        foreach (UIToButtonObject map in mapper)
        {
            map.ui.SetActive(false);
        }
    }
}
