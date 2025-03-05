using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Data", menuName = "TahaScripts/CreateNewWave")]
public class WaveDataBase : ScriptableObject
{
    public _AllWavesStruct[] _allWaves;

    [System.Serializable]
    public struct _AllWavesStruct
    {
        public float _spawnDelay;
        public _EachWaveStruct[] _wave;
    }
    [System.Serializable]
    public struct _EachWaveStruct
    {
        public GameObject _enemyPrefab;
        public int _count;
    }
}
