using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    ItemData _itemData;

    public void _StartTheBullet(ItemData iItemData)
    {
        _itemData = iItemData;
        Invoke(nameof(_DespawnBullet), _itemData._towerInfo._bulletInfo._lifeTime);
    }
    private void Update()
    {
        if (_itemData == null) return;

        transform.position +=
            Vector3.left * Time.deltaTime * _itemData._towerInfo._bulletInfo._bulletSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_itemData == null) return;

        if (collision.CompareTag(A.Tags.enemy))
        {
            collision.GetComponent<EnemyController>()._TakeDamage(_itemData._towerInfo._damage);
        }

        _DespawnBullet();
    }
    private void _DespawnBullet()
    {
        PoolManager._despawn(gameObject);
    }
}
