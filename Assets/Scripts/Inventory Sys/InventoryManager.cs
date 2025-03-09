using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance_Player;
    public static event UnityAction _onInventoryChange;

    [HideInInspector]
    public List<InventorySlotDataClass> _inventoryDataSlots =
        new List<InventorySlotDataClass>();

    private int _selectedSlotIndex = -1;

    private void Awake()
    {
        if (Instance_Player == null)
            Instance_Player = this;
        else
        {
            // we do nothing so it can extend it and add secondary inventories later
        }
    }
    public bool _AddItem(ItemData item, int quantity = 1)
    {
        if (item._invInfo._maxStack > 1)
        {
            InventorySlotDataClass existingSlot = _inventoryDataSlots.Find(slot => slot.Item == item && slot.Quantity < item._invInfo._maxStack);
            if (existingSlot != null)
            {
                existingSlot.Quantity += quantity;
                _onInventoryChange?.Invoke();
                return true;
            }
        }

        if (_HasEmptySpace())
        {
            _inventoryDataSlots.Add(new InventorySlotDataClass(item, quantity));
            _onInventoryChange?.Invoke();
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool _HasEmptySpace()
    {
        return _inventoryDataSlots.Count < InventoryUi.Instance_Player._uiSlots.Length;
    }
    public void _SaveSelectedItem(int slotIndex) // logic is handled in InventoryUi
    {
        if (slotIndex >= 0 && slotIndex < _inventoryDataSlots.Count)
        {
            _selectedSlotIndex = slotIndex;
            ItemData item = _inventoryDataSlots[_selectedSlotIndex].Item;
            InventoryUi.Instance_Player._UpdateSelectedItemInfo(item);
        }
        else
        {
            _selectedSlotIndex = -1;
            InventoryUi.Instance_Player._UpdateSelectedItemInfo(null);
        }
    }
    public void _UseSelectedItem()
    {
        if (_selectedSlotIndex != -1)
        {
            ItemData selectedItem = _inventoryDataSlots[_selectedSlotIndex].Item;

            if (selectedItem._canBeWorn)
            {
                _EquipItem(selectedItem);
            }
            else if (selectedItem._canBePlaced)
            {
                _PlaceItem(selectedItem);
            }
            else
            {
                _UseItem(selectedItem);
                _inventoryDataSlots[_selectedSlotIndex].Quantity--;
                if (_inventoryDataSlots[_selectedSlotIndex].Quantity <= 0)
                {
                    _inventoryDataSlots.RemoveAt(_selectedSlotIndex);
                    _selectedSlotIndex = -1;
                }
            }
        }
    }
    public void _RemoveItem(ItemData item, int quantity = 1)
    {
        InventorySlotDataClass slot = _inventoryDataSlots.Find(s => s.Item == item);
        if (slot != null)
        {
            slot.Quantity -= quantity;
            if (slot.Quantity <= 0)
                _inventoryDataSlots.Remove(slot);
            _onInventoryChange?.Invoke();
        }
    }
    private void _EquipItem(ItemData item)
    {
        EquipmentManager.instance._ChangeEquipment(item);
    }
    private void _UseItem(ItemData item)
    {
    }
    private void _PlaceItem(ItemData item)
    {
        PoolManager._Instantiate(item._towerInfo._towerPrefab
            , PlayerBuildController.instance._spawnPos.position, Quaternion.identity);

        _RemoveItem(item);
    }
}

[System.Serializable]
public class InventorySlotDataClass
{
    public ItemData Item;
    public int Quantity;

    public InventorySlotDataClass(ItemData item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }
}