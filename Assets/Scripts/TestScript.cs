using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public bool _e2;
    [ConditionField(nameof(_e2))] public _C _c;

    [System.Serializable]
    public class _C
    {
        public bool _e;
        [ConditionField(nameof(_e))] public int _i;
    }
}
