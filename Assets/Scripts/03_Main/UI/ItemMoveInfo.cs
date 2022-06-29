using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameS
{
    public class ItemMoveInfo : MonoBehaviour
    {
        public Image icon;
        

        private void Start()
        {
            transform.SetParent(GameSystem_AllInfo.inst.mainCanvas);
            transform.localScale = new Vector3(1, 1, 1);
            
        }

        public void GoGo(int idx)
        {
            int itemicon = CsvManager.inst.itemInfo[idx].Icon;
            icon.sprite = IconManager.inst.icon[itemicon];
            
            transform.DOMove(GameSystem_AllInfo.inst.itemMoveTrans.position, 0.9f);
            StartCoroutine(IitemGet(idx));
        }

        IEnumerator IitemGet(int idx)
        {
            yield return YieldInstructionCache.WaitForSeconds(1.1f);
            ItemManager.inst.ItemAdd(idx);
            gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {

            ObjectPooler.ReturnToPool(gameObject);
        }

    }
}
