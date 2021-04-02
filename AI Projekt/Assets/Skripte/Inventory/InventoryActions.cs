using System;
using UnityEngine;
using Zenject;

public class InventoryActions : MonoBehaviour
{
    private bool isObtainable;
    private PickUpItem pickUpItem;
   

    [Inject(Id = "Player")] PlayerStorage inventory;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            pickUpItem = collision.GetComponent<PickUpItem>();
            if (pickUpItem != null)
                isObtainable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Flush();
    }

    private void Update()
    {
        if (isObtainable && Input.GetButtonDown("Jump"))
        {
            StorageSlot item = pickUpItem.PickUp();
            inventory.AddItem(item.GetItem(), item.GetAmount());
            Flush();
        }
    }

    private void Flush()
    {
        isObtainable = false;
        pickUpItem = null;
    }
}
