using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PFB;

public class TestPFBLog : MonoBehaviour
{
    public PFBLog log = new PFBLog("Test");
    // Start is called before the first frame update
    void Start()
    {
        PFBLogHelper.GetSafeCurrent();

        PFBLogHelper.current.SetDefaultLogType(ePFBLogType.Warning);
        log.Log("Warning?");
        log.Log("LogInfoType!", ePFBLogType.Info);
        PFBLogHelper.current.SetDefaultLogType(ePFBLogType.Log);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
