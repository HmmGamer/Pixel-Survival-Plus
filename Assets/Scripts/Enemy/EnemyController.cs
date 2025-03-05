using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyDatabase _enemyData;
    [SerializeField] LootDatabase _lootData;

    int _currentHp;
    bool _hasCollisionWithPlayer;
    bool _hasCollisionWithHome;

    #region starter
    private void OnEnable()
    {
        _ResetEnemy();
    }
    private void _ResetEnemy()
    {
        _currentHp = _enemyData._stats._hp;
        _hasCollisionWithPlayer = false;
    }
    #endregion
    #region collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(A.Tags.home))
        {
            _hasCollisionWithHome = true;
            StartCoroutine(_AttackHomeOnCollision());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(A.Tags.home))
        {
            _hasCollisionWithHome = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(A.Tags.player))
        {
            _hasCollisionWithPlayer = true;
            StartCoroutine(_AttackPlayerOnCollision());
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(A.Tags.player))
        {
            _hasCollisionWithPlayer = false;
        }
    }
    #endregion
    #region Damage Logic
    private void _AttackPlayer(int iDamage)
    {
        PlayerController.instance._TakeDamage(iDamage);
    }
    private void _AttackHome(int iDamage)
    {
        HomeController.instance._TakeDamage(iDamage);
    }
    IEnumerator _AttackPlayerOnCollision()
    {
        _AttackPlayer(_enemyData._damage._collisionDamage);
        yield return new WaitForSeconds(_enemyData._damage._collisionAttackSpeed);
        if (_hasCollisionWithPlayer)
        {
            StartCoroutine(_AttackPlayerOnCollision());
        }
    }
    IEnumerator _AttackHomeOnCollision()
    {
        _AttackHome(_enemyData._damage._collisionDamage);
        yield return new WaitForSeconds(_enemyData._damage._collisionAttackSpeed);
        if (_hasCollisionWithHome)
        {
            StartCoroutine(_AttackHomeOnCollision());
        }
    }
    #endregion

    #region Death Logic
    public void _TakeDamage(int iDamage)
    {
        iDamage = AAA.HpTools._CalculateDamage(iDamage, _enemyData._stats._armor);
        _currentHp -= iDamage;
        if (_currentHp <= 0)
        {
            _DropLoot();
            PoolManager._despawn(gameObject);
        }
    }
    public void _DropLoot()
    {
        GameObject _loot = _lootData._GetLoot();

        if (_loot != null)
        {
            PoolManager._Instantiate(_loot, transform.position, Quaternion.identity);
        }
    }
    #endregion
}
