using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using PFB;

namespace PFB.Log
{
    public class PFBLogHelper : MonoBehaviour
    {

        #region ������ --


        public static PFBLogHelper current { get; private set; }

        public bool isCreatedGameObject;

        //----------------------------------
        // ���� ����
        //----------------------------------
        public ePFBLogSaveMode logSaveMode { get; private set; }
        public ePFBLogType logLevel { get; private set; } = ePFBLogType.Error;



        //----------------------------------
        // File ����
        //----------------------------------

        public string logTypeString { get; private set; }

        private DirectoryInfo localDirectoryInfo;
        private FileInfo localFileInfo;

        public string localFilePath { get; private set; } = Directory.GetCurrentDirectory() + @"\PFBLogs\PFBLog-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".log";
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

        public void LogType(PFBLogMessage msg)
        {
            if (msg.pfbLogType == ePFBLogType.Default)
            {
                msg.pfbLogType = current.logLevel;
            }

            switch (msg.pfbLogType)
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
        public void Log(PFBLogMessage msg)
        {
            msg.pfbLogType = ePFBLogType.Log;
            Debug.Log(GetLogTextForUnityConsole(msg));
            TrySaveLog(msg);
        }
        public void LogInfo(PFBLogMessage msg)
        {
            msg.pfbLogType = ePFBLogType.Info;
            msg.messageString = "<color=blue>" + msg.messageString + "</color>";

            Debug.Log(GetLogTextForUnityConsole(msg));
            TrySaveLog(msg);
        }

        public void LogWarning(PFBLogMessage msg)
        {
            msg.pfbLogType = ePFBLogType.Warning;
            Debug.LogWarning(GetLogTextForUnityConsole(msg));
            TrySaveLog(msg);

        }
        public void LogError(PFBLogMessage msg)
        {
            msg.pfbLogType = ePFBLogType.Error;
            msg.messageString = "<color=red>" + msg.messageString + "</color>";

            Debug.LogError(GetLogTextForUnityConsole(msg));
            TrySaveLog(msg);
        }

        private void TrySaveLog(PFBLogMessage msg)
        {

            if ((int)msg.pfbLogType > (int)current.logLevel)
            {
                return;
            }


            string str;
            switch (logSaveMode)
            {
                case ePFBLogSaveMode.DoNotSave:
                    break;

                case ePFBLogSaveMode.OnRealTime:

                    //msg = RemoveRichText(msg.);
                    str = GetLogTextForFile(msg);

                    logStringQueue.Enqueue(str);
                    SaveLogToTxtFile();
                    break;

                case ePFBLogSaveMode.OnExit:

                    str = GetLogTextForFile(msg);
                    logStringQueue.Enqueue(str);

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
                string localFilePath = Directory.GetCurrentDirectory() + @"\PFBLogs\PFBLog-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".log";
                string localDirectioryPath = Directory.GetCurrentDirectory() + @"\PFBLogs";

                current.SetLocalDirectoryAndFileInfo(localDirectioryPath, localFilePath);
                //ť �ʱ�ȭ
                current.logStringQueue = new Queue<string>();

                current.logLevel = ePFBLogType.Error;
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

        public static void SetCurrentLogLevel(ePFBLogType logType)
        {
            if (logType == ePFBLogType.Default)
            {
                logType = ePFBLogType.Log;
            }
            current.logLevel = logType;
            current.SetLogTypeString(logType);
        }
        public static void SetSaveMode(ePFBLogSaveMode saveMode)
        {
            current.logSaveMode = saveMode;
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
        private string GetLogTypeString(ePFBLogType logType)
        {
            string str;
            switch (logType)
            {
                case ePFBLogType.Default:
                case ePFBLogType.Log:
                    str = "[LOG]";
                    break;
                case ePFBLogType.Info:
                    str = "[INFO]";
                    break;
                case ePFBLogType.Warning:
                    str = "[WARN]";
                    break;
                case ePFBLogType.Error:
                    str = "[ERROR]";
                    break;
                default:
                    str = "[LOG]";
                    break;
            }

            return str;
        }


        public string GetLogTextForUnityConsole(PFBLogMessage msg)
        {
            return string.Format("{0}{1}", msg.titleRichString, msg.messageString);
        }
        public string GetLogTextForFile(PFBLogMessage msg)
        {
            msg.messageString = RemoveRichText(msg.messageString);
            return string.Format("{0} ({1}) {2}{3}", new string[4] { GetLogTypeString(msg.pfbLogType), msg.dateString, msg.titleString, msg.messageString });
        }
        public string GetDateTime()
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


        private void OnApplicationQuit()
        {
            if (current == null)
            {
                return;
            }
            if (current.logSaveMode == ePFBLogSaveMode.OnExit)
            {
                SaveLogToTxtFile();
            }

        }


    }

}
