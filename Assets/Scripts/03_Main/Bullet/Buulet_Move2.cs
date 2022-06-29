using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Buulet_Move2 : MonoBehaviour
    {
        public PhotonView pv;
        public GameObject me;
        public int EnemyIdx;
        public float damage;
        private bool phy = false;
        private bool IsDestory = false;
        public List<GameObject> NoDamage;

        [SerializeField] private float Speed = 10f;
        // Start is called before the first frame update


        // Update is called once per frame
        public void StartFUnc(GameObject _me,Vector3 _target,float _damage,int _EnemyIdx,bool _IsDestory,bool _phy=true)
        {
            me = _me;
            transform.LookAt(_target);
            damage = _damage;
            EnemyIdx = _EnemyIdx;
            IsDestory = _IsDestory;
            phy = _phy;
        }
        void Update()
        {
            if (!pv.IsMine) return;
            transform.Translate(Vector3.forward*Time.deltaTime*Speed);
        }

        void DestoryFunc()
        {
            PhotonNetwork.Destroy(gameObject);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!pv.IsMine) return;
            if (other.gameObject.CompareTag("Enemy")||other.gameObject.CompareTag("Cards")&&NoDamage.Contains(other.gameObject)==false)
            {
                if (other.TryGetComponent(out Card_Info info))
                {
                    if (info.TeamIdx==EnemyIdx&&info.IsFiled)
                    {
                        
                        if (phy)
                        {
                            DamageManager.inst.DamageFunc1(me,other.gameObject,damage,eDamageType.Basic_phy);
                    
                        }
                        else
                        {
                            DamageManager.inst.DamageFunc1(me,other.gameObject,damage,eDamageType.Speel_Magic);
                    
                        }
                        NoDamage.Add(other.gameObject);
                        if(IsDestory) DestoryFunc();
                        
                        
                        
                        
                        
                        
                        
                    }
                }

            }
        }
    }
}
