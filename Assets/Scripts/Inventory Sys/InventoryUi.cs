using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryUi : MonoBehaviour
{
    public static InventoryUi Instance_Player;

    [Header("General Attachments")]
    public InventorySlotUi[] _uiSlots;

    [Header("Stats Text Attachments")]
    public Text _damageText;
    public Text _armorText;
    public Text _hpText;
    public Text _attackSpeedText;

    [Header("Info Text Attachments")]
    public Text _itemNameText;
    public Text _itemDescriptionText;
    public Button _useButton;

    UnityAction _lastEvents;

    // we save them here to avoid saving them multiple times in UiInventorySlot scripts
    [HideInInspector] public Sprite _slotDefaultSprite;
    [HideInInspector] public Color _slotDefaultFrameColor;

    private void Awake()
    {
        if (Instance_Player == null)
        {
            Instance_Player = this;
        }
        else
        {
            // we do nothing so we can add multiple inventories later
        }

        _slotDefaultSprite = _uiSlots[0].GetComponent<Image>().sprite;
        _slotDefaultFrameColor = _uiSlots[0].GetComponent<Image>().color;
    }
    private void Start()
    {
        _InitButtons();
        _UpdateUiSlots();
    }
    private void OnEnable()
    {
        InventoryManager._onInventoryChange += _UpdateUiSlots;
    }
    private void OnDisable()
    {
        InventoryManager._onInventoryChange -= _UpdateUiSlots;
    }
    private void _InitButtons()
    {
        _useButton.onClick.AddListener(InventoryManager.Instance_Player._UseSelectedItem);
    }
    public void _UpdateUiSlots()
    {
        foreach (InventorySlotUi slot in _uiSlots)
        {
            slot._ClearSlot();
        }

        for (int i = 0; i < InventoryManager.Instance_Player._inventoryDataSlots.Count; i++)
        {
            if (i < _uiSlots.Length)
            {
                InventorySlotDataClass slot = InventoryManager.Instance_Player._inventoryDataSlots[i];
                _uiSlots[i]._UpdateSlot(slot.Item._invInfo._inventorySprite, slot.Quantity);
            }
        }
    }
    public void _UpdateSelectedItemInfo(ItemData iData, UnityAction iNewUnityAction = null)
    {
        if (iData == null)
        {
            _Show_Hide_ItemInfoPanel(false);
            return;
        }

        _Show_Hide_ItemInfoPanel(true);
        _UpdateItemStats(iData);
        _UpdateBasicInfo(iData);

        _ChangeEvents(iNewUnityAction);
    }
    private void _ChangeEvents(UnityAction iNewUnityAction)
    {
        if (_lastEvents != null)
            _useButton.onClick.RemoveListener(_lastEvents);
        if (iNewUnityAction != null)
            _useButton.onClick.AddListener(iNewUnityAction);
        _lastEvents = iNewUnityAction;
    }
    private void _UpdateItemStats(ItemData iData)
    {
        if (iData._canBePlaced)
        {
            _armorText.text = iData._attackInfo._armor.ToString();
            _damageText.text = iData._attackInfo._damage.ToString();
            _hpText.text = iData._attackInfo._hp.ToString();

            if (iData._attackInfo._attackSpeed != 0)
                _attackSpeedText.text = iData._attackInfo._attackSpeed.ToString();
            else
                _attackSpeedText.text = "";
        }
        else if (iData._canBeWorn)
        {
            _armorText.text = iData._defenseInfo._extraArmor.ToString();
            _damageText.text = iData._defenseInfo._extraDamage.ToString();
            _hpText.text = iData._defenseInfo._extraDamage.ToString();

            if (iData._defenseInfo._attackSpeed != 0)
                _attackSpeedText.text = iData._defenseInfo._attackSpeed.ToString();
            else
                _attackSpeedText.text = "";
        }
        else
        {
            _armorText.text = "";
            _damageText.text = "";
            _hpText.text = "";
        }
    }
    private void _UpdateBasicInfo(ItemData iData)
    {
        _itemNameText.text = iData._invInfo._name.ToString();
        _itemDescriptionText.text = iData._invInfo._description.ToString();

        if (!(iData._canBeWorn || iData._canBePlaced))
        {
            _useButton.gameObject.SetActive(false);
        }
    }
    private void _Show_Hide_ItemInfoPanel(bool iActivation)
    {
        // this should be called before every _UpdateUi event to avoid problems

        _armorText.enabled = iActivation;
        _damageText.enabled = iActivation;
        _hpText.enabled = iActivation;
        _attackSpeedText.enabled = iActivation;
        _itemNameText.enabled = iActivation;
        _itemDescriptionText.enabled = iActivation;
        _useButton.gameObject.SetActive(iActivation);
    }
}
public class __FutureUpdates_1
{
    // add a system to disable the Stats panel when it dosent have any stats shown
}