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


    // ����ȭ
    public static byte[] Serialize(object customobject)
    {
        MyCustomType ct = (MyCustomType)customobject;

        // ��Ʈ���� �ʿ��� �޸� ������(Byte)
        MemoryStream ms = new MemoryStream(sizeof(char) + sizeof(int));
        // �� �������� Byte �������� ��ȯ, �������� ���� ������
        ms.Write(BitConverter.GetBytes(ct.rndCode), 0, sizeof(char));
        ms.Write(BitConverter.GetBytes(ct.hashCode), 0, sizeof(int));

        // ������� ��Ʈ���� �迭 �������� ��ȯ
        return ms.ToArray();
    }

    // ������ȭ
    public static object Deserialize(byte[] bytes)
    {
        MyCustomType ct = new MyCustomType();
        // ����Ʈ �迭�� �ʿ��� ��ŭ �ڸ���, ���ϴ� �ڷ������� ��ȯ
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
        // ���� ��Ʈ��ũ�� Ÿ���� ���
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
