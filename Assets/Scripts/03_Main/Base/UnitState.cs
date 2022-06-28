using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameS
{
    public class UnitState : MonoBehaviourPunCallbacks
    {
        //public float attackTime=1f;
        public PhotonView pv;
        public SphereCollider collider;
        public NavMeshAgent nav;
        public Animator ani;
        public Animation gani;
        public bool IsCard = true;
        public bool monster = false;
        public bool IsUnit = false;
        [SerializeField]protected GameObject StunOb; // 스턴오브젝트
        [SerializeField] protected float _currentHp; //현재체력

        [SerializeField]protected float _shiled; // 쉴드
        [SerializeField]protected float _currentMana; // 마나
        //[SerializeField]protected float _range; // 사거리
         // 최대마나

        [SerializeField] protected int KillCnt; //라운드 킬카운트
        [SerializeField] public int AttackCnt { get; protected set; } //평타횟수
        
        [SerializeField] protected GameObject AttackOb; //평타대상
        [SerializeField] public int OneAttackCnt{ get; protected set; } //같은대상 평타횟수

        [Header("UI")] 
        [SerializeField] protected Slider HpSlider;
        [SerializeField] protected Slider HpSliderCheck;
        [SerializeField] protected Slider ManaSlider;
        [SerializeField] protected Slider ShiledSlider1;
        [SerializeField] protected Slider ShiledSlider2;
        [SerializeField] protected Color[] colors;
        [SerializeField] protected Image[] fills;
        [Header("카드만")] 
        public Card_Info info;

        protected Coroutine HpFunc;
        public bool NoDmg = false;
        public bool IsCopy = false;
        
        [Header("딜체크")] 
        public int DmgIdx = -1;
        public float PhyDmg;
        public float MagicDmg;
        public float TrueDmg;
        [Header("체렷칸")]  public GameObject HpLine;

        [Header("아이템확인")] 
        public bool IsItemFunc11=false;
        public bool IsItemFunc12=false;
        public int IsItemFunc46=0;//본인만있어도함
        public int IsItemFunc19 { get; protected set; }
        public int IsItemFunc22 { get; protected set; }
        public int IsItemFunc24 { get; protected set; }
        public bool IsItemFunc26 { get; protected set; } = false;
        public int IsItemFunc37 { get; protected set; }

        public bool IsItemFunc49= false;
        public bool IsItemFunc36= false;
        
        [SerializeField] protected int IsItemFunc29;
        public item29_Scan ItemFunc29Scan;
        protected Coroutine CorDotDamage=null;
        public bool NoHeal;
        [Header("특성계열 확인")] 
        
        public bool Job3=false;
        public bool Job4=false;
        public int Job7;
        public bool Job9;
        public bool Job29;
        public int Job10;
        public int Job27;
        public bool Job31;
        public int Job31_2;

        public void ColorChange()
        {


            fills[0].color = colors[0];
            fills[1].color = colors[1];

        }

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

        // public virtual float AtkAniTime()
        // {
        //     return attackTime;
        // }


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
                if (IsNomana||BuffNomana>0) return;
                
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
        public int IsInvin;
        public int BuffNomana=0;
        public int AttackFailed=0;
        public bool IsStun;
        public bool IsNomana=false;

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
            return;
            float f = Range();
            f=Mathf.Clamp(f, 0, 10);
            collider.radius =f;
        }

        protected IEnumerator IHpSlider(float time=0.8f)
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
            float mph = 10f;
            if (info.IsHaveJob(28)) mph = 20;
            MpHeal((info.IsItemHave(14)*8)+mph);
            if (NoAtk)
            {
                if (target.GetComponent<Card_Info>().IsItemHave(31)>0)
                {
                    NetStopFunc(true,1f,false);
                }
                return;
            }

            if (Job31)
            {
                Job31_2++;
            }
            if (PlayerInfo.Inst.TraitandJobCnt[24]>=3&&info.IsHaveJob(24))
            {
                int per = 30;
                if (PlayerInfo.Inst.TraitandJobCnt[24] >= 9) per = 90;
                else if (PlayerInfo.Inst.TraitandJobCnt[24] >= 6) per = 60;
                
                if (Random.Range(0,100f)<=per) NetStopFunc(true,1,false);
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
                    if (info.IsItemHave(42)>0)
                    {
                        IsCri = true;
                    }
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

        public void UnitInvin(int Invin)
        {
            pv.RPC(nameof(RPC_UnitInvin),RpcTarget.All,Invin);
        }

        [PunRPC]
        protected void RPC_UnitInvin(int Invin)
        {
            IsInvin += Invin;
        }

        public void HpSize()
        {
            //float k = 5 * 400;
            float f = 2000 / HpMax();
            //float f = (2000 / currentHp) / (HpMax() / currentHp);
            var a = HpLine.GetComponent<HorizontalLayoutGroup>();
            // if (f>=5)
            // {
            // }

                a.padding.left = (int)(f*-14);
            a.gameObject.SetActive(false);
            foreach (Transform child in HpLine.transform)
            {
                child.gameObject.transform.localScale = new Vector3(f, 1, 1);
            }
            a.gameObject.SetActive(true);
        }

        public void InvinSet(float time)
        {
            if (time <= 0) return;
            
            pv.RPC(nameof(RPC_InvinSet),RpcTarget.All,time);
        }

        [PunRPC]
        protected void RPC_InvinSet(float time)
        {
            StartCoroutine(IInvinSet(time));
        }
        
        protected IEnumerator IInvinSet(float time)
        {
            IsInvin++;
            yield return YieldInstructionCache.WaitForSeconds(time);
            IsInvin--;

        }

        public void HpHeal(float v)
        {
            if (v <= 0||NoHeal) return;
            currentHp += v;
        }

        public void MpHeal(float v)
        {
            if (IsNomana||BuffNomana>0) return;
            if (v <= 0) return;
            currentMana += v;
        }

        public void ItemFuncAdd(int idx,bool timeb=false,float time=0f,bool minus=false)
        {
            pv.RPC(nameof(RPC_ItemFuncAdd),RpcTarget.All,idx,timeb,time,minus);
        }

        [PunRPC]
        protected void RPC_ItemFuncAdd(int idx,bool timeb,float time,bool minus)
        {
            if (timeb && time > 0)
            {

                StartCoroutine(IitemFuncAdd(idx, true, time));

            }
            else
            {

                if (minus)
                {
                    if(idx==19) IsItemFunc19--;
                    else if(idx==22) IsItemFunc22--;
                    else if(idx==24) IsItemFunc24--;
                    else if(idx==26) IsItemFunc26=false;
                    else if (idx == 30) NoHeal=false;
                }
                else
                {
                    
            if(idx==19) IsItemFunc19++;
            else if(idx==22) IsItemFunc22++;
            else if(idx==24) IsItemFunc24++;
            else if(idx==26) IsItemFunc26=true;
            else if (idx == 30) NoHeal=true;
                }
            }
        }

        protected IEnumerator IitemFuncAdd(int idx, bool timeb, float time)
        {
            if(idx==19) IsItemFunc19++;
            else if(idx==22) IsItemFunc22++;
            else if(idx==24) IsItemFunc24++;
            else if(idx==26) IsItemFunc26=true;
            yield return YieldInstructionCache.WaitForSeconds(time);
            if(idx==19) IsItemFunc19--;
            else if(idx==22) IsItemFunc22--;
            else if(idx==24) IsItemFunc24--;
            else if(idx==26) IsItemFunc26=false;
        }

        public void NetStopFunc(bool stun,float t,bool b)
        {
            if (monster)
            {

                StopFunc(stun, t,b);
            }
            else
            {
                pv.RPC(nameof(RPC_StopFunc), RpcTarget.All, stun, t, info.TeamIdx,b);
            }

        }

        [PunRPC]
        protected void RPC_StopFunc(bool stun,float t,int pidx,bool b)
        {
            if (PlayerInfo.Inst.PlayerIdx != pidx) return;

            StopFunc(stun, t, b);

        }

        protected void StopFunc(bool stun,float t,bool b)
        {
            if (stun&&IsItemFunc37>0)
            {
                Isitem37Check(1, false);
                return;
            }
            if (stun) StunShow(stun);
                
            info.fsm.NoConTime(t,b);
        }

        public void Isitem37Check(int i, bool b)
        {
            pv.RPC(nameof(RPC_IsItemFunc37),RpcTarget.All,i,b);
        }

        [PunRPC]
        protected void RPC_IsItemFunc37(int i, bool b)
        {
            if (b)
            {
                IsItemFunc37+=i;
                if (IsItemFunc37>0)
                {
                    //이펙트추가
                }
            }
            else
            {
                IsItemFunc37-=i;

            if (IsItemFunc37==0)
            {
                //이펙트제거
            }
            }

             

        }

        public void DotDamageGo(GameObject card,GameObject target)
        {
            if (CorDotDamage==null)
            {
                CorDotDamage = StartCoroutine(DotDamgeFunc(card, target));
            }
            else
            {
                
                StopCoroutine(CorDotDamage);
                CorDotDamage = StartCoroutine(DotDamgeFunc(card, target));
                
            }
        }
        
        protected IEnumerator DotDamgeFunc(GameObject card,GameObject target)
        {
            var card_stat = card.GetComponent<CardState>();
            var target_stat = target.GetComponent<CardState>();
            int cnt = 0;
            target_stat.ItemFuncAdd(30,false,0,false);
            yield return YieldInstructionCache.WaitForSeconds(1);
            while (cnt<=10)
            {
                if (!PlayerInfo.Inst.IsBattle||PlayerInfo.Inst.BattleEnd||card_stat.IsDead||target_stat.IsDead||!target_stat.NoHeal)
                {
                    yield break;
                }

                cnt++;
                float da = HpMax() * 0.02f;
                DamageManager.inst.DamageFunc1(card,target,da,eDamageType.True);
                yield return YieldInstructionCache.WaitForSeconds(1);
            }
            
            target_stat.ItemFuncAdd(30,false,0,true);
            CorDotDamage = null;
        }

        public void Item34Func(int targetnum)
        {
            float da = 125 * info.IsItemHave(34);
            pv.RPC(nameof(targetnum),RpcTarget.All,da,info.TeamIdx);
        }

        [PunRPC]
        protected void Item34Func(int targetnum,float d,int pidx)
        {
            if (PlayerInfo.Inst.PlayerIdx != pidx) return;
            
            DamageManager.inst.DamageFunc1(gameObject,PhotonView.Find(targetnum).gameObject,d,eDamageType.Speel_Magic);
        }

        public void AniStart(string name)
        {
            pv.RPC(nameof(RPC_AniStart),RpcTarget.All,name);
        }

        [PunRPC]
        protected void RPC_AniStart(string name)
        {
            if (gameObject.activeSelf)
            {

                if (info.IsAni)
                {
                    gani.Play(name);
                }
                else
                {
                    ani.Play(name);
                }
            }


        }

        public void Jump(Vector3 pos,float power,int cnt,float dur)
        {
            pv.RPC(nameof(RPC_Jump),RpcTarget.All,pos,power,cnt,dur);
        }

        [PunRPC]
        protected void RPC_Jump(Vector3 pos,float power,int cnt,float dur)
        {
            if (pv.IsMine)
            {
                transform.DOJump(pos,power,cnt,dur);
            }
        }

        public void Job3Func()
        {
            pv.RPC(nameof(RPC_Job3Func),RpcTarget.All);
            //이펙트
        }

        [PunRPC]
        protected void RPC_Job3Func()
        {
            Job3 = true;
        }
        public void Job4Func()
        {
            Job4 = true;
            //이펙트
        }
        public void Job29Func()
        {
            Job29 = true;
            //이펙트
        }
        public void Job7Func()
        {
            int cnt = PlayerInfo.Inst.TraitandJobCnt[7];
            pv.RPC(nameof(RPC_Job7),RpcTarget.All,cnt);
        }

        public void Job7Func2()
        {
            int cnt = Job7;
            Job7 = 0;
            StartCoroutine(IJob7Func(cnt));
        }

        protected IEnumerator IJob7Func(int lv)
        {
            int cnt = 66;
            float va = ((currentHp / HpMax()) * 0.3f) / cnt;
            if (lv >= 6) va *= 3;
            else if (lv >= 4) va *= 2;
            while (cnt>=0)
            {

                HpHeal(va);
                yield return null;
            }
        }
        
        public void Job9Func()
        {
            Job9 = false;
            pv.RPC(nameof(RPC_Job9),RpcTarget.All);
        }

        public void Job9Func2()
        {
            StartCoroutine(IJob9Func());
        }

        protected IEnumerator IJob9Func()
        {
            UnitInvin(1);
            yield return YieldInstructionCache.WaitForSeconds(2);
            UnitInvin(-1);
        }

        [PunRPC]
        protected void RPC_Job7(int cnt)
        {
            Job7 = cnt;
        }
        
        [PunRPC]
        protected void RPC_Job9()
        {
            Job9 = false;
        }
        
        public void Job10Func()
        {
            int cnt = PlayerInfo.Inst.TraitandJobCnt[10];
            pv.RPC(nameof(RPC_Job10),RpcTarget.All,cnt);
        }
        
        [PunRPC]
        protected void RPC_Job10(int cnt)
        {
            Job10 = cnt;
        }

        public void NoAttack(bool Plus, float t)
        {
            pv.RPC(nameof(RPC_NoAttack),RpcTarget.All,Plus,t);
        }

        [PunRPC]
        protected void RPC_NoAttack(bool Plus ,float t)
        {
            if (t <= 0)
            {
                if (Plus) AttackFailed ++;
                else AttackFailed --;
                return;
            }
            else
            {
                StartCoroutine(IRPC_NoAttack(Plus, t));
            }
        }

        protected IEnumerator IRPC_NoAttack(bool Plus, float t)
        {
            if (Plus) AttackFailed ++;
            else AttackFailed --;
            yield return YieldInstructionCache.WaitForSeconds(t);
            if (IsDead)
            {
                yield break;
            }
            
            if (Plus) AttackFailed --;
            else AttackFailed ++;
        }


        public void Knockback(Vector3 dis,int power)
        {
            if (IsDead) return;
            pv.RPC(nameof(RPC_Knockback),RpcTarget.All,dis,power);
        }
        [PunRPC]
        protected void RPC_Knockback(Vector3 dis,int power)
        {
            if (pv.IsMine)
            {
                if (IsDead) return;
                StartCoroutine(IKnockback(dis, power));
            }
        }

        protected IEnumerator IKnockback(Vector3 dis,int power)
        {
            int cnt = power;
            while (cnt>0)
            {
                if(IsDead) yield break;
                cnt--;


                //transform.Translate(dis*Time.deltaTime*10);
                transform.position = transform.position + (dis * Time.deltaTime*30);
                yield return null;
            }
            
        }

    }
}
