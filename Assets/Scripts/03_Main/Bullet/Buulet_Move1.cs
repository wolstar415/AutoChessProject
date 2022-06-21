using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Buulet_Move1 : MonoBehaviour
    {
        public PhotonView pv;
        public GameObject target;
        public GameObject me;
        public float damage;
        private bool phy = false;

        [SerializeField] private float Speed = 10f;
        // Start is called before the first frame update


        // Update is called once per frame
        public void StartFUnc(GameObject _me,GameObject _target,float _damage,bool _phy=true)
        {
            me = _me;
            target = _target;
            damage = _damage;
            phy = _phy;
        }
        void Update()
        {
            if (!pv.IsMine) return;
            if (target == null) return;
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
            PhotonNetwork.Destroy(gameObject);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!pv.IsMine) return;
            if (other.gameObject==target)
            {
                if (phy)
                {
                DamageManager.inst.DamageFunc1(me,target,damage);
                    
                }
                else
                {
                DamageManager.inst.DamageFunc1(me,target,damage,eDamageType.Basic_Magic);
                    
                }
                DestoryFunc();
            }
        }
    }
}
