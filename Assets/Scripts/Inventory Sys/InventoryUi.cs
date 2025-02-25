using UnityEngine;
using UnityEngine.UI;

public class InventoryUi : MonoBehaviour
{
    public InventorySlotUi[] _uiSlots;

    private void Start()
    {
        _UpdateUI();
    }
    public void _UpdateUI()
    {
        foreach (InventorySlotUi slot in _uiSlots)
        {
            slot._ClearSlot();
        }

        for (int i = 0; i < InventoryManager.Instance_PlayerInventory._inventoryDataSlots.Count; i++)
        {
            if (i < _uiSlots.Length)
            {
                InventorySlotDataClass slot = InventoryManager.Instance_PlayerInventory._inventoryDataSlots[i];
                _uiSlots[i]._UpdateSlot(slot.Item._invInfo._inventorySprite, slot.Quantity);
            }
        }
    }
}