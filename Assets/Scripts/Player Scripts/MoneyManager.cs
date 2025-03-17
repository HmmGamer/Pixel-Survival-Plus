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
        _msgBox = GetComponent<MessageBoxController>();
        _UpdateUi();
    }
    public void _AddMoney(int iAmount)
    {
        _totalMoney += iAmount;
        _UpdateUi();
    }
    public bool _PurchaseItem(int iCost, bool iShowTimeline = true)
    {
        if (iCost <= _totalMoney)
        {
            _totalMoney -= iCost;
            _UpdateUi();
            return true;
        }
        _ShowPurchaseTimeline(iShowTimeline);
        return false;
    }
    public bool _PurchaseItem(ItemData iData, bool iShowTimeline = true)
    {
        if (iData._shopInfo._sellPrice <= _totalMoney)
        {
            _totalMoney -= iData._shopInfo._sellPrice;
            _UpdateUi();
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
        if (iData._shopInfo._sellPrice <= _totalMoney)
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