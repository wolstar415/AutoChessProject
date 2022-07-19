using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Buulet_Skill29_Move : MonoBehaviour
    {
        public int playeridx;
        public GameObject me;
        public float damage;
        public int EnemyIdx;
        public bool IsStart = false;
        [SerializeField] private float Speed = 10f;
        // Start is called before the first frame update


        // Update is called once per frame
        public void StartFUnc(int _playeridx,GameObject _me, float _damage,int _EnemyIdx)
        {
            me = _me;
            playeridx = _playeridx;
            damage = _damage; 
            EnemyIdx=_EnemyIdx;
            IsStart = true;
        }
        void Update()
        {
            if (!IsStart) return;
            transform.Translate(Vector3.forward*Time.deltaTime*Speed);
        }


        private void OnTriggerEnter(Collider other)
        {

            if (!IsStart) return;
            
            if (other.gameObject.CompareTag("Enemy")||other.gameObject.CompareTag("Cards"))
            {
                if (other.TryGetComponent(out Card_Info info))
                {
                    if (info.TeamIdx==EnemyIdx&&info.IsFiled)
                    {
                        
                        DamageManager.inst.DamageFunc1(me,other.gameObject,damage,eDamageType.Spell_Magic);

                        gameObject.SetActive(false);

                    }
                }

            }
            
        }
        private void OnDisable()
        {
            playeridx = 0;
            me = null;
            EnemyIdx = 0;
            IsStart = false;
        }
    }
}
