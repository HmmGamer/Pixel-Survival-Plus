using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;
    public static event UnityAction _onStatsChange;

    [Header("Attachments")]
    [SerializeField] _EquipmentButtonsClass _equipmentButtons;
    [SerializeField] _StatsTextsClass _statTexts;

    public _DefenseInfoClass _currentStats = new _DefenseInfoClass();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public void _CalculateAllStats()
    {
        _ResetStats();

        _AddNewStat(_equipmentButtons._head._GetStats());
        _AddNewStat(_equipmentButtons._body._GetStats());
        _AddNewStat(_equipmentButtons._legs._GetStats());
        _AddNewStat(_equipmentButtons._foot._GetStats());
        _AddNewStat(_equipmentButtons._weapon._GetStats());
        _AddNewStat(_equipmentButtons._shield._GetStats());

        _UpdateStatsUi();
        _onStatsChange?.Invoke();
    }
    public void _ChangeEquipment(ItemData item)
    {
        if (!item._canBeWorn) return;

        if (item._defenseInfo._wearableType == _WearableTypes.head)
        {
            _equipmentButtons._head._SetCurrentItem(item);
        }
        else if (item._defenseInfo._wearableType == _WearableTypes.body)
        {
            _equipmentButtons._body._SetCurrentItem(item);
        }
        else if (item._defenseInfo._wearableType == _WearableTypes.legs)
        {
            _equipmentButtons._legs._SetCurrentItem(item);
        }
        else if (item._defenseInfo._wearableType == _WearableTypes.foot)
        {
            _equipmentButtons._foot._SetCurrentItem(item);
        }
        else if (item._defenseInfo._wearableType == _WearableTypes.weapon)
        {
            _equipmentButtons._weapon._SetCurrentItem(item);
        }
        else if (item._defenseInfo._wearableType == _WearableTypes.shield)
        {
            _equipmentButtons._weapon._SetCurrentItem(item);
        }
        InventoryManager.Instance_Player._RemoveItem(item);
        _CalculateAllStats();
    }
    public void _RemoveEquipment(ItemData item)
    {

    }
    private void _ResetStats()
    {
        _currentStats._attackSpeed = 0;
        _currentStats._extraDamage = 0;
        _currentStats._extraArmor = 0;
        _currentStats._extraHp = 0;
    }
    private void _AddNewStat(_DefenseInfoClass iStat)
    {
        if (iStat == null) return;
        _currentStats._attackSpeed += iStat._attackSpeed;
        _currentStats._extraHp += iStat._extraHp;
        _currentStats._extraArmor += iStat._extraArmor;
        _currentStats._extraDamage += iStat._extraDamage;
    }
    private void _UpdateStatsUi()
    {
        _statTexts._armorText.text = _currentStats._extraArmor.ToString();
        _statTexts._damageText.text = _currentStats._extraDamage.ToString();
        _statTexts._hpText.text = _currentStats._extraHp.ToString();
        _statTexts._attackSpeedText.text = _currentStats._attackSpeed.ToString();
    }

    [Serializable]
    public class _EquipmentButtonsClass
    {
        public EquipmentSlots _head;
        public EquipmentSlots _body;
        public EquipmentSlots _legs;
        public EquipmentSlots _foot;
        public EquipmentSlots _weapon;
        public EquipmentSlots _shield;
    }
    [Serializable]
    public class _StatsTextsClass
    {
        public Text _hpText;
        public Text _armorText;
        public Text _damageText;
        public Text _attackSpeedText;
    }
}
public enum _WearableTypes
{
    head, body, legs, foot, weapon, shield
}