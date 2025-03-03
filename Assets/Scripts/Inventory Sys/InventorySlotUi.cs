using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventorySlotUi : MonoBehaviour
{
    public static event UnityAction _onSelection;

    [SerializeField] Color _frameColor;

    public Image _selectedFrame;
    public Button _slotButton;
    public Image _iconImage;
    public Text _quantityText;

    bool _isSelected;

    private void Start()
    {
        _InitButtonEvents();
    }
    private void OnEnable()
    {
        _onSelection += _ResetSelectedItem;
    }
    private void OnDisable()
    {
        _onSelection -= _ResetSelectedItem;
    }
    private void _InitButtonEvents()
    {
        _slotButton.onClick.AddListener(_B_SelectItem);
    }
    private void _B_SelectItem()
    {
        if (_isSelected)
        {
            _ResetSelectedItem();
            return;
        }

        _onSelection?.Invoke();
        _isSelected = true;
        _selectedFrame.color = Color.yellow;

        int slotIndex = transform.GetSiblingIndex();
        InventoryManager.Instance_Player._SaveSelectedItem(slotIndex);
    }
    private void _ResetSelectedItem()
    {
        _isSelected = false;
        _selectedFrame.color = InventoryUi.Instance_Player._slotDefaultFrameColor;
        InventoryManager.Instance_Player._SaveSelectedItem(-1);
    }
    public void _UpdateSlot(Sprite icon, int quantity)
    {
        // this method is called in the inventoryUi after all of the slots are set to unclickable
        // so when its called it means there is an object inside it and it should be clickable
        _slotButton.interactable = true;
        _iconImage.sprite = icon;

        _quantityText.text = quantity.ToString();
        if (quantity == 1)
            _quantityText.text = "";
    }
    public void _ClearSlot()
    {
        _ResetSelectedItem();
        _slotButton.interactable = false;
        _iconImage.sprite = InventoryUi.Instance_Player._slotDefaultSprite;

        _quantityText.text = "";
    }
    public static void _ResetSelection()
    {
        _onSelection?.Invoke();
    }
}