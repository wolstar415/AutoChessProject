using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Buulet_Skill9_Move : MonoBehaviour
    {
        public PhotonView pv;
        public GameObject me;
        public int EnemyIdx;
        public float damage;

        [SerializeField] private float Speed = 10f;
        // Start is called before the first frame update


        // Update is called once per frame
        public void StartFUnc(GameObject _me,Vector3 _target,float _damage,int _EnemyIdx)
        {
            me = _me;
            transform.LookAt(_target);
            damage = _damage;
            EnemyIdx = _EnemyIdx;
        }
        void Update()
        {
            if (!pv.IsMine) return;
            transform.Translate(Vector3.forward*Time.deltaTime*Speed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!pv.IsMine) return;
            if (other.gameObject.CompareTag("Enemy")||other.gameObject.CompareTag("Cards"))
            {
                if (other.TryGetComponent(out Card_Info info))
                {
                    if (info.TeamIdx==EnemyIdx)
                    {
                        
                        DamageManager.inst.DamageFunc1(me,other.gameObject,damage,eDamageType.Basic_phy);
                        me.GetComponent<Attack_9>().BulletFunc();
                        PhotonNetwork.Destroy(gameObject);
                    }
                }

            }
        }
    }
}
