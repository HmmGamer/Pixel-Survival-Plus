using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyDatabase _enemyData;
    int _currentHp;
    bool _hasCollisionWithPlayer;

    private void OnEnable()
    {
        _ResetEnemy();
    }
    private void _ResetEnemy()
    {
        _currentHp = _enemyData._stats._hp;
        _hasCollisionWithPlayer = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(A.Tags.player))
        {
            _hasCollisionWithPlayer = true;
            StartCoroutine(_AttackOnCollision());
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(A.Tags.player))
        {
            _hasCollisionWithPlayer = false;
        }
    }
    private void _AttackPlayer(int iDamage)
    {
        PlayerController.instance._TakeDamage(iDamage);
    }
    IEnumerator _AttackOnCollision()
    {
        _AttackPlayer(_enemyData._damage._collisionDamage);
        yield return new WaitForSeconds(_enemyData._damage._collisionAttackSpeed);
        if (_hasCollisionWithPlayer)
        {
            StartCoroutine(_AttackOnCollision());
        }
    }
}
