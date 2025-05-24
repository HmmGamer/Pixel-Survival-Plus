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

    [Header("Attachments")]
    [SerializeField] Image _iconImage;
    [SerializeField] Text _quantityText;

    [HideInInspector] public _AllWearableTypes _slotType = _AllWearableTypes.none;
    [HideInInspector] public _InvData _data = null;
    bool _isSelected;
    Image _selectionBorder;

    string _uniqueId;

    #region starter
    private void Awake()
    {
        _LoadData(); // dont transfer this to OnEnable!
    }
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
    #region Save - Load
    public void _SaveData()
    {
        if (_data != null)
        {
            string _itemID = _data._itemData._invInfo._name;
            int _quantity = _data._quantity;
            PlayerPrefs.SetString(_uniqueId + "_ItemID", _itemID);
            PlayerPrefs.SetInt(_uniqueId + "_Quantity", _quantity);
            Debug.Log("Saved " + _data._itemData._invInfo._name);
        }
        else
        {
            PlayerPrefs.DeleteKey(_uniqueId + "_ItemID");
            PlayerPrefs.DeleteKey(_uniqueId + "_Quantity");
        }
    }

    /// <summary>
    /// read _GetUniqueId Summery for more info
    /// </summary>
    public void _LoadData(bool iIsForInventory = false)
    {
        _GetUniqueId(iIsForInventory);

        string _keyItemID = _uniqueId + "_ItemID";
        string _keyQuantity = _uniqueId + "_Quantity";
        if (PlayerPrefs.HasKey(_keyItemID))
        {
            string _itemID = PlayerPrefs.GetString(_keyItemID);
            int _quantity = PlayerPrefs.GetInt(_keyQuantity);
            ItemData _itemData = InventoryManager.Instance._GetItemDataByID(_itemID);
            if (_itemData != null)
            {
                _InvData _loadedData = new _InvData(_itemData, _quantity);
                _ChangeData(_loadedData, true);
                Debug.Log(_loadedData + " was added to inventory");
            }
        }
    }

    /// <summary>
    /// we use the _UniqueId to insure the save Key is unique in the game
    /// 
    /// as the inventory slots need to be loaded before the game Starts ( logical reasons ) and 
    /// the UniqueId Generator needs active GameObjects for generating Id in start , we do this 
    /// to avoid Cpu overheat for slots other than the Inventory as they dont need to be loaded 
    /// at the start!
    /// </summary>
    public void _GetUniqueId(bool iIsForInventory = false)
    {
        if (_uniqueId != null) return;

        if (iIsForInventory)
            _uniqueId = UniqueIdTools._MakeUniqueId(transform, true);
        else
            _uniqueId = UniqueIdTools._MakeUniqueId(transform);
    }
    #endregion
    #region functional
    /// <summary>
    /// this is the main method of this class and used in almost everything
    /// 
    /// it is mainly recommended to avoid changing data manually unless necessary
    /// </summary>
    /// <param name="iData"></param>
    /// <param name="iIsForLoad"> this bool is only true for disk loading and called from _LoadData</param>
    /// <returns></returns>
    public bool _ChangeData(_InvData iData, bool iIsForLoad = false)
    {
        if (iData != null && _slotType != _AllWearableTypes.none)
            if (!_CanSlotStoreItem(iData._itemData)) return false;

        _data = iData;
        _UpdateUi();

        if (iIsForLoad) // only called for loading data from disk
        {
            return true;
        }

        _onSlotChange?.Invoke(iData);
        _onNewSelection?.Invoke();
        _SaveData();
        return true;
    }
    /// <summary>
    /// this method does the job of reducing stacks as well as deleting them
    /// </summary>
    public void _RemoveItem(int iCount = 1)
    {
        _data._quantity -= iCount;

        if (_data._quantity <= 0)
        {
            // we deSelect and remove the item form the inventory
            _B_ChangeSelection();
            _ChangeData(null);
        }

        else // we decreased it first and now we update the Ui 
        {
            _UpdateUi();
            _SaveData();
        }

    }
    private bool _CanSlotStoreItem(ItemData iNewData)
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
    #endregion
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
public class __FutureUpdates3
{
    /* remember to change the logic of selection so only the last selected item will change
     * the selection and remove the event Listeners for the other slots
     */
}