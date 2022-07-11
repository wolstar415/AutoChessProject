using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Buulet_Skill9_Move : MonoBehaviour
    {
        public GameObject me;
        public int EnemyIdx;
        public float damage;
        public int playeridx;
        [SerializeField] private float Speed = 10f;
        // Start is called before the first frame update
        public bool IsStart = false;

        // Update is called once per frame
        public void StartFUnc(int _playeridx,GameObject _me,Vector3 _target,float _damage,int _EnemyIdx)
        {
            playeridx = _playeridx;
            me = _me;
            transform.LookAt(_target);
            damage = _damage;
            EnemyIdx = _EnemyIdx;
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
                        if (playeridx == PlayerInfo.Inst.PlayerIdx)
                        {
                            
                        DamageManager.inst.DamageFunc1(me,other.gameObject,damage,eDamageType.Basic_phy);
                        me.GetComponent<Attack_9>().BulletFunc();
                        }
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
