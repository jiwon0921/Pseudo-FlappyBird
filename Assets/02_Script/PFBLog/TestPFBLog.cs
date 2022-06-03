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

        PFBLogHelper.current.SetSaveMode(ePFBLogSaveMode.OnRealTime);

        PFBLogHelper.current.SetDefaultLogType(ePFBLogType.Info);

        log.LogWarning("Warning?");
        log.Log("LogInfoType!", ePFBLogType.Default);
        PFBLogHelper.current.SetDefaultLogType(ePFBLogType.Log);
        log.Log("LogDefaultType!", ePFBLogType.Default);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
