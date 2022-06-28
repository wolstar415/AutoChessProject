using System;
using DG.Tweening;
using UnityEngine;
public class Test3 : MonoBehaviour
{

    public GameObject move1;
    public Transform check;

    public Vector3 asd;

    public void Start()
    {
        //move1.transform.position = Camera.main.ScreenToWorldPoint(transform.position);
        move1.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        //asd = Camera.main.WorldToScreenPoint(check.position);
        move1.transform.DOMove(check.position, 1);
    }

    private void Update()
    {
        
    }
}
