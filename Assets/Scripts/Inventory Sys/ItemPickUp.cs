using UnityEngine;

[RequireComponent (typeof(ItemController))]
public class ItemPickup : MonoBehaviour
{
    private ItemController _item;
    private void Awake()
    {
        _item = GetComponent<ItemController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(A.Tags.player))
        {
            InventoryManager.Instance_PlayerInventory._AddItem(_item);
        }
    }
}