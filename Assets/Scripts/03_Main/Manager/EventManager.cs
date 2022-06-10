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

        private void Awake()
        {
            inst = this;
        }

    }
}
