using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    [SerializeField] int _DefaultMoney;
    [SerializeField] Text _coinText;
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
        _UpdateUi();
    }
    public void _AddMoney(int iAmount)
    {
        _totalMoney += iAmount;
        _UpdateUi();
    }
    public bool _PurchaseItem(int iCost)
    {
        if (iCost <= _totalMoney)
        {
            _totalMoney -= iCost;
            _UpdateUi();
            return true;
        }
        return false;
    }
    private void _UpdateUi()
    {
        _coinText.text = _totalMoney.ToString();
    }
}
