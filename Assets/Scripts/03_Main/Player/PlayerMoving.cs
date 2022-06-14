using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerMoving : MonoBehaviourPunCallbacks
{
    [SerializeField] private PhotonView pv;
    public NavMeshAgent nav;
    [SerializeField]Animator ani;
    bool IsMoving;
    public TextMeshProUGUI nickNameText;
    public TextMeshProUGUI lvText;
    public Slider hpSlider;


    public void check1()
    {
        
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                nav.ResetPath();
                ani.SetBool("Run", true);
                IsMoving = true;
                nav.isStopped = false;
                nav.updatePosition = true;
                nav.updateRotation = true;
                nav.SetDestination(hit.point);
            }
        
    }
    public void movePos(Vector3 pos)
    {
        nav.ResetPath();
        ani.SetBool("Run", true);
        IsMoving = true;
        
        nav.isStopped = false;
        nav.updatePosition = true;
        nav.updateRotation = true;
        nav.SetDestination(pos);
    }
    void Update()
    {
        if (!pv.IsMine)
        {
            return;
        }

        if (IsMoving && (nav.velocity.sqrMagnitude >= 0.2f * 0.2f && nav.remainingDistance <= 0.5f))
           
        // remainingDistance ������ ���������� ���� �Ÿ��� ��ȯ.
        // velocity.sqrMagnitude �ӵ�.
        {
            
            IsMoving = false;
            nav.isStopped = true;
            nav.updatePosition = false;
            nav.updateRotation = false;
            nav.velocity=Vector3.zero;
            ani.SetBool("Run", false);
            nav.ResetPath();
        }

        
        

        
    }

    public void NickNameSetting(string name)
    {
        pv.RPC(nameof(RPC_NickNameSetting),RpcTarget.All,name);
    }

    [PunRPC]
    void RPC_NickNameSetting(string name)
    {
        nickNameText.text = name;
    }
    public void LevelSet(int lv)
    {
        pv.RPC(nameof(RPC_LevelSet),RpcTarget.All,lv);
    }

    [PunRPC]
    void RPC_LevelSet(int lv)
    {
        lvText.text = lv.ToString();
    }
    public void HpSetting(int hp)
    {
        pv.RPC(nameof(RPC_HpSetting),RpcTarget.All,hp);
    }

    [PunRPC]
    void RPC_HpSetting(int hp)
    {
        hpSlider.value = hp;
    }
}
