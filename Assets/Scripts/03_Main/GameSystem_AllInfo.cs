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


        // LINQ �޼ҵ带 �̿��� ���� ����� ���� ã���ϴ�.
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

        // LINQ �޼ҵ带 �̿��� ���� ����� ���� ã���ϴ�.
        var neareastObject = Obs
            .OrderBy(obj =>
            {
                return Vector3.Distance(pos, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject.gameObject;
    }
}
