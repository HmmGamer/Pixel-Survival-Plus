using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] EnemyDatabase _enemyData;

    Vector2 _basicDestinationPos;
    Vector2 _tempDestinationPos;

    private void Start()
    {
        if (HomeController.instance != null)
            _basicDestinationPos = HomeController.instance.transform.position;
        else
            _basicDestinationPos = PlayerController.instance.transform.position;

        if (!_enemyData._movement._canFly)
            _basicDestinationPos.y = transform.position.y;
    }
    private void Update()
    {
        _CheckForPlayer();
        _Move();
    }
    private void _CheckForPlayer()
    {
        Collider2D iPlayer = Physics2D.OverlapCircle(transform.position
            , _enemyData._view._playerLockRange, A.LayerMasks.player);
        if (iPlayer != null)
        {
            _tempDestinationPos = iPlayer.transform.position;

            if (!_enemyData._movement._canFly)
            {
                _tempDestinationPos.y = transform.position.y;
            }
        }
        else
            _tempDestinationPos = Vector2.zero;
    }
    private void _Move()
    {
        Vector2 targetPosition =
            _tempDestinationPos != Vector2.zero ? _tempDestinationPos : _basicDestinationPos;

        transform.position = Vector2.MoveTowards(transform.position
            , targetPosition, _enemyData._movement._speed * Time.deltaTime);
    }
}
