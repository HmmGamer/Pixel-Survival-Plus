using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float _lifeTime;
    [SerializeField] float _speed;
    [SerializeField] Collider2D _collider;
    ItemData _itemData;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(_DestroyCooldown(_lifeTime));
        _collider.enabled = true;
    }
    private void OnDisable()
    {
        _collider.enabled = false;
    }
    public void _StartTheBullet(ItemData iItemData)
    {
        _itemData = iItemData;
    }
    private void Update()
    {
        if (_itemData == null) return;

        transform.position += Vector3.left * Time.deltaTime * _speed;
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
    IEnumerator _DestroyCooldown(float iLifeTime)
    {
        yield return new WaitForSeconds(iLifeTime);
        _DespawnBullet();
    }
    private void _DespawnBullet()
    {
        PoolManager._GetInstance(_PoolType.bullet)._Despawn(gameObject);
    }
}
