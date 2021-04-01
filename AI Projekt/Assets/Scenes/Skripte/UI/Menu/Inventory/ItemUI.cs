using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI amountText;
    public ItemUIProperties Properties { private set; get; }
    [SerializeField]private GameObject focusObject;

    public void InitiateItemUI(int id)
    {
        Properties = new ItemUIProperties(id);
        UISwitch(false);
    }

    public void SetProperties(ItemObject _item, int _amount)
    {  
        if (Properties != null)
        {
            Properties.SetAttributes(_item, _amount);
            if (_item != null)
            {
                image.sprite = Properties.Sprite;
                amountText.SetText(Properties.Amount.ToString());
                UISwitch(true);
            }
            else
            {
                UISwitch(false);
            }
        }
        
    }

    public void SwitchProperties(ItemUI other)
    {
        ItemObject itemA = other.Properties.Item;
        int amount = other.Properties.Amount;
        other.SetProperties(Properties.Item, Properties.Amount);
        SetProperties(itemA, amount);
    }
    
    public void UISwitch(bool boolean)
    {
        image.gameObject.SetActive(boolean);
        amountText.gameObject.SetActive(boolean);
        
    }

    public void SwitchFocus(bool boolean)
    {
        focusObject.SetActive(boolean);
    }
}