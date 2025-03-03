using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] ItemData _item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(A.Tags.player))
        {
            if (InventoryManager.Instance_Player._AddItem(_item))
            {
                gameObject.SetActive(false);
            }
            else
            {
                // do nothing as the inventory is out of space
            }
        }
    }
}