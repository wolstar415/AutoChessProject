using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Buulet_Skill34_Move : MonoBehaviour
    {
        public GameObject target;
        public GameObject me;
        public float damage;
        public int playeridx;
        [SerializeField] private float Speed = 10f;
        // Start is called before the first frame update
        public bool IsStart = false;

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
                DestoryFunc();
                return;
            }
            if (target.TryGetComponent(out UnitState state))
            {
                if (state.IsDead==true)
                {
                    DestoryFunc();
                    return;
                }
            }
            
            transform.LookAt(target.transform);

            transform.Translate(Vector3.forward*Time.deltaTime*Speed);
        }

        void DestoryFunc()
        {
            gameObject.SetActive(false);
        }
        private void OnTriggerEnter(Collider other)
        {
       
            if (other.gameObject==target)
            {
                if (playeridx == PlayerInfo.Inst.PlayerIdx)
                {
                DamageManager.inst.DamageFunc1(me,target,damage,eDamageType.Speel_Magic);
                me.GetComponent<Attack_34>().DamageFunc();
                }
                DestoryFunc();
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
