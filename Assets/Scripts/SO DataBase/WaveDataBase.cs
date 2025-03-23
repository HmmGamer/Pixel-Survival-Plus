using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Data", menuName = "TahaScripts/CreateNewWave")]
public class WaveDataBase : ScriptableObject
{
    public _AllWavesStruct[] _allWaves;

    #region editor only
#if UNITY_EDITOR
    /// <summary>
    /// we set the _canFly in here to improve performance in spawning
    /// this avoids using GetComponent and checking _canFly for each enemy in mid game
    /// </summary>
    [CreateButton("Update DataBase")]
    private void _UpdateDatabase()
    {
        for (int i = 0; i < _allWaves.Length; i++)
        {
            if (_allWaves[i] == null)
            {
                Debug.LogError("You cant have a null wave");
                return;
            }
            for (int b = 0; b < _allWaves[i]._waves.Length; b++)
            {
                if (_allWaves[i]._waves[b] == null && !_allWaves[i]._waves[b]._enemyPrefab)
                {
                    Debug.LogError("You cant have a null wave/prefab");
                    return;
                }
                _allWaves[i]._waves[b]._canFly =
                    _allWaves[i]._waves[b]._enemyPrefab.GetComponent<EnemyController>()
                    ._GetCanFly();
            }
        }
        Debug.Log("Updated");
    }
#endif
    #endregion

    [System.Serializable]
    public class _AllWavesStruct
    {
        [Tooltip("true => spawn enemy as the array. false=> rotate enemy spawn")]
        public bool _finishEachEnemyFirst;

        public float _spawnDelay;
        public _EachWaveStruct[] _waves;
    }
    [System.Serializable]
    public class _EachWaveStruct
    {
        public GameObject _enemyPrefab;
        [HideInInspector] public bool _canFly;
        public int _count;
    }
}
