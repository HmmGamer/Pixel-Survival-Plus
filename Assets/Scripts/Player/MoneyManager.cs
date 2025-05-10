using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[RequireComponent(typeof(MessageBoxController))]
public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    [SerializeField] int _Default_SCoin;
    //[SerializeField] int _Default_GCoin;
    [SerializeField] Text _coinText;
    MessageBoxController _msgBox;

    [ReadOnly, SerializeField] int _totalMoney;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        _LoadMoney();
        _msgBox = GetComponent<MessageBoxController>();
        _UpdateUi();
    }
    public void _LoadMoney()
    {
        _totalMoney = PlayerPrefs.GetInt(A.DataKey.money, _Default_SCoin);
    }
    public void _SaveMoney()
    {
        PlayerPrefs.SetInt(A.DataKey.money, _totalMoney);
    }
    [CreateButton("Reset Money")]
    public void _ResetMoney()
    {
        PlayerPrefs.DeleteKey(A.DataKey.money);
        _LoadMoney();
    }
    public void _AddMoney(int iAmount)
    {
        _totalMoney += iAmount;
        _UpdateUi();
        _SaveMoney();
    }
#if UNITY_EDITOR
    [CreateButton("Add 100 Money")]
    public void _AddCheatMoney()
    {
        _AddMoney(100);
    }
#endif
    public bool _PurchaseItem(int iCost, bool iShowTimeline = true)
    {
        if (iCost <= _totalMoney)
        {
            _totalMoney -= iCost;
            _UpdateUi();
            _SaveMoney();
            return true;
        }
        _ShowPurchaseTimeline(iShowTimeline);
        return false;
    }
    public bool _PurchaseItem(ItemData iData, bool iShowTimeline = true)
    {
        if (iData._shopInfo._buyPrice <= _totalMoney)
        {
            _totalMoney -= iData._shopInfo._sellPrice;
            _UpdateUi();
            _SaveMoney();
            return true;
        }
        _ShowPurchaseTimeline(iShowTimeline);
        return false;
    }
    public bool _CanPurchaseItem(int iCost, bool iShowTimeline = true)
    {
        if (iCost <= _totalMoney)
        {
            return true;
        }
        _ShowPurchaseTimeline(iShowTimeline);
        return false;
    }
    public bool _CanPurchaseItem(ItemData iData, bool iShowTimeline = true)
    {
        if (iData._shopInfo._buyPrice <= _totalMoney)
        {
            return true;
        }
        _ShowPurchaseTimeline(iShowTimeline);
        return false;
    }
    private void _ShowPurchaseTimeline(bool iShowNoMoney)
    {
        if (iShowNoMoney)
            _msgBox._StartMsg();
    }
    private void _UpdateUi()
    {
        _coinText.text = _totalMoney.ToString();
    }
}