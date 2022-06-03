using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

using PFB;

public class PFBLogHelper : MonoBehaviour
{
    public static PFBLogHelper current { get; private set; }

    private bool isCreatedGameObject;
    public ePFBLogSaveMode logSaveMode { get; private set; }
    public ePFBLogType defaultLogType { get; private set; } = ePFBLogType.Log;

    public string logTypeString { get; private set; }

    public void Log(object msg, ePFBLogType logType)
    {
        if (logType == ePFBLogType.FollowLogHelperCurrent)
        {
            logType = defaultLogType;
        }

        switch (logType)
        {
            case ePFBLogType.FollowLogHelperCurrent:
            case ePFBLogType.Log:
                Log(msg);
                break;

            case ePFBLogType.Info:
                LogInfo(msg);
                break;

            case ePFBLogType.Warning:
                LogWarning(msg);
                break;

            case ePFBLogType.Error:
                LogError(msg);
                break;
            default:
                Log(msg);
                break;
        }
    }
    public void Log(object msg)
    {
        Debug.Log(msg);
    }
    public void LogInfo(object msg)
    {
        Debug.Log("<color=blue>" + msg + "</color>");
    }

    public void LogWarning(object msg)
    {
        Debug.LogWarning(msg);

    }
    public void LogError(object msg)
    {

        Debug.LogError(msg);
    }
    private void Awake()
    {
        //Init_Instance();
    }


    private void TrySaveLog()
    {

    }

    public static PFBLogHelper GetSafeCurrent()
    {
        //������ ���� �����ؾ���
        if (current == null)
        {
            GameObject helperObject = new GameObject("[PFBLogHelper]");
            current = helperObject.AddComponent<PFBLogHelper>();

            return current;
        }
        //������ �ڻ�
        else
        {
            return current;
        }
    }


    /// <summary>
    /// �α��� ���� ����� �����մϴ�.
    /// </summary>
    /// <param name="saveMode"></param>
    public void SetSaveMode(ePFBLogSaveMode saveMode)
    {
        current.logSaveMode = logSaveMode;
    }

    public void SetDefaultLogType(ePFBLogType logType)
    {
        if (logType == ePFBLogType.FollowLogHelperCurrent)
        {
            logType = ePFBLogType.Log;
        }
        current.defaultLogType = logType;
        SetLogTypeString(logType);
    }

    private void Init_Instance()
    {
        //������ ���� �����ؾ���
        if (current == null)
        {
            CreateGameObject();
        }
        //������ �ڻ�
        else
        {
            if (current != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// �α� ������ ���Ϸ� ����մϴ�.
    /// </summary>
    public void SaveLogToTxtFile(object msg)
    {
        string FilePath = Directory.GetCurrentDirectory() + @"\Logs\CarsLog-CarLogTester-" + DateTime.Today.ToString("yyyyMMdd") + ".log";
        string DirPath = Directory.GetCurrentDirectory() + @"\Logs";

        string logText;

        DirectoryInfo di = new DirectoryInfo(DirPath);
        FileInfo fi = new FileInfo(FilePath);
        try
        {
            if (!di.Exists) Directory.CreateDirectory(DirPath);

            if (!fi.Exists)
            {
                using (StreamWriter sw = new StreamWriter(FilePath))
                {
                    logText = string.Format("[{0}] {1}", GetDateTime(), msg);
                    sw.WriteLine(logText);
                    sw.Close();

                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(FilePath))
                {
                    logText = string.Format("[{0}] {1}", GetDateTime(), msg);
                    sw.WriteLine(logText);
                    sw.Close();
                }
            }
        }
        catch (Exception e)
        {

        }
    }

    private string GetDateTime()
    {
        DateTime NowDate = DateTime.Now;
        return NowDate.ToString("yyyy-MM-dd/HH:mm:ss") + ":" + NowDate.Millisecond.ToString("000");
    }

    private void SetLogTypeString(ePFBLogType logType)
    {
        switch (logType)
        {
            case ePFBLogType.FollowLogHelperCurrent:
            case ePFBLogType.Log:
                logTypeString = "[LOG]";
                break;
            case ePFBLogType.Info:
                logTypeString = "[INFO]";
                break;
            case ePFBLogType.Warning:
                logTypeString = "[WARN]";
                break;
            case ePFBLogType.Error:
                logTypeString = "[ERROR]";
                break;
            default:
                logTypeString = "[LOG]";
                break;
        }
    }
    private void CreateGameObject()
    {
        GameObject helperObject = new GameObject("[PFBLogHelper]");
        current = helperObject.AddComponent<PFBLogHelper>();
    }



}
