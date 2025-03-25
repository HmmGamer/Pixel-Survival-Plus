using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance;

    [SerializeField] Button _useButton;

    InventorySlot _currentSlot;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public void _ChangeActivation(bool iActivation, InventorySlot iSlot = null, ItemData iData = null)
    {
        _currentSlot = iSlot;
        _useButton.gameObject.SetActive(iActivation);
        if (iData == null) return;

        if (iData._type == _ItemDataType.building)
        {
            _ChangeEvent(() =>
                {
                    BuildController.Instance._BuildObject(iData._towerInfo._towerPrefab);
                    _RemoveFromInventory();
                });
        }
        else if (iData._type == _ItemDataType.equipment)
        {
            _useButton.gameObject.SetActive(false);
        }
        else if (iData._type == _ItemDataType.potion)
        {
            if (iData._potionInfo._type == _AllPotionTypes.heal)
                _ChangeEvent(() =>
                {
                    PlayerController.instance._ConsumeHpPotion(iData._potionInfo._hpRestore);
                    _RemoveFromInventory();
                });
        }
    }
    private void _ChangeEvent(UnityAction iNewAction)
    {
        _useButton.onClick.RemoveAllListeners();
        _useButton.onClick.AddListener(iNewAction);
    }
    private void _RemoveFromInventory()
    {
        _currentSlot._RemoveItem();
    }
}
public class __FutureUpdates2
{
    // activate use button for equipping as well
}