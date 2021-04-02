using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField]private StorageSlot item;

    public StorageSlot PickUp()
    {
        Destroy(gameObject);
        return item;
    }
}
