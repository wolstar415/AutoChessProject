using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Buulet_Skill22_Move : MonoBehaviour
    {
        public int playeridx;
        public GameObject target;
        public GameObject me;
        public float damage;
        public bool IsStart = false;
        [SerializeField] private float Speed = 10f;
        // Start is called before the first frame update


        // Update is called once per frame
        public void StartFUnc(int _playeridx,GameObject _me, GameObject _target, float _damage)
        {
            playeridx = _playeridx;
            me = _me;
            target = _target;
            damage = _damage;
            IsStart = true;
        }
        void Update()
        {
            if (!IsStart) return;
            if (target == null)
            {
                gameObject.SetActive(false);
                return;
            }
            if (target.TryGetComponent(out UnitState state))
            {
                if (state.IsDead==true)
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
            
            transform.LookAt(target.transform);

            transform.Translate(Vector3.forward*Time.deltaTime*Speed);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!IsStart) return;
            if (other.gameObject==target)
            {

                if (playeridx == PlayerInfo.Inst.PlayerIdx)
                {
                DamageManager.inst.DamageFunc1(me,target,damage,eDamageType.Speel_Magic,1);
                    
                }
                gameObject.SetActive(false);
            }
        }
        private void OnDisable()
        {
            playeridx = 0;
            me = null;
            target = null;
            IsStart = false;
        }
    }
}
