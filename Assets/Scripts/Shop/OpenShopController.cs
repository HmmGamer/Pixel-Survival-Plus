using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OpenShopController : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] float _closeShopDistance;

    [Header("Attachments")]
    [Tooltip("The Parent of ( shop, sell, controller")]
    [SerializeField] GameObject _allCanvases;
    [SerializeField] GameObject _shopCanvas;
    [SerializeField] GameObject _sellCanvas;
    [SerializeField] Button _shopTabButton;
    [SerializeField] Button _SellTabButton;
    [SerializeField] Button _exitButton;

    bool _isOpen;
    Coroutine _exitHandler;

    private void Start()
    {
        _shopTabButton.onClick.AddListener(() => _ChangeTab(_AllTabTypes.shop));
        _SellTabButton.onClick.AddListener(() => _ChangeTab(_AllTabTypes.sell));
        _exitButton.onClick.AddListener(() => _CanvasesActivation(false));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(A.Tags.playerWeapon) && !_isOpen)
        {
            _CanvasesActivation(true);
        }
    }
    private void _CanvasesActivation(bool iActivation)
    {
        if (iActivation && !_isOpen) //enter
        {
            _isOpen = true;
            _shopTabButton.interactable = false;
            _SellTabButton.interactable = true;
            UiManager.Instance._ActivateInventory(true, false);
            _shopCanvas.SetActive(true);
            _allCanvases.SetActive(true);
            _sellCanvas.SetActive(false);

            _exitHandler = StartCoroutine(_CheckPlayerDistance());
        }
        else if (!iActivation && _isOpen) //exit
        {
            _isOpen = false;
            UiManager.Instance._ActivateInventory(false);
            _allCanvases.SetActive(false);
            _exitHandler = null;
        }
    }
    private void _ChangeTab(_AllTabTypes iType)
    {
        _shopTabButton.interactable = iType != _AllTabTypes.shop;
        _SellTabButton.interactable = iType != _AllTabTypes.sell;

        _shopCanvas.SetActive(iType == _AllTabTypes.shop);
        _sellCanvas.SetActive(iType == _AllTabTypes.sell);
    }

    /// <summary>
    /// this custom update makes sure the player can't move too far from the shop
    /// </summary>
    private IEnumerator _CheckPlayerDistance()
    {
        while (true)
        {
            if (Vector2.Distance(PlayerController.instance.transform.position, transform.position) > _closeShopDistance)
            {
                _CanvasesActivation(false);
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    public enum _AllTabTypes
    {
        shop, sell
    }
}