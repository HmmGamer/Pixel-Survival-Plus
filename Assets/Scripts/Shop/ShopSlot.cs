using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InventorySlot))]
public class ShopSlot : MonoBehaviour
{
    [SerializeField] ItemData _itemForSell;
    [SerializeField] ShopController _shopController;
    [SerializeField] Text _priceText;

    InventorySlot _invSlot;

    private void Awake()
    {
        _invSlot = GetComponent<InventorySlot>();
        _invSlot._data = new _InvData(_itemForSell);
        _invSlot._UpdateUi();
        _priceText.text = _itemForSell._shopInfo._buyPrice.ToString();
    }
    private void OnEnable()
    {
        _invSlot._onChangeSelection += _SetShopDataOnSelection;
    }
    private void OnDisable()
    {
        _invSlot._onChangeSelection -= _SetShopDataOnSelection;
    }
    private void _SetShopDataOnSelection(_InvData iData)
    {
        _shopController._SetDataOnSelect(iData, _invSlot);
    }
}
