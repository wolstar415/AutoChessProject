using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    private static Singleton inst = null;

    void Awake()
    {
        if (null == inst)
        {
            //싱글톤
            inst = this;


            DontDestroyOnLoad(this.gameObject);
            //씬이 넘어가도 삭제 안되게 설정
        }
        else
        {

            Destroy(this.gameObject);
            //중복 방지용
        }
    }
    
}
public static class YieldInstructionCache
{
    class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals (float x, float y)
        {
            return x == y;
        }
        int IEqualityComparer<float>.GetHashCode (float obj)
        {
            return obj.GetHashCode();
        }
    }

    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    private static readonly Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(new FloatComparer());

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if (!_timeInterval.TryGetValue(seconds, out wfs))
            _timeInterval.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }
}