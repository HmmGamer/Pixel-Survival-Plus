using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] ItemData _item;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(A.Tags.player))
        {
            if (InventoryManager.Instance_Player._AddItem(_item))
            {
                PoolManager._despawn(gameObject);
            }
            else
            {
                // do nothing as the inventory is out of space
            }
        }
    }

}