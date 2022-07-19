using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class Buulet_Move1 : MonoBehaviour
    {
        public GameObject target;
        public GameObject me;
        public float damage;
        private bool phy = false;
        public int buIdx = 0;
        public int playeridx;
        public bool IsStart = false;
        [SerializeField] private float Speed = 10f;
        // Start is called before the first frame update


        // Update is called once per frame
        public void StartFUnc(int _playeridx,GameObject _me, GameObject _target, float _damage,bool _phy = true, int _buIdx = 0)
        {
            playeridx = _playeridx;
            me = _me;
            target = _target;
            damage = _damage;
            phy = _phy;
            buIdx = _buIdx;
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
            if (!IsStart) return;
            if (other.gameObject==target)
            {
                if (playeridx == PlayerInfo.Inst.PlayerIdx)
                {
                    
                    if (phy)
                    {
                    DamageManager.inst.DamageFunc1(me,target,damage);
                        
                    }
                    else
                    {
                    DamageManager.inst.DamageFunc1(me,target,damage,eDamageType.Spell_Magic);
                    if (buIdx==1)
                    {
                        Vector3 dir=target.transform.position-transform.position;
                        dir.Normalize();
                        dir.y = 0;
                        target.GetComponent<CardState>().Knockback(dir,2);
                    }
                    }
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
