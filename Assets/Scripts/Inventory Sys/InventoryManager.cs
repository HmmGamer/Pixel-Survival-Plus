using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // this is so we can detect the player's inventory easily
    public static InventoryManager Instance_PlayerInventory;

    public int _maxSlots;
    public List<InventorySlotDataClass> _inventoryDataSlots = new List<InventorySlotDataClass>();

    private void Awake()
    {
        if (Instance_PlayerInventory == null)
            Instance_PlayerInventory = this;
        else
        {
            // we do nothing here so we can have multiple inventories later Like chests
        }
    }
    public bool _AddItem(ItemController item, int quantity = 1)
    {
        if (item._invInfo._maxStack > 1)
        {
            InventorySlotDataClass existingSlot = _inventoryDataSlots.Find(slot => slot.Item == item && slot.Quantity < item._invInfo._maxStack);
            if (existingSlot != null)
            {
                existingSlot.Quantity += quantity;
                return true;
            }
        }

        if (_HasEmptySpace())
        {
            _inventoryDataSlots.Add(new InventorySlotDataClass(item, quantity));
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool _HasEmptySpace()
    {
        return _inventoryDataSlots.Count < _maxSlots;
    }
    public void _RemoveItem(ItemController item, int quantity = 1)
    {
        InventorySlotDataClass slot = _inventoryDataSlots.Find(s => s.Item == item);
        if (slot != null)
        {
            slot.Quantity -= quantity;
            if (slot.Quantity <= 0)
                _inventoryDataSlots.Remove(slot);
        }
    }
    public void _MoveItem(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || fromIndex >= _inventoryDataSlots.Count || toIndex < 0 || toIndex >= _inventoryDataSlots.Count)
            return;

        InventorySlotDataClass temp = _inventoryDataSlots[fromIndex];
        _inventoryDataSlots[fromIndex] = _inventoryDataSlots[toIndex];
        _inventoryDataSlots[toIndex] = temp;
    }
}

[System.Serializable]
public class InventorySlotDataClass
{
    public ItemController Item;
    public int Quantity;

    public InventorySlotDataClass(ItemController item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }
}