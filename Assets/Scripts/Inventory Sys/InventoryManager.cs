using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MessageBoxController))]
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Ui Attachments")]
    [SerializeField] InventorySlot[] _invSlots;
    [SerializeField] _uiAttachments _uiAttach;

    [Header("Public Data")]
    public Sprite _defaultSlotSprite;
    public Color _defaultSlotColor = Color.white;
    [HideInInspector] public GameObject _dragGameObject;
    public Canvas _inventoryCanvas;

    MessageBoxController _msgBox;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _dragGameObject = new GameObject("Drag Object");
        _dragGameObject.transform.parent = _inventoryCanvas.transform;
        _dragGameObject.AddComponent<Image>();
        _dragGameObject.SetActive(false);
    }
    #region Visual
    public void _ShowSlotInfo(ItemData iData)
    {
        if (iData == null)
        {
            _ActivateInfoPanel(false);
            return;
        }

        _ActivateInfoPanel(true);
        _ShowBasicData(iData);
        _ShowStatsData(iData);
    }
    private void _ShowBasicData(ItemData iData)
    {
        _uiAttach._itemNameText.text = iData._invInfo._name;
        _uiAttach._itemDescriptionText.text = iData._invInfo._description;
    }
    private void _ShowStatsData(ItemData iData)
    {
        _Stats currentStats = iData._GetStats();

        _uiAttach._hpText.text = currentStats._hp.ToString();
        _uiAttach._armorText.text = currentStats._armor.ToString();
        _uiAttach._damageText.text = currentStats._damage.ToString();
        _uiAttach._attackSpeedText.text = currentStats._attackSpeed.ToString();
    }
    private void _ActivateInfoPanel(bool iActivation)
    {
        _uiAttach._infoPanel.SetActive(iActivation);
    }
    #endregion

    public bool _AddNewItem(ItemData iNewData)
    {
        InventorySlot firstEmpty = null;
        // check if there is any slot with enough space or stack
        for (int i = 0; i < _invSlots.Length; i++)
        {
            if (_invSlots[i]._data == null)
            {
                if (firstEmpty == null)
                    firstEmpty = _invSlots[i];
            }
            else
            {
                if (_invSlots[i]._data._itemData != iNewData) continue;

                if (_invSlots[i]._data._quantity < _invSlots[i]._data._itemData._invInfo._maxStack)
                {
                    _invSlots[i]._data._quantity++;
                    return true;
                }
            }
        }
        // if we are here it means we can't stack anywhere
        if (firstEmpty != null)
        {
            firstEmpty._ChangeData(new _InvData(iNewData));
            return true;
        }
        // if we are here it means there is no available slot in the inventory
        _msgBox._StartMsg();
        return false;
    }
    public void _RemoveItemFromSlot(_InvData iData, InventorySlot iSlot)
    {
        if (iSlot._data._itemData != iData._itemData) return;

        if (iSlot._data._quantity > iData._quantity)
            iSlot._ChangeData(null);
        else
        {
            iData._quantity = iSlot._data._quantity - iData._quantity;
            iSlot._ChangeData(iData);
        }
    }

    [System.Serializable]
    public class _uiAttachments
    {
        [Header("General Attachments")]
        public GameObject _infoPanel;

        [Header("Stats Text Attachments")]
        public Text _damageText;
        public Text _armorText;
        public Text _hpText;
        public Text _attackSpeedText;

        [Header("Info Text Attachments")]
        public Text _itemNameText;
        public Text _itemDescriptionText;
        public Button _useButton;
    }
}
