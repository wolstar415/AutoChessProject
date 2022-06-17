using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace GameS
{
    public class UnitState : MonoBehaviourPunCallbacks
    {
        public float attackTime=1f;
        public PhotonView pv;
        public BoxCollider collider;
        public NavMeshAgent nav;
        public Animator ani;
        public bool IsCard = true;
        [SerializeField]protected GameObject StunOb; // 스턴오브젝트
        [SerializeField] protected float _currentHp; //현재체력

        [SerializeField]protected float _shiled; // 쉴드
        [SerializeField]protected float _currentMana; // 마나
        //[SerializeField]protected float _range; // 사거리
         // 최대마나

        [SerializeField] protected int KillCnt; //라운드 킬카운트
        [SerializeField] protected int AttackCnt; //평타횟수
        
        [SerializeField] protected GameObject AttackOb; //평타대상
        [SerializeField] protected int OneAttackCnt; //같은대상 평타횟수
        

        [Header("UI")] 
        [SerializeField] protected Slider HpSlider;
        [SerializeField] protected Slider HpSliderCheck;
        [SerializeField] protected Slider ManaSlider;
        [SerializeField] protected Slider ShiledSlider1;
        [SerializeField] protected Slider ShiledSlider2;
        [Header("카드만")] 
        [SerializeField] protected Card_Info info;

        private Coroutine HpFunc;

        public virtual float HpMax()//최대체력
        {
            return 1;
        }
        public virtual float Mana()//마나
        {
            
            return 1;
        }
        public virtual float ManaMax()//최대마나
        {
            
            return 1;
        }

        public virtual float Atk_Cool()//공속
        {
            return 1;
        }
        public virtual float Range()//사정거리
        {

            return 1;
        }
        public virtual float NoAttack()//회피
        {

            return 1;
        }

        public virtual float Atk_Damage() //데미지
        {
            return 1;
        }
        public virtual float Magic_Damage()//데미지
        {
            return 1;
        }
        public virtual float Defence()//방어
        {
            return 1;
        }
        public virtual float Defence_Magic()//마법방어
        {
            return 1;
        }
        public virtual float Speed()//이동속도
        {
            return 1;
        }

        public virtual float CriPer()//크리
        {
            return 1;
        }
        public virtual float CriDmg()//크리데미지
        {
            return 1;
        }

        public virtual float AtkAniTime()
        {
            return attackTime;
        }


        public float currentHp
        {
            get { return _currentHp;}
            set
            {
                float hp= value;
                
                float hpmax = HpMax();
                if (hp<=0 )
                {
                    hp = 0;
                }
                if (hp>=hpmax )
                {
                    hp = hpmax;
                }
                if (hp==_currentHp)
                {
                    return;
                }
                
                

                pv.RPC(nameof(RPC_SetCurrentHp),RpcTarget.All,hp);
                
            }
        }

        public float currentMana
        {
            get { return _currentMana; }
            set
            {
                if (ManaMax()<=0)
                {
                    return;
                }
                
                float mana= value;
                float mpmax = ManaMax();
                if (mana<=0 )
                {
                    mana = 0;
                    
                }
                if (mana>=mpmax )
                {
                    mana = mpmax;
                    
                }
                if (mana==_currentMana)
                {
                    return;
                }
                

                pv.RPC(nameof(RPC_SetCurrentMana),RpcTarget.All,mana);
                
            }
        }

        public float shiled
        {
            get { return _shiled; }
            set
            {
                float shiled= value;
                if (shiled<=0 )
                {
                    shiled = 0;
                }
                if (shiled==_shiled)
                {
                    return;
                }

                pv.RPC(nameof(RPC_SetShiled),RpcTarget.All,shiled);
            }
        }
        
        public bool IsDead;
        public bool IsInvin;
        public bool IsStun;

        [PunRPC]
        protected void RPC_SetCurrentHp(float hp)
        {
            
            float hpmax = HpMax();
            float hper = hp / hpmax ;
            HpSlider.value = hper;
            if (_currentHp<=hp)
            {
                
                HpSliderCheck.value = hper;
                //체력채워짐
            }
            else
            {
                if (HpFunc==null)
                {
                    HpFunc=StartCoroutine(IHpSlider());
                    
                }
                else
                {
                    StopCoroutine(HpFunc);
                    HpFunc=StartCoroutine(IHpSlider());
                }
            }
            

            
            _currentHp = hp;
        }
        [PunRPC]
        protected void RPC_SetCurrentMana(float Mana)
        {
            float manamax = ManaMax();
            float mper = Mana / manamax ;
            ManaSlider.value = mper;
            _currentMana = Mana;
        }

        [PunRPC]
        protected void RPC_SetShiled(float value)
        {
            float hpmax = HpMax();
            float sper = value / hpmax ;
            float hpper = HpSlider.value;

            if (sper+hpper>1)
            {
                ShiledSlider1.value = 0;
                ShiledSlider2.value = sper;
                
            }
            else
            {
                ShiledSlider1.value = sper;
                ShiledSlider2.value = 0;
            }

            
            _shiled = value;
        }

        public void RangeSet()
        {
            float f = Range();
            Mathf.Clamp(f, 0, 10);
            collider.size = new Vector3(f, f, f);
        }

        IEnumerator IHpSlider(float time=0.8f)
        {
            float curTime = 0;
            while (curTime<time)
            {
                if (HpSliderCheck.value<=HpSlider.value)
                {
                    break;
                }
                curTime += Time.deltaTime;
                float f = Mathf.Lerp(HpSliderCheck.value, HpSlider.value, curTime / time);

                HpSliderCheck.value = f;
                
                yield return null;
            }

            HpFunc = null;
        }

        public void StunShow(bool active)
        {
            
            pv.RPC(nameof(RPC_StunShow),RpcTarget.All,active);
        }

        [PunRPC]
        protected void RPC_StunShow(bool active)
        {
            IsStun = active;
            StunOb.SetActive(active);
        }

        public void BasicFunc(GameObject target,bool NoAtk)
        {
            if (!IsCard) return;

            currentMana += 10;
            if (NoAtk)
            {
                return;
            }
            AttackCnt++;
            if (AttackOb==target)
            {
                OneAttackCnt++;
            }
            else
            {
                AttackOb = target;
                OneAttackCnt = 1;
            }
        }

        public bool CriCheck(bool IsPhy=true,bool IsMagic=false)
        {
            if (!IsCard)
            {
                return false;
            }
            float p = Random.Range(0.01f, 100f);
            bool IsCri = false;

            if (p <= CriPer())
            {

                if (IsPhy)
                {
                    IsCri = true;
                }

                if (IsMagic)
                {
                    //아이템낄경우
                }
                
            }

            return IsCri;
        }
        public bool NoAttackCheck(bool IsPhy=true)
        {
            if (!IsCard)
            {
                return false;
            }
            float p = Random.Range(0.01f, 100f);
            bool IsNo = false;

            if (p <= NoAttack())
            {
                if (IsPhy)
                {
                    IsNo = true;
                }
            }

            return IsNo;
        }

        public virtual void UnitKill()
        {
            
        }

        public virtual void UnitDead()
        {
            
        }

        public void UnitInvin(bool Invin)
        {
            pv.RPC(nameof(RPC_UnitInvin),RpcTarget.All,Invin);
        }

        [PunRPC]
        void RPC_UnitInvin(bool Invin)
        {
            IsInvin = Invin;
        }
        
    }
}
