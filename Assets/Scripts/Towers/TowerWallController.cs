using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerWallController : MonoBehaviour
{
    [SerializeField] ItemData _itemData;

    int _currentHp;

    private void OnEnable()
    {
        _currentHp = _itemData._towerInfo._hp;
    }
    public void _TakeDamage(int iDamage)
    {
        iDamage = GeneralTools.HpTools._CalculateDamage(iDamage, _itemData._towerInfo._armor);
        _currentHp -= iDamage;

        if (iDamage >= _currentHp)
        {
            PoolManager._instance._Despawn(gameObject);
        }
    }
}
