using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

using PFB;

public class PFBLogHelper : MonoBehaviour
{

    #region ������ --


    public static PFBLogHelper current { get; private set; }

    public bool isCreatedGameObject;

    //----------------------------------
    // ���� ����
    //----------------------------------
    public ePFBLogSaveMode logSaveMode { get; private set; }
    public ePFBLogType defaultLogType { get; private set; } = ePFBLogType.Log;



    //----------------------------------
    // File ����
    //----------------------------------

    public string logTypeString { get; private set; }

    private DirectoryInfo localDirectoryInfo;
    private FileInfo localFileInfo;

    public string localFilePath { get; private set; } = Directory.GetCurrentDirectory() + @"\PFBLogs\PFBLog-" + DateTime.Today.ToString("yyyy-MM-dd") + ".log";
    public string localDirectioryPath { get; private set; } = Directory.GetCurrentDirectory() + @"\PFBLogs";



    //----------------------------------
    // Log Data ���� 
    //----------------------------------

    private Queue<string> logStringQueue = null;


    #endregion

    private void Awake()
    {
        //if (!isCreatedGameObject)
        //{
        //    GetSafeCurrent();
        //}
    }
    private void OnDestroy()
    {
        current = null;
    }

    public void Log(object msg, ePFBLogType logType)
    {
        if (logType == ePFBLogType.Default)
        {
            logType = defaultLogType;
        }

        switch (logType)
        {
            case ePFBLogType.Default:
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
        TrySaveLog(msg.ToString());
    }
    public void LogInfo(object msg)
    {

        Debug.Log("<color=blue>" + msg + "</color>");
        TrySaveLog(msg.ToString());
    }

    public void LogWarning(object msg)
    {
        Debug.LogWarning(msg);
        TrySaveLog(msg.ToString());

    }
    public void LogError(object msg)
    {
        Debug.LogError(msg);
        TrySaveLog(msg.ToString());
    }

    private void TrySaveLog(string msg)
    {
        switch (logSaveMode)
        {
            case ePFBLogSaveMode.DoNotSave:
                break;

            case ePFBLogSaveMode.OnRealTime:

                msg = RemoveRichText(msg);
                Debug.Log(msg);
                logStringQueue.Enqueue(msg);
                SaveLogToTxtFile();
                break;

            default:
                break;
        }
    }

    public static PFBLogHelper GetSafeCurrent()
    {
        //������ ���� �����ؾ���
        if (current == null)
        {
            //���ӿ�����Ʈ ����
            GameObject helperObject = new GameObject("[--PFBLogHelper--]");
            current = helperObject.AddComponent<PFBLogHelper>();
            current.isCreatedGameObject = true;
            //���� ���� �۾�
            string localFilePath = Directory.GetCurrentDirectory() + @"\PFBLogs\PFBLog-" + DateTime.Today.ToString("yyyy-MM-dd") + ".log";
            string localDirectioryPath = Directory.GetCurrentDirectory() + @"\PFBLogs";

            current.SetLocalDirectoryAndFileInfo(localDirectioryPath, localFilePath);
            //ť �ʱ�ȭ
            current.logStringQueue = new Queue<string>();
            return current;
        }
        //������ ��...
        else
        {
            return current;
        }
    }

    public void SetLocalDirectoryAndFileInfo(string diPath, string fiPath)
    {
        localDirectoryInfo = new DirectoryInfo(diPath);
        localFileInfo = new FileInfo(fiPath);

        //���� ����
        if (!localDirectoryInfo.Exists)
        {
            Directory.CreateDirectory(diPath);
        }

        //���� ����
        if (!localFileInfo.Exists)
        {
            using (StreamWriter sw = new StreamWriter(fiPath))
            {
                string logText = string.Format("[SYSTEM] ({0}) {1}", GetDateTime(), "Create New Log File");
                sw.WriteLine(logText);
                sw.Close();
            }
        }
    }

    /// <summary>
    /// ���� �ؽ�Ʈ�� ���� ��ȯ�մϴ�.
    /// </summary>
    public static string RemoveRichText(string str)
    {
        Regex rich = new Regex(@"<[^>]*>");

        if (rich.IsMatch(str))
        {
            str = rich.Replace(str, string.Empty);
        }
        return str;
    }

    public void SetDefaultLogType(ePFBLogType logType)
    {
        if (logType == ePFBLogType.Default)
        {
            logType = ePFBLogType.Log;
        }
        defaultLogType = logType;
        SetLogTypeString(logType);
    }
    public void SetSaveMode(ePFBLogSaveMode saveMode)
    {
        logSaveMode = logSaveMode;
    }

    /// <summary>
    /// �α��� ���� ����� �����մϴ�.
    /// </summary>
    /// <param name="saveMode"></param>
    private void SetLogTypeString(ePFBLogType logType)
    {
        switch (logType)
        {
            case ePFBLogType.Default:
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


    private string GetDateTime()
    {
        DateTime NowDate = DateTime.Now;
        return NowDate.ToString("yyyy-MM-dd/HH:mm:ss") + ":" + NowDate.Millisecond.ToString("000");
    }

    /// <summary>
    /// �α� ������ ���Ϸ� ����մϴ�.
    /// </summary>
    public void SaveLogToTxtFile()
    {

        string logText;

        try
        {
            using (StreamWriter sw = File.AppendText(localFilePath))
            {

                while (logStringQueue.Count > 0)
                {
                    logText = logStringQueue.Dequeue();
                    sw.WriteLine(logText);
                }
                sw.Close();
            }

        }
        catch (Exception e)
        {
            print(string.Format("[�����߻�] - ���� : {0}", e.ToString()));
        }
    }

    private void CreateGameObject()
    {
        GameObject helperObject = new GameObject("[PFBLogHelper]");
        current = helperObject.AddComponent<PFBLogHelper>();
    }



}
