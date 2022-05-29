using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIntro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator check()
    {
        yield return YieldInstructionCache.WaitForSeconds(1);
    }
}
