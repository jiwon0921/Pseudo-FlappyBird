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
        public ePFBLogLevel logLevel { get; private set; } = ePFBLogLevel.All;



        //----------------------------------
        // File ����
        //----------------------------------


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

            switch (msg.pfbLogType)
            {
                case ePFBLogType.Log:
                case ePFBLogType.Debug:
                    LogDebug(msg);
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
                    LogDebug(msg);
                    break;
            }
        }
        public void LogDebug(PFBLogMessage msg)
        {
            msg.pfbLogType = ePFBLogType.Debug;
            msg.messageString = $"<color=green>{GetLogTypeString(msg.pfbLogType)}</color> {msg.messageString}";
            if (IsAllowedLevel(msg.pfbLogType))
            {
                Debug.Log(GetLogTextForUnityConsole(msg));
                TrySaveLog(msg);
            }
        }
        public void LogInfo(PFBLogMessage msg)
        {
            msg.pfbLogType = ePFBLogType.Info;
            msg.messageString = $"<color=blue>{GetLogTypeString(msg.pfbLogType)}</color> {msg.messageString}";

            if (IsAllowedLevel(msg.pfbLogType))
            {
                Debug.Log(GetLogTextForUnityConsole(msg));
                TrySaveLog(msg);
            }
        }

        public void LogWarning(PFBLogMessage msg)
        {
            msg.pfbLogType = ePFBLogType.Warning;
            msg.messageString = $"<color=yellow>{GetLogTypeString(msg.pfbLogType)}</color> {msg.messageString}";

            if (IsAllowedLevel(msg.pfbLogType))
            {
                Debug.Log(GetLogTextForUnityConsole(msg));
                TrySaveLog(msg);
            }

        }
        public void LogError(PFBLogMessage msg)
        {
            msg.pfbLogType = ePFBLogType.Error;
            msg.messageString = $"<color=red>{GetLogTypeString(msg.pfbLogType)}</color> {msg.messageString}";

            if (IsAllowedLevel(msg.pfbLogType))
            {
                Debug.Log(GetLogTextForUnityConsole(msg));
                TrySaveLog(msg);
            }
        }

        private void TrySaveLog(PFBLogMessage msg)
        {

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


        /// <summary>
        /// �ش� �α� Ÿ���� ���� ���� �󿡼� ǥ�ð� ���Ǵ��� �����Ǵ���...
        /// </summary>
        /// <param name="logType"></param>
        private bool IsAllowedLevel(ePFBLogType logType)
        {
            return (int)logType >= (int)current.logLevel;
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

                current.logLevel = ePFBLogLevel.All;
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

        public static void SetCurrentLogLevel(ePFBLogLevel level)
        {
            current.logLevel = level;
        }
        public static void SetSaveMode(ePFBLogSaveMode saveMode)
        {
            current.logSaveMode = saveMode;
        }

        private string GetLogTypeString(ePFBLogType logType)
        {
            string logTypeString = string.Empty;

            switch (logType)
            {
                case ePFBLogType.Log:
                    logTypeString = "[LOG]";
                    break;

                case ePFBLogType.Debug:
                    logTypeString = "[DEBUG]";
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

            return logTypeString;
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
