using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventorySlot))]
public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] _AllWearableTypes _slotType;

    InventorySlot _slot;
    private void Start()
    {
        _slot = GetComponent<InventorySlot>();
        _slot._slotType = _slotType;
    }
    public void _UpdateData()
    {

    }
}
