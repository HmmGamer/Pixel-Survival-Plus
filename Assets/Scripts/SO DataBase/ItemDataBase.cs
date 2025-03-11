using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "TahaScripts/CreateNewItem")]
public class ItemData : ScriptableObject
{
    [Header("General Info")]
    public _InventoryInfoClass _invInfo;
    public _ItemDataType _type;

    [Header("Advanced Info")]

    [ConditionalEnum(nameof(_type), (int)_ItemDataType.building)]
    public _TowerInfoClass _towerInfo;

    [ConditionalEnum(nameof(_type), (int)_ItemDataType.equipment)]
    public _DefenseInfoClass _defenseInfo;

    private void Awake()
    {
        if (_type != _ItemDataType.equipment)
            _defenseInfo._wearableType = _AllWearableTypes.none;
    }
    public _Stats _GetStats()
    {
        if (_type == _ItemDataType.equipment)
        {
            return new _Stats(_defenseInfo._extraDamage, _defenseInfo._extraHp
                , _defenseInfo._extraArmor, _defenseInfo._attackSpeed);
        }
        else if (_type == _ItemDataType.building)
        {
            return new _Stats(_towerInfo._damage, _towerInfo._hp
                , _towerInfo._armor, _towerInfo._attackSpeed);
        }
        else
        {
            return null;
        }
    }
}
[System.Serializable]
public enum _ItemDataType
{
    equipment, building, none
}

[System.Serializable]
public class _InventoryInfoClass
{
    public string _name;
    public string _description;
    public Sprite _inventorySprite;

    public int _buyPrice;
    public int _sellPrice;
    public int _maxStack = 1;
}
[System.Serializable]
public class _Stats
{
    public int _damage;
    public int _hp;
    public int _armor;
    public float _attackSpeed;

    public _Stats(int iDamage, int iHp, int iArmor, float iAttkSpeed)
    {
        _damage = iDamage;
        _hp = iHp;
        _armor = iArmor;
        _attackSpeed = iAttkSpeed;
    }
}
[System.Serializable]
public class _TowerInfoClass
{
    public GameObject _towerPrefab;
    public int _damage;
    public int _hp;
    public int _armor;
    public float _attackSpeed;
    public int _attackRange;

    public _AllTowerTypes _towerType;
    [ConditionalEnum(nameof(_towerType), (int)_AllTowerTypes.bullets)]
    public _BulletInfoClass _bulletInfo;
}
[System.Serializable]
public class _BulletInfoClass
{
    public GameObject _bulletPrefab;
    public float _bulletSpeed;
    public float _lifeTime;
}
[System.Serializable]
public class _DefenseInfoClass
{
    public _AllWearableTypes _wearableType;
    public Sprite _weaponSprite;
    public int _extraDamage;
    public int _extraHp;
    public int _extraArmor;
    public float _attackSpeed;
}
[System.Serializable]
public enum _AllTowerTypes
{
    bullets, spin
}
[System.Serializable]
public enum _AllWearableTypes
{
    none, head, body, legs, weapon, shield
}