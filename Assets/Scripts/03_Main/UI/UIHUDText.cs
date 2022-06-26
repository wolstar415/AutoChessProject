using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameS
{
    public class UIHUDText : MonoBehaviour
    {
        [SerializeField] float moveDistance = 100;
        [SerializeField] float moveTime = 1f;

        [SerializeField] RectTransform rectTransform;
        [SerializeField] TextMeshProUGUI textHUD;

        private void Start()
        {
            transform.SetParent(GameSystem_AllInfo.inst.TextUIparent);
        }

        public void Play(string text, Color color, Bounds bounds, float gap = 0.1f)
        {
            textHUD.text = text;
            textHUD.color = color;
            StartCoroutine(OnHUDText(bounds, gap));
        }

        public void Play2(string text, Color color, Vector3 pos,float Size=1.0f, float gap = 0.1f)
        {
            textHUD.text = text;
            textHUD.color = color;
            rectTransform.localScale = new Vector3(Size, Size, Size);
            StartCoroutine(OnHUDText2(pos+new Vector3(0,Random.Range(0,3f),0), gap));
        }

        IEnumerator OnHUDText2(Vector3 pos, float gap)
        {
            Vector2 start = Camera.main.WorldToScreenPoint(new Vector3(pos.x, pos.y + gap, pos.z));
            Vector2 end = start + Vector2.up * moveDistance;

            float current = 0f;
            float percent = 0f;

            while (percent < 1)

            {
                current += Time.deltaTime;
                percent = current / moveTime;

                rectTransform.position = Vector2.Lerp(start, end, percent);
                Color color = textHUD.color;
                color.a = Mathf.Lerp(1, 0, percent);
                textHUD.color = color;

                yield return null;



            }

            gameObject.SetActive(false);
        }

        IEnumerator OnHUDText(Bounds bounds, float gap)
        {
            Vector2 start =
                Camera.main.WorldToScreenPoint(new Vector3(bounds.center.x, bounds.max.y + gap, bounds.center.z));
            Vector2 end = start + Vector2.up * moveDistance;

            float current = 0f;
            float percent = 0f;

            while (percent < 1)

            {
                current += Time.deltaTime;
                percent = current / moveTime;

                rectTransform.position = Vector2.Lerp(start, end, percent);
                Color color = textHUD.color;
                color.a = Mathf.Lerp(1, 0, percent);
                textHUD.color = color;

                yield return null;



            }

            gameObject.SetActive(false);

        }

        private void OnDisable()
        {

            ObjectPooler.ReturnToPool(gameObject);
        }

    }
}