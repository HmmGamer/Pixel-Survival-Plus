using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackController : MonoBehaviour
{
    [SerializeField] ItemData _itemData;
    [SerializeField] Transform _firePos;
    [SerializeField] bool _showGizmos;

    bool _canAttack = true;

    private void Update()
    {
        if (!_canAttack) return;

        _RayCast();
    }
    private void _RayCast()
    {
        RaycastHit2D enemy = Physics2D.Raycast(_firePos.position,
            Vector2.left, _itemData._towerInfo._attackRange, A.LayerMasks.enemy);

        if (enemy)
            _Attack();
    }
    private void _Attack()
    {
        if (!_canAttack) return;
        StartCoroutine(_AttackCoolDown());

        GameObject bullet = PoolManager._instance._Instantiate (_PoolType.bullet,
            _itemData._towerInfo._bulletPrefab, _firePos.position, Quaternion.identity);

        bullet.GetComponent<BulletController>()._StartTheBullet(_itemData);
    }
    IEnumerator _AttackCoolDown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_itemData._towerInfo._attackSpeed);
        _canAttack = true;
    }
    private void OnDrawGizmos()
    {
        if (_showGizmos)
        {
            Vector2 rayDirection = Vector2.left;
            Vector2 rayEndPoint = (Vector2)_firePos.position + rayDirection *
                _itemData._towerInfo._attackRange;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(_firePos.position, rayEndPoint);
        }
    }
}
