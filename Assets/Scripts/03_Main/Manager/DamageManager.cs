using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public enum eDamageType
    {
        Basic_phy,
        Basic_Magic,
        Spell_phy,
        Speel_Magic,
    }
    public class DamageManager : MonoBehaviour
    {
        public static DamageManager inst;

        private void Awake() => inst = this;

        public void DamageFunc1(GameObject card, GameObject target,float damage,eDamageType Type=eDamageType.Basic_phy)
        {
            
        }

    }
}
