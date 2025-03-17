using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenShopController : MonoBehaviour
{
    [Tooltip("The Parent of ( shop, sell, controller")]
    [SerializeField] GameObject _allCanvases;
    [SerializeField] GameObject _shopCanvas;
    [SerializeField] GameObject _sellCanvas;
    [SerializeField] Button _shopTabButton;
    [SerializeField] Button _SellTabButton;
    [SerializeField] Button _exitButton;

    bool _isOpen;

    private void Start()
    {
        _shopTabButton.onClick.AddListener(() => _ChangeTab(_AllTabTypes.shop));
        _SellTabButton.onClick.AddListener(() => _ChangeTab(_AllTabTypes.sell));
        _exitButton.onClick.AddListener(() => { _allCanvases.SetActive(false); });
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(A.Tags.playerWeapon) && !_isOpen)
        {
            _CanvasesActivation(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(A.Tags.player) && _isOpen)
        {
            _CanvasesActivation(false);
        }
    }
    private void _CanvasesActivation(bool iActivation)
    {
        if (iActivation && !_isOpen)
        {
            _isOpen = true;
            _shopTabButton.interactable = false;
            _SellTabButton.interactable = true;
            UiManager.Instance._ActivateInventory(true, false);
            _shopCanvas.SetActive(true);
            _allCanvases.SetActive(true);
            _sellCanvas.SetActive(false);
        }
        else if (!iActivation && _isOpen)
        {
            _isOpen = false;
            UiManager.Instance._ActivateInventory(false);
            _allCanvases.SetActive(false);
        }
    }
    private void _ChangeTab(_AllTabTypes iType)
    {
        _shopTabButton.interactable = iType != _AllTabTypes.shop;
        _SellTabButton.interactable = iType != _AllTabTypes.sell;

        _shopCanvas.SetActive(iType == _AllTabTypes.shop);
        _sellCanvas.SetActive(iType == _AllTabTypes.sell);
    }
    public enum _AllTabTypes
    {
        shop, sell
    }
}
