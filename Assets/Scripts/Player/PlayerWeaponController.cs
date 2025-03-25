using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// reminder : you need to activate weapon collider in the animation
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
        EquipmentManager._onNewStats += _ChangeStats;
    }
    private void OnDisable()
    {
        EquipmentManager._onNewStats -= _ChangeStats;
    }
    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _Attack();
        }
    }
    private void _ChangeStats()
    {
        _weaponDamage = EquipmentManager.Instance._totalStats._damage;
        _attackSpeed = EquipmentManager.Instance._totalStats._attackSpeed;
    }
    private void _Attack()
    {
        if (!_canAttack) return;

        if (_attackSpeed > 0)
        {
            // this part is for weapon attack
            _anim.speed = 1;
            StartCoroutine(_WeaponCoolDown());
        }
        else
        {
            // this part is for basic hand attack
            _anim.speed = 2;
            StartCoroutine(_WeaponCoolDown(0.5f));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(A.Tags.enemy))
        {
            collision.GetComponent<EnemyController>()._TakeDamage(_weaponDamage);
        }
    }
    private IEnumerator _WeaponCoolDown(float iManualCd = 0)
    {
        _canAttack = false;

        _anim.SetTrigger(A.Anim.PlayerAttack);

        if (iManualCd > 0)
            yield return new WaitForSeconds(iManualCd);
        else
            yield return new WaitForSeconds(_attackSpeed);

        _canAttack = true;
    }
}