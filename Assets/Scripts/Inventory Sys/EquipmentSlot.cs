using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventorySlot))]
public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] _AllWearableTypes _slotType;
    [SerializeField] SpriteRenderer _equipmentImage;

    InventorySlot _slot;
    private void Awake()
    {
        _slot = GetComponent<InventorySlot>();
        _slot._slotType = _slotType;
    }
    private void OnEnable()
    {
        EquipmentManager._onUpdateStats += _UpdateEquipmentStats;
        _slot._onSlotChange += _StatsAreChanged;
    }
    private void OnDisable()
    {
        EquipmentManager._onUpdateStats -= _UpdateEquipmentStats;
        _slot._onSlotChange -= _StatsAreChanged;
    }
    public void _StatsAreChanged(_InvData iData)
    {
        _ChangeEquipmentSprite();
        EquipmentManager.Instance._CalculateStats();
    }
    public void _UpdateEquipmentStats()
    {
        if (_slot._data != null)
        {
            EquipmentManager.Instance._AddNewStat((_Stats)_slot._data._itemData._defenseInfo);
        }

    }
    private void _ChangeEquipmentSprite()
    {
        if (_equipmentImage == null) return;

        if (_slot._data == null)
            _equipmentImage.color = Color.clear;
        else
        {
            _equipmentImage.color = Color.white;
            _equipmentImage.sprite = _slot._data._itemData._defenseInfo._equipmentSprite;
        }    
    }
}
public class __FutureUpdates
{
    /* remember to change the logic for calculating stats by removing the loop and only
     * changing the data that was changed for more performance
     */
}