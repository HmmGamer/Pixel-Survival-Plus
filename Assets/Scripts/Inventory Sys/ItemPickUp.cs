using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    /* use case:
     * this gameObject get's two colliders :
     * one small collider for handling floor and not going down
     * and one bigger one with isTrigger active
     * it will insure the player wont lose speed after collision with the loot 
    */
    [SerializeField] ItemData _item;

    Rigidbody2D _rb;

    private void OnEnable()
    {
        // disable rigidBody after some time
        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();

        _ResetOnPool();
        Invoke(nameof(_DisablePhysics), 3);
    }
    private void _DisablePhysics()
    {
        _rb.isKinematic = true;
    }
    private void _ResetOnPool()
    {
        _rb.isKinematic = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(A.Tags.player))
        {
            if (InventoryManager.Instance._AddNewItem(_item))
            {
                PoolManager._GetInstance(_PoolType.item)._Despawn(gameObject);
            }
        }
    }
}
