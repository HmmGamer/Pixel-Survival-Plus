using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralTools : MonoBehaviour
{
    public static class HpTools
    {
        public static int _CalculateDamage(int iDamage, int iArmor)
        {
            iDamage = iDamage - iArmor / 3;
            if (iDamage <= 0)
            {
                if (iDamage * 2 > iArmor)
                    iDamage = 1;
                else
                    iDamage = 0;
            }

            return iDamage;
        }
    }
}
