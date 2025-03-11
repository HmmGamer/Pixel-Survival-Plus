using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventorySlot : MonoBehaviour
{
    // it's called so no there wont be another item selected as well (for visual bugs)
    public static event System.Action onNewSelection;

    [SerializeField] Image _iconImage;
    [SerializeField] Text _quantityText;

    [HideInInspector] public _AllWearableTypes _slotType = _AllWearableTypes.none;
    [HideInInspector] public _InvData _data;
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
        onNewSelection += _ResetSelection;
    }
    private void OnDisable()
    {
        onNewSelection -= _ResetSelection;
    }
    private void _InitButtons()
    {
        GetComponent<Button>().onClick.AddListener(_B_ChangeSelection);
    }
    #endregion
    #region visual
    private void _UpdateUi()
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
        return true;
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
            onNewSelection?.Invoke();
            _isSelected = true;
            _selectionBorder.color = Color.yellow;
            InventoryManager.Instance._ShowSlotInfo(_data._itemData);
        }
        else
        {
            _isSelected = false;
            InventoryManager.Instance._ShowSlotInfo(null);
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
}