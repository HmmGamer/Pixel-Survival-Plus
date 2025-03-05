using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot Data", menuName = "TahaScripts/CreateNewLoot")]
public class LootDatabase : ScriptableObject
{
    [SerializeField, Range(0, 100)] int _lootChance;
    public _AllLootsStruct[] _allLoots;

    private int _allChancesCount = 0;

    private void Awake()
    {
        _CountAllChances();
    }
    public GameObject _GetLoot()
    {
        if (Random.Range(0, 100) >= _lootChance)
        {
            return null;
        }
        return _allLoots[_GetNextRandomLootIndex()]._lootPrefab;
    }
    private int _GetNextRandomLootIndex()
    {
        int randomValue = Random.Range(0, _allChancesCount);
        int cumulativeChance = 0;

        for (int i = 0; i < _allLoots.Length; i++)
        {
            cumulativeChance += _allLoots[i]._chance;
            if (randomValue < cumulativeChance)
            {
                return i;
            }
        }

        return 0;
    }
    private void _CountAllChances()
    {
        foreach (_AllLootsStruct item in _allLoots)
        {
            _allChancesCount += item._chance;
        }
    }

    [System.Serializable]
    public struct _AllLootsStruct
    {
        public int _chance;
        public GameObject _lootPrefab;
    }
}
