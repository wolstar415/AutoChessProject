using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameS
{
    public class Buulet_Skill51_Move : MonoBehaviour
    {
        public GameObject target;
        public GameObject me;
        public float damage;

        private Vector3[] pos = new Vector3[4];
        private float curTime=0;
        private float coolTime;
        public int playeridx;
        [Header("표시")] 
        public bool IsStart = false;
        
        [SerializeField] private float Speed = 2f;
        // Start is called before the first frame update


        // Update is called once per frame
        public void StartFUnc(int _playeridx,GameObject _me, GameObject _target, float _damage)
        {
            coolTime = Random.Range(0.8f, 1.0f);
            
            me = _me;
            target = _target;
            playeridx = _playeridx;
            pos[0] = me.transform.position;
            pos[3] = target.transform.position;
            pos[1] = pos[0] +
                     (6 * Random.Range(-1.0f, 1.0f) * me.transform.right) +
                     (6 * Random.Range(-0.15f, 1.0f) * me.transform.up) + 
                     (6 * Random.Range(-1.5f, -1.0f) * me.transform.forward); 

            pos[2] = pos[3] +
                     (3 * Random.Range(-1.0f, 1.0f) * target.transform.right) +
                     (3 * Random.Range(-1.0f, 1.0f) * target.transform.up) + 
                     (3 * Random.Range(0.8f, 1.0f) * target.transform.forward); 
            
            IsStart = true;
            damage = _damage;
        }
        void Update()
        {
            if (!IsStart) return;
            if (target == null||curTime>=coolTime)
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
            curTime += Time.deltaTime * Speed;
            
            transform.LookAt(target.transform);

            //transform.Translate(Vector3.forward*Time.deltaTime*Speed);
            transform.position = new Vector3(
                Bezier(pos[0].x, pos[1].x, pos[2].x, pos[3].x),
                Bezier(pos[0].y, pos[1].y, pos[2].y, pos[3].y),
                Bezier(pos[0].z, pos[1].z, pos[2].z, pos[3].z)
            );
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
                    
                DamageManager.inst.DamageFunc1(me,target,damage,eDamageType.Spell_Magic);
                }
                DestoryFunc();
            }
        }
        
        private float Bezier(float a, float b, float c, float d)
        {
            float t = curTime / coolTime; 
            
            float ab = Mathf.Lerp(a, b, t);
            float bc = Mathf.Lerp(b, c, t);
            float cd = Mathf.Lerp(c, d, t);

            float abbc = Mathf.Lerp(ab, bc, t);
            float bccd = Mathf.Lerp(bc, cd, t);

            return Mathf.Lerp(abbc, bccd, t);
        }
        private void OnDisable()
        {
            playeridx = 0;
            me = null;
            target = null;
            IsStart = false;
            pos[0]=Vector3.zero;
            pos[1]=Vector3.zero;
            pos[2]=Vector3.zero;
            pos[3]=Vector3.zero;
            curTime = 0;
        }
    }
}
