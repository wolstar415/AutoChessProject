using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSystem_AllInfo : MonoBehaviour
{
    public static GameSystem_AllInfo inst;
    public Transform[] StartPos;
    public Transform[] CameraPos_At;
    public Transform[] CameraPos_De;
    public Transform PickPos;
    public LayerMask[] masks;

    private void Awake()
    {
        inst = this;
    }

    public GameObject FindNearestObject(Vector3 pos,GameObject[] Obs)
    {


        // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
        var neareastObject = Obs
            .OrderBy(obj =>
            {
                return Vector3.Distance(pos, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject;
    }
    public GameObject FindNearestObject(Vector3 pos, Collider[] Obs)
    {

        // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
        var neareastObject = Obs
            .OrderBy(obj =>
            {
                return Vector3.Distance(pos, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject.gameObject;
    }
}
