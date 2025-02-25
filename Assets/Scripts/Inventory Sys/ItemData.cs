using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "TahaScripts/CreateNewItem")]
public class ItemController : ScriptableObject
{
    [Header("General Info")]
    public _InventoryInfoClass _invInfo;

    [Header("Fill if it can be placed")]
    public bool _canBePlaced;
    public _AttackInfoClass _attackInfo;

    [Header("Fill if is wearable")]
    public bool _canBeWorn;
    public _DefenseInfoClass _defenseInfo;
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
public class _AttackInfoClass
{
    public GameObject _prefab;
    public int _damage;
    public int _hp;
    public int _armor;
    public float _attackSpeed;
}
[System.Serializable]
public class _DefenseInfoClass
{
    public int _extraDamage;
    public int _extraHp;
    public int _extraArmor;
}