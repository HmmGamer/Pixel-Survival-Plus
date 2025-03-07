using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    [SerializeField] int _coinCount;

    private void Start()
    {
        Invoke(nameof(_DisablePhysics), 2);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(A.Tags.player))
        {
            MoneyManager.instance._AddMoney(_coinCount);
            PoolManager._despawn(gameObject);
        }
    }
    private void _DisablePhysics()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
    }
}
