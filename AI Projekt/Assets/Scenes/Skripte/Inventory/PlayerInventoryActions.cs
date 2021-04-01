using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInventoryActions : MonoBehaviour
{
    [Inject(Id="Player")]private PlayerStorage inventory;
    public SpriteRenderer holdingItemRenderer;

    private void Start()
    {
        inventory.OnItemFocusUpdated += SetHoldingItem;
    }

    private void SetHoldingItem()
    {
        int slotNumber = inventory.GetFocusedSlotNumber();
        IGetStorageSlot storageSlotGetter = inventory.GetItemSlotValues(slotNumber);
        if (storageSlotGetter != null && storageSlotGetter.GetItem() != null)
        {
            holdingItemRenderer.gameObject.SetActive(true);
            holdingItemRenderer.sprite = storageSlotGetter.GetItem().picture;
        }
        else
        {
            holdingItemRenderer.gameObject.SetActive(false);
        }
    }
}
