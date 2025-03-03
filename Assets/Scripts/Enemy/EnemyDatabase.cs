using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "TahaScripts/CreateNewEnemy")]
public class EnemyDatabase : ScriptableObject
{
    public _EnemyStatsStruct _stats;
    public _EnemyViewStruct _view;
    public _EnemyDamageStruct _damage;

    #region Types
    [System.Serializable]
    public struct _EnemyStatsStruct
    {
        public int _hp;
        public int _armor;
        public float _speed;
    }
    [System.Serializable]
    public struct _EnemyViewStruct
    {
        public float _attackRange;
        public float _playerLockRange;
    }
    [System.Serializable]
    public struct _EnemyDamageStruct
    {
        public float _attackSpeed;
        public int _attackDamage;

        public float _collisionAttackSpeed;
        public int _collisionDamage;
    }
    #endregion
}
