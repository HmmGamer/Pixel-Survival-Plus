using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField] Button _buyButton;

    ItemData _currentData;
    InventorySlot _slot;

    private void Start()
    {
        _InitButtons();
    }
    private void _InitButtons()
    {
        _buyButton.onClick.AddListener(_BuyItem);
    }
    private void _BuyItem()
    {
        if (_currentData == null) return;

        if (MoneyManager.instance._CanPurchaseItem(_currentData))
        {
            if (InventoryManager.Instance._AddNewItem(_currentData))
            {
                MoneyManager.instance._PurchaseItem(_currentData);
            }
        }
    }
    public void _SetDataOnSelect(_InvData iData, InventorySlot iSlot)
    {
        if (iData == null)
            _currentData = null;
        else
            _currentData = iData._itemData;

        _slot = iSlot;
        _UpdateUi();
    }
    private void _UpdateUi()
    {
        _buyButton.gameObject.SetActive(_currentData);
    }
}