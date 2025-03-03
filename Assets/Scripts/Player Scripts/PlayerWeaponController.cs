using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerWeaponController : MonoBehaviour
{
    int _weaponDamage = 0;
    float _attackSpeed = 0;
    bool _canAttack = true;

    private void OnEnable()
    {
        EquipmentManager._onStatsChange += _ChangeStats;
    }
    private void OnDisable()
    {
        EquipmentManager._onStatsChange += _ChangeStats;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
        {
            _Attack();
        }
    }
    private void _ChangeStats()
    {
        _weaponDamage = EquipmentManager.instance._currentStats._extraDamage;
        _weaponDamage += PlayerController.instance._defaultDamage;
        _attackSpeed = EquipmentManager.instance._currentStats._attackSpeed;
    }
    private void _Attack()
    {
        if (_canAttack)
        {
            // attack
            StartCoroutine(_WeaponCoolDown());
        }
    }
    private IEnumerator _WeaponCoolDown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackSpeed);
        _canAttack = true;
    }
}
