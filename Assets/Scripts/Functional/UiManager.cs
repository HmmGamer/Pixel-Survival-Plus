using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    [SerializeField] GameObject _inventoryCanvas;
    [SerializeField] GameObject _extraPanel;
    [SerializeField] Button _openInventoryButton;
    [SerializeField] Button _closeInventoryButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        _InitButtons();
    }
    private void _InitButtons()
    {
        _openInventoryButton.onClick.AddListener(() => _ActivateInventory(true));
        _closeInventoryButton.onClick.AddListener(() => _ActivateInventory(false));
    }
    // reminder : the inventory's basic exit button is in the extra panel
    public void _ActivateInventory(bool iActivation, bool iExtraPanelActivation = true)
    {
        if (!_inventoryCanvas) return;

        _inventoryCanvas.SetActive(iActivation);
        _openInventoryButton.gameObject.SetActive(!iActivation);
        _extraPanel.SetActive(iExtraPanelActivation);
    }
}
