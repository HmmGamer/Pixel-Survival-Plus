using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyDatabase _enemyData;

    [SerializeField, ReadOnly] int _currentHp;
    bool _hasCollisionWithPlayer;
    bool _hasCollisionWithHome;
    [HideInInspector] public int _canMove = 0;
    TowerWallController _currentTowerWall;
    Rigidbody2D _rb;

    #region starter
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        _ResetEnemy();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void _ResetEnemy()
    {
        _currentHp = _enemyData._stats._hp;
        _hasCollisionWithPlayer = false;
        _canMove = 0;
        _currentTowerWall = null;
        _hasCollisionWithHome = false;
    }
    #endregion
    #region collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(A.Tags.home) && !_hasCollisionWithHome)
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
        if (collision.gameObject.CompareTag(A.Tags.tower))
        {
            _currentTowerWall = collision.gameObject.GetComponent<TowerWallController>();
            StartCoroutine(_AttackTowerOnCollision());
            _canMove--;
        }
        else if (collision.gameObject.CompareTag(A.Tags.player))
        {
            _hasCollisionWithPlayer = true;
            StartCoroutine(_AttackPlayerOnCollision());
            _canMove--;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(A.Tags.tower))
        {
            _currentTowerWall = null;
            _canMove++;
        }
        else if (collision.gameObject.CompareTag(A.Tags.player))
        {
            _hasCollisionWithPlayer = false;
            _canMove++;
        }
    }
    private void _StopMovement()
    {
        _rb.velocity = new Vector2(0, _rb.velocity.y);
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
    IEnumerator _AttackTowerOnCollision()
    {
        _currentTowerWall._TakeDamage(_enemyData._damage._collisionDamage);

        yield return new WaitForSeconds(_enemyData._damage._collisionAttackSpeed);
        if (_currentTowerWall)
        {
            StartCoroutine(_AttackTowerOnCollision());
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
            Pool._GetInstance(_PoolType.enemy)._Despawn(gameObject);
        }
    }
    public void _DropLoot()
    {
        foreach (LootDatabase _lootData in _enemyData._loots)
        {
            GameObject _loot = _lootData._GetLoot();

            if (_loot != null)
            {
                GameObject lootInstance = PoolManager._Instantiate(_loot, transform.position, Quaternion.identity);

                Rigidbody2D lootRigidbody = lootInstance.GetComponent<Rigidbody2D>();

                if (lootRigidbody != null)
                {
                    float randomDirection = Random.Range(0f, 1f) > 0.5f ? 1f : -1f; // Random direction: 1 for right, -1 for left
                    Vector2 force = new Vector2(randomDirection * Random.Range(2.5f, 4.5f), Random.Range(4f, 6f)); // Random horizontal and vertical force
                    lootRigidbody.AddForce(force, ForceMode2D.Impulse); // Apply the force instantly
                }
            }
        }
    }

    #endregion
}
