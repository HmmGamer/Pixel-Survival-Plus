using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlots : MonoBehaviour
{
    [SerializeField] Image _iconImage;
    [SerializeField] Image _frameImage;
    [SerializeField] Button _selectionButton;

    ItemData _currentItem;
    bool _isSelected;
    Color _defaultColor;
    Sprite _defaultSprite;

    private void Start()
    {
        _defaultSprite = _iconImage.sprite;
        _defaultColor = _frameImage.color;
        _selectionButton.onClick.AddListener(_SelectButton);
    }
    private void OnEnable()
    {
        InventorySlotUi._onSelection += _ResetSelection;
    }
    private void OnDisable()
    {
        InventorySlotUi._onSelection -= _ResetSelection;
    }
    private void _SelectButton()
    {
        if (_isSelected)
        {
            InventoryUi.Instance_Player._UpdateSelectedItemInfo(null, null);
            _ResetSelection();
        }
        else
        {
            InventorySlotUi._ResetSelection();
            _isSelected = true;
            _frameImage.color = Color.yellow;
            InventoryUi.Instance_Player._UpdateSelectedItemInfo(_currentItem, _RemoveEquipment);
        }
    }
    private void _RemoveEquipment()
    {
        InventoryManager.Instance_Player._AddItem(_currentItem);
        _currentItem = null;
        _ResetSelection();
        _UpdateUiSprite();
        EquipmentManager.instance._CalculateAllStats();
    }
    private void _ResetSelection()
    {
        _isSelected = false;
        _frameImage.color = _defaultColor;
    }
    public void _SetCurrentItem(ItemData iNewItem)
    {
        if (_currentItem != null) // if he already has an equipment
        {
            InventoryManager.Instance_Player._AddItem(_currentItem);
        }
        _currentItem = iNewItem;
        _UpdateUiSprite();
    }
    private void _UpdateUiSprite()
    {
        if (_currentItem == null)
        {
            _iconImage.sprite = _defaultSprite;
        }
        else
        {
            _iconImage.sprite = _currentItem._invInfo._inventorySprite;
        }
    }
    public _DefenseInfoClass _GetStats()
    {
        if (_currentItem)
            return _currentItem._defenseInfo;
        return null;
    }
}
