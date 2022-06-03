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

        PFBLogHelper.SetSaveMode(ePFBLogSaveMode.OnRealTime);

        PFBLogHelper.SetDefaultLogType(ePFBLogType.Info);

        log.LogWarning("Warning?");
        log.LogInfo("LogInfoType!");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
