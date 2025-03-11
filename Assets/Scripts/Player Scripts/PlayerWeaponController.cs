using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] Animator _anim;

    int _weaponDamage = 0;
    float _attackSpeed = 0;
    bool _canAttack = true;

    private void Start()
    {
        _ChangeStats();
    }
    private void OnEnable()
    {
        //EquipmentManager._onStatsChange += _ChangeStats;
    }
    private void OnDisable()
    {
        //EquipmentManager._onStatsChange += _ChangeStats;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButton(0))
        {
            _Attack();
        }
    }
    private void _ChangeStats()
    {
        //GetComponent<SpriteRenderer>().sprite =
        //    EquipmentManager.instance._currentStats._weaponSprite;

        //_weaponDamage = EquipmentManager.instance._currentStats._extraDamage;
        //_weaponDamage += PlayerController.instance._defaultDamage;
        //_attackSpeed = EquipmentManager.instance._currentStats._attackSpeed;
    }
    private void _Attack()
    {
        if (_canAttack && _attackSpeed > 0)
        {
            StartCoroutine(_WeaponCoolDown());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(A.Tags.enemy))
        {
            collision.GetComponent<EnemyController>()._TakeDamage(_weaponDamage);
        }
    }
    private IEnumerator _WeaponCoolDown()
    {
        _canAttack = false;
        GetComponent<Collider2D>().enabled = true;

        _anim.speed = 1 / _attackSpeed;
        _anim.SetTrigger(A.Anim.PlayerAttack);

        yield return new WaitForSeconds(_attackSpeed);
        GetComponent<Collider2D>().enabled = false;
        _canAttack = true;
    }
}