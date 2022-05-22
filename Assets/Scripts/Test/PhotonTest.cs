using UnityEngine.UI;

using System;
using System.IO;
using System.Text;

using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine;
using TMPro;

public class MyCustomType
{
    public char rndCode { get; set; }
    public int hashCode { get; set; }


    // 직렬화
    public static byte[] Serialize(object customobject)
    {
        MyCustomType ct = (MyCustomType)customobject;

        // 스트림에 필요한 메모리 사이즈(Byte)
        MemoryStream ms = new MemoryStream(sizeof(char) + sizeof(int));
        // 각 변수들을 Byte 형식으로 변환, 마지막은 개별 사이즈
        ms.Write(BitConverter.GetBytes(ct.rndCode), 0, sizeof(char));
        ms.Write(BitConverter.GetBytes(ct.hashCode), 0, sizeof(int));

        // 만들어진 스트림을 배열 형식으로 반환
        return ms.ToArray();
    }

    // 역직렬화
    public static object Deserialize(byte[] bytes)
    {
        MyCustomType ct = new MyCustomType();
        // 바이트 배열을 필요한 만큼 자르고, 원하는 자료형으로 변환
        ct.rndCode = BitConverter.ToChar(bytes, 0);
        ct.hashCode = BitConverter.ToInt32(bytes, sizeof(char));
        return ct;
    }
}

public class PhotonTest : MonoBehaviour
{
    public TextMeshProUGUI msgText = null;
    public Button sendBtn = null;
    public PhotonView pv;


    private void Awake()
    {
        // 포톤 네트워크에 타입을 등록
        PhotonPeer.RegisterType(typeof(MyCustomType), 0, MyCustomType.Serialize, MyCustomType.Deserialize);

        sendBtn.onClick.AddListener(Send);
    }

    public void Send()
    {
        MyCustomType custom = new MyCustomType();
        custom.rndCode = (char)UnityEngine.Random.Range(0, 255);
        custom.hashCode = GetHashCode();
        pv.RPC("RPCCustomType", RpcTarget.All, custom);
    }

    [PunRPC]
    public void RPCCustomType(MyCustomType _custumType)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(_custumType.rndCode.ToString());
        sb.AppendLine(_custumType.hashCode.ToString());
        msgText.text = sb.ToString();
    }

}
