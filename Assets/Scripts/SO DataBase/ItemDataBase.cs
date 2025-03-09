using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "TahaScripts/CreateNewItem")]
public class ItemData : ScriptableObject
{
    [Header("General Info")]
    public _InventoryInfoClass _invInfo;

    [Header("Only Select One")]
    public bool _canBeWorn;
    public bool _canBePlaced;

    //[Header("Fill if it can be placed")]
    [ConditionField(nameof(_canBePlaced))] public _TowerInfoClass _towerInfo;

    //[Header("Fill if is wearable")]
    [ConditionField(nameof(_canBeWorn))] public _DefenseInfoClass _defenseInfo;

#if UNITY_EDITOR
    private void Awake()
    {
        if (_canBeWorn && _canBePlaced)
        {
            Debug.LogError(name + " _canBePlaced & _canBeWorn cant be active at the same time");
        }
    }
#endif
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
    public _WearableTypes _wearableType;
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