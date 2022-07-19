using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Buulet_Move2 : MonoBehaviour
    {
        public GameObject me;
        public int EnemyIdx;
        public float damage;
        private bool phy = false;
        public List<GameObject> NoDamage=new List<GameObject>();
        public bool IsStart = false;
        [SerializeField] private float Speed = 10f;
        public int playeridx;
        // Start is called before the first frame update


        // Update is called once per frame
        public void StartFUnc(int _playeridx,GameObject _me,Vector3 _target,float _damage,int _EnemyIdx,bool _phy=true)
        {
            NoDamage.Clear();
            playeridx = _playeridx;
            me = _me;
            transform.LookAt(_target);
            damage = _damage;
            EnemyIdx = _EnemyIdx;
            phy = _phy;
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
            if (playeridx!=PlayerInfo.Inst.PlayerIdx) return;
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
                            DamageManager.inst.DamageFunc1(me,other.gameObject,damage,eDamageType.Spell_Magic);
                    
                        }
                        NoDamage.Add(other.gameObject);
                        
                        
                        
                        
                        
                        
                        
                        
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
