using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public static TestScript instance;

    public GameObject[] _ObjectForSpawn;
    int spawnIndex;
    List<GameObject> _SpawnList = new List<GameObject>();

    public GameObject _myInstantiate(GameObject iObject, Vector3 iPos, Quaternion iRotation)
    {
        foreach (GameObject oldObject in _SpawnList)
        {
            if (!oldObject.activeInHierarchy)
            {
                if (oldObject.name.StartsWith(iObject.name))
                {
                    oldObject.SetActive(true);


                    oldObject.transform.position = iPos;
                    oldObject.transform.rotation = iRotation;
                    return oldObject;
                }

            }

        }
        _SpawnList.Add(Instantiate(iObject, iPos, iRotation));
        return iObject;
    }
}
