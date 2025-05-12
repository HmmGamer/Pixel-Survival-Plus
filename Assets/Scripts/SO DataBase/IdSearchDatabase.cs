using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Search Database", menuName = "TahaScripts/CreateNewSearchData")]
public class IdSearchDatabase : ScriptableObject
{
    public List<ItemData> _allItems;

    public ItemData _GetItemDataByID(string iItemID)
    {
        foreach (var _item in _allItems)
        {
            if (_item._invInfo._name == iItemID)
                return _item;
        }
        Debug.LogError("Your DataBase does not have this Item :" + iItemID);
        return null;
    }
#if UNITY_EDITOR
    private void Awake()
    {
        _CheckDuplicatedIdNames();
    }
    public void _CheckDuplicatedIdNames()
    {
        HashSet<string> _seenIds = new HashSet<string>();
        List<string> _duplicateIds = new List<string>();
        foreach (var _item in _allItems)
        {
            if (_item == null) continue;
            string _id = _item._invInfo._name;
            if (string.IsNullOrEmpty(_id)) continue;
            if (_seenIds.Contains(_id))
            {
                if (!_duplicateIds.Contains(_id))
                    _duplicateIds.Add(_id);
            }
            else
            {
                _seenIds.Add(_id);
            }
        }
        if (_duplicateIds.Count > 0)
        {
            string _duplicates = string.Join(", ", _duplicateIds);
            Debug.LogError("Duplicate Item IDs found in IdSearchDatabase: " + _duplicates, this);
        }
    }
#endif
}
