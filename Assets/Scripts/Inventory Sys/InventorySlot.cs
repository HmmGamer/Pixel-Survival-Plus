using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventorySlot : MonoBehaviour
{
    // its static and it tells all of the slots that they can't be selected
    public static event UnityAction _onNewSelection;
    // this is like the _onNewSelection but its not static
    public event UnityAction<_InvData> _onChangeSelection;
    public event UnityAction<_InvData> _onSlotChange;

    //[Header("General Settings")]
    //[SerializeField] bool _isSelectable = true;

    [Header("Attachments")]
    [SerializeField] Image _iconImage;
    [SerializeField] Text _quantityText;

    [HideInInspector] public _AllWearableTypes _slotType = _AllWearableTypes.none;
    [HideInInspector] public _InvData _data = null;
    bool _isSelected;
    Image _selectionBorder;
    
    #region starter
    private void Start()
    {
        _InitButtons();
        _selectionBorder = GetComponent<Image>();
    }
    private void OnEnable()
    {
        _onNewSelection += _ResetSelection;
    }
    private void OnDisable()
    {
        _onNewSelection -= _ResetSelection;
    }
    private void _InitButtons()
    {
        GetComponent<Button>().onClick.AddListener(_B_ChangeSelection);
    }
    #endregion
    #region visual
    public void _UpdateUi()
    {
        _UpdateIcon();
        _UpdateQuantityText();
    }
    private void _UpdateIcon()
    {
        if (_data == null)
            _iconImage.color = Color.clear;
        else
        {
            _iconImage.sprite = _data._itemData._invInfo._inventorySprite;
            _iconImage.color = Color.white;
        }
    }
    private void _UpdateQuantityText()
    {
        if (_quantityText == null) return;

        if (_data == null)
            _quantityText.text = string.Empty;
        else if (_data._quantity == 1)
            _quantityText.text = string.Empty;
        else
            _quantityText.text = _data._quantity.ToString();
    }
    #endregion

    public bool _ChangeData(_InvData iData)
    {
        if (iData != null && _slotType != _AllWearableTypes.none)
            if (!_CanSlotStoreItem(iData._itemData)) return false;

        _data = iData;
        _UpdateUi();
        _onSlotChange?.Invoke(iData);
        _onNewSelection?.Invoke();
        return true;
    }
    public void _UseItem(int iCount = 1)
    {
        _data._quantity -= iCount;

        if (_data._quantity <= 0)
        {
            // we deSelect and remove the item form the inventory
            _B_ChangeSelection();
            _ChangeData(null);
        }
            
        else // we decreased it first and now we update the Ui 
            _UpdateUi();
    }
    public bool _CanSlotStoreItem(ItemData iNewData)
    {
        if (iNewData._defenseInfo._wearableType == _slotType)
            return true;
        return false;
    }
    private void _B_ChangeSelection()
    {
        if (_data == null) return;

        if (!_isSelected)
        {
            _onNewSelection?.Invoke();
            _onChangeSelection?.Invoke(_data);
            _isSelected = true;
            _selectionBorder.color = Color.yellow;
            InventoryManager.Instance._ShowSlotInfo(_data._itemData);
            ButtonManager.instance._ChangeActivation(true, this, _data._itemData);
        }
        else
        {
            _onChangeSelection?.Invoke(null);
            InventoryManager.Instance._ShowSlotInfo(null);
            ButtonManager.instance._ChangeActivation(false);
            _ResetSelection();
        }
    }
    private void _ResetSelection()
    {
        _isSelected = false;
        _selectionBorder.color = InventoryManager.Instance._defaultSlotColor;
    }
}
public class _InvData
{
    public ItemData _itemData;
    public int _quantity;

    public _InvData(ItemData iData, int iQuantity = 1)
    {
        _itemData = iData;
        _quantity = iQuantity;
    }
    public _InvData() { }
}