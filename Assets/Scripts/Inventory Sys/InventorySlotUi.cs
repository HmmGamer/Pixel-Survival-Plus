using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventorySlotUi : MonoBehaviour
{
    public static event UnityAction _onSelection;

    public GameObject _selectedFrame;
    public Button _slotButton;
    public Image _iconImage;
    public Text _quantityText;

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
        _onSelection?.Invoke();
        _selectedFrame.SetActive(true);
    }
    private void _ResetSelectedItem()
    {
        _selectedFrame.SetActive(false);
    }
    public void _UpdateSlot(Sprite icon, int quantity)
    {
        _iconImage.enabled = true;
        _iconImage.sprite = icon;
        _quantityText.text = quantity.ToString();
        if (quantity == 1)
            _quantityText.text = "";
    }
    public void _ClearSlot()
    {
        _iconImage.sprite = null;
        _iconImage.enabled = false;
        _quantityText.text = "";
    }
}
