using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "TahaScripts/CreateNewItem")]
public class ItemData : ScriptableObject
{
    [Header("General Info")]
    public _InventoryInfoClass _invInfo;
    public _ShopInfoClass _shopInfo;
    public _ItemDataType _type;

    [Header("Advanced Info")]
    [ConditionalEnum(nameof(_type), (int)_ItemDataType.building)]
    public _TowerInfoClass _towerInfo;
    [ConditionalEnum(nameof(_type), (int)_ItemDataType.equipment)]
    public _DefenseInfoClass _defenseInfo;
    [ConditionalEnum(nameof(_type), (int)_ItemDataType.potion)]
    public _PotionInfoClass _potionInfo;

    private void Awake()
    {
        if (_type != _ItemDataType.equipment)
            _defenseInfo._wearableType = _AllWearableTypes.none;
    }
    public _Stats _GetStats()
    {
        if (_type == _ItemDataType.equipment)
        {
            return new _Stats(_defenseInfo._damage, _defenseInfo._extraHp
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
#region types
[System.Serializable]
public class _InventoryInfoClass
{
    public string _name;
    public string _description;
    public Sprite _inventorySprite;
    public int _maxStack = 1;
}
[System.Serializable]
public class _Stats
{
    public int _damage;
    public int _hp;
    public int _armor;
    public float _attackSpeed;

    public _Stats() { }
    public _Stats(int iDamage, int iHp, int iArmor, float iAttkSpeed)
    {
        _damage = iDamage;
        _hp = iHp;
        _armor = iArmor;
        _attackSpeed = iAttkSpeed;
    }
    public static _Stats operator +(_Stats a, _Stats b)
    {
        return new _Stats(a._damage + b._damage, a._hp + b._hp,
            a._armor + b._armor, a._attackSpeed + b._attackSpeed);
    }
    public void _ResetStats()
    {
        _damage = 0;
        _hp = 0;
        _armor = 0;
        _attackSpeed = 0;
    }
}
[System.Serializable]
public class _ShopInfoClass
{
    public int _buyPrice;
    public int _sellPrice;
}
[System.Serializable]
public class _TowerInfoClass
{
    public GameObject _towerPrefab;
    public int _hp;
    public int _armor;
    public int _damage;
    public float _attackSpeed;
    public int _attackRange;

    public _AllTowerTypes _towerType;
    [ConditionalEnum(nameof(_towerType), (int)_AllTowerTypes.bullets)]
    public GameObject _bulletPrefab;
}
[System.Serializable]
public class _PotionInfoClass
{
    public _AllPotionTypes _type;
    public int _hpRestore;
}
[System.Serializable]
public class _DefenseInfoClass
{
    public _AllWearableTypes _wearableType;
    public Sprite _equipmentSprite;
    public int _extraHp;
    public int _extraArmor;
    [ConditionalEnum(nameof(_wearableType), (int)_AllWearableTypes.weapon)] public int _damage;
    [ConditionalEnum(nameof(_wearableType), (int)_AllWearableTypes.weapon)] public float _attackSpeed;

    public static explicit operator _Stats(_DefenseInfoClass _defenseInfo)
    {
        return new _Stats(_defenseInfo._damage, _defenseInfo._extraHp,
            _defenseInfo._extraArmor, _defenseInfo._attackSpeed);
    }
}
#endregion
#region enums
[System.Serializable]
public enum _ItemDataType
{
    equipment, building, potion, none
}
[System.Serializable]
public enum _AllPotionTypes
{
    heal
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
#endregion