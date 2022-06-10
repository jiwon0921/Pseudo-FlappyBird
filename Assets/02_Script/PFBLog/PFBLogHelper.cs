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

        #region 변수들 --


        public static PFBLogHelper current { get; private set; }

        public bool isCreatedGameObject;

        //----------------------------------
        // 설정 관련
        //----------------------------------
        public ePFBLogSaveMode logSaveMode { get; private set; }
        public ePFBLogType logLevel { get; private set; } = ePFBLogType.Error;



        //----------------------------------
        // File 관련
        //----------------------------------

        public string logTypeString { get; private set; }

        private DirectoryInfo localDirectoryInfo;
        private FileInfo localFileInfo;

        public string localFilePath { get; private set; } = Directory.GetCurrentDirectory() + @"\PFBLogs\PFBLog-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".log";
        public string localDirectioryPath { get; private set; } = Directory.GetCurrentDirectory() + @"\PFBLogs";



        //----------------------------------
        // Log Data 관련 
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
            //없으면 새로 생성해야함
            if (current == null)
            {
                //게임오브젝트 생성
                GameObject helperObject = new GameObject("[--PFBLogHelper--]");
                current = helperObject.AddComponent<PFBLogHelper>();
                current.isCreatedGameObject = true;
                //파일 관련 작업
                string localFilePath = Directory.GetCurrentDirectory() + @"\PFBLogs\PFBLog-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".log";
                string localDirectioryPath = Directory.GetCurrentDirectory() + @"\PFBLogs";

                current.SetLocalDirectoryAndFileInfo(localDirectioryPath, localFilePath);
                //큐 초기화
                current.logStringQueue = new Queue<string>();

                current.logLevel = ePFBLogType.Error;
                return current;
            }
            //있으면 뭐...
            else
            {
                return current;
            }
        }

        public void SetLocalDirectoryAndFileInfo(string diPath, string fiPath)
        {
            localDirectoryInfo = new DirectoryInfo(diPath);
            localFileInfo = new FileInfo(fiPath);

            //폴더 생성
            if (!localDirectoryInfo.Exists)
            {
                Directory.CreateDirectory(diPath);
            }

            //파일 생성
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
        /// 서식 텍스트를 빼서 반환합니다.
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
        /// 로그의 저장 방식을 설정합니다.
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
        /// 로그 내용을 파일로 기록합니다.
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
                print(string.Format("[오류발생] - 사유 : {0}", e.ToString()));
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
