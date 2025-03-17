using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;
    public static event UnityAction _onUpdateStats;
    public static event UnityAction _onNewStats;

    [Header("General Settings")]
    [SerializeField] _Stats _defaultStats;

    [Header("Total Stats Text Attachments")]
    [SerializeField] Text _totalDamageText;
    [SerializeField] Text _totalHpText;
    [SerializeField] Text _totalArmorText;
    [SerializeField] Text _totalAttackSpeedText;

    [HideInInspector] public _Stats _totalStats = new _Stats();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        _CalculateStats();
    }
    public void _CalculateStats()
    {
        _totalStats._ResetStats();
        _onUpdateStats?.Invoke();
        _totalStats += _defaultStats;
        _UpdateStatsUi();
        _onNewStats?.Invoke();
    }
    public void _AddNewStat(_Stats iStat)
    {
        _totalStats += iStat;
    }
    private void _UpdateStatsUi()
    {
        _totalDamageText.text = _totalStats._damage.ToString();
        _totalArmorText.text = _totalStats._armor.ToString();
        _totalHpText.text = _totalStats._hp.ToString();
        _totalAttackSpeedText.text = _totalStats._attackSpeed.ToString();
    }
    //private void Update()
    //{
    //    print(_totalStats._attackSpeed);
    //}
}
