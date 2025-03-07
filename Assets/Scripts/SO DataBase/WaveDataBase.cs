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
        [Tooltip("true => spawn enemy as the array. false=> rotate enemy spawn")]
        public bool _finishEachWaveFirst;

        public float _spawnDelay; 
        public _EachWaveStruct[] _waves;
    }
    [System.Serializable]
    public struct _EachWaveStruct
    {
        public GameObject _enemyPrefab;
        public int _count;
    }
}
