using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "TahaScripts/CreateNewItem")]
public class ItemData : ScriptableObject
{
    [Header("General Info")]
    public _InventoryInfoClass _invInfo;

    [Header("Fill if it can be placed")]
    public bool _canBePlaced;
    public _AttackInfoClass _attackInfo;

    [Header("Fill if is wearable")]
    public bool _canBeWorn;
    public _DefenseInfoClass _defenseInfo;

#if UNITY_EDITOR
    private void Awake()
    {
        if (_canBeWorn&& _canBePlaced)
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
    public _WearableTypes _wearableType;
    public int _extraDamage;
    public int _extraHp;
    public int _extraArmor;
    public float _attackSpeed;
}