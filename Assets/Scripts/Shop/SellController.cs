using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellController : MonoBehaviour
{
    [SerializeField] Button _confirmSellButton;
    [SerializeField] Text _totalPriceText;
    [SerializeField] InventorySlot _sellSlot;

    private int _totalPrice;

    #region Starter
    private void Start()
    {
        _InitButtons();
        _confirmSellButton.gameObject.SetActive(false);
    }
    private void _InitButtons()
    {
        _confirmSellButton.onClick.AddListener(() => _B_SellItem());
    }
    private void OnEnable()
    {
        _sellSlot._onSlotChange += _UpdateShopInfo;
    }
    private void OnDisable()
    {
        _sellSlot._onSlotChange -= _UpdateShopInfo;
    }
    #endregion

    private void _B_SellItem()
    {
        MoneyManager.instance._AddMoney(_totalPrice);

        // reminder : this method calls the _UpdateShopInfo automatically
        _sellSlot._ChangeData(null);
    }
    private void _UpdateShopInfo(_InvData iData)
    {
        if (iData == null)
        {
            _totalPrice = 0;
            _confirmSellButton.gameObject.SetActive(false);
        }
        else
        {
            _totalPrice = iData._itemData._shopInfo._sellPrice * iData._quantity;
            _confirmSellButton.gameObject.SetActive(true);
        }
        _UpdatePriceText();
    }
    private void _UpdatePriceText()
    {
        _totalPriceText.text = _totalPrice.ToString();
    }
}
