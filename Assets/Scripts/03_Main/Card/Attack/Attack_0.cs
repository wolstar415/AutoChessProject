using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_0 : AttackFunc
    {

        public override void BasicAttack(GameObject target)
        {
            base.BasicAttack(target);
            fsm.NoConTime(stat.AtkAniTime(),false);
            fsm.CoolStart();
            AttackFunc();
        }

        void AttackFunc()
        {
            //총알생성
            GameObject bullet = PhotonNetwork.Instantiate("Bullet_0", CreatePos.position, Quaternion.identity);
            if (bullet.TryGetComponent(out Buulet_Move1 move))
            {
                move.StartFUnc(gameObject,Target,stat.Atk_Damage());
            }
        }


    }
}
