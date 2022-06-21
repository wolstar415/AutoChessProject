using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GameS
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager inst;
        public Subject<int> Sub_CardMove = new Subject<int>();
        public Subject<int> Sub_CardBattleMove = new Subject<int>();
        public Subject<int> Sub_LevelUpCheck = new Subject<int>();
        
        public Subject<float> Sub_Item40Func = new Subject<float>();
        
        
       //public Subject<int> Sub_CardJobAndTraitShow = new Subject<int>();
        //public Subject<int> Sub_CardJobAndTraitNoShow = new Subject<int>();

        private void Awake()
        {
            inst = this;
        }

    }
}
