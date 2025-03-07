using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot Data", menuName = "TahaScripts/CreateNewLoot")]
public class LootDatabase : ScriptableObject
{
    [SerializeField, Range(0, 100)] int _lootChance;
    public _AllLootsStruct[] _allLoots;

    private int _totalChances = 0;

    private void OnEnable()
    {
        _CountAllChances();
    }
    public GameObject _GetLoot()
    {
        if (_lootChance <= 0 || Random.Range(0, 100) >= _lootChance)
        {
            return null;
        }

        return _allLoots[_GetNextRandomLootIndex()]._lootPrefab;
    }
    private int _GetNextRandomLootIndex()
    {
        int randomValue = Random.Range(0, _totalChances);
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
        _totalChances = 0;
        foreach (_AllLootsStruct loot in _allLoots)
        {
            if (loot._chance < 0 || loot._chance > 100)
            {
                Debug.LogError("Loot chance value must be between 0 and 100.");
                continue;
            }
            _totalChances += loot._chance;
        }

        if (_totalChances <= 0)
        {
            Debug.LogWarning("Total chances sum is 0, no loot will drop.");
        }
    }
    [System.Serializable]
    public struct _AllLootsStruct
    {
        [Range(0, 100)] public int _chance;
        public GameObject _lootPrefab;
    }
}
