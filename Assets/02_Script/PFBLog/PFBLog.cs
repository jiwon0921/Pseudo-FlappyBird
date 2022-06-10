using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;


namespace PFB.Log
{
    public enum ePFBLogType
    {
        Log = 0,
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
    }

    public enum ePFBLogLevel
    {
        All = -1,
        Log = 0,
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
        Off = 5,
    }

    public class PFBLogMessage
    {
        public ePFBLogType pfbLogType;
        public string dateString;
        public string titleRichString;
        public string titleString;
        public string messageString;
        public PFBLogMessage(ePFBLogType logType, string date, string titleRichText, string title, string msg)
        {
            pfbLogType = logType;
            dateString = date;
            titleString = title;
            titleRichString = titleRichText;
            messageString = msg;
        }
        public PFBLogMessage(string date, string titleRichText, string title, string msg)
        {
            dateString = date;
            titleString = title;
            titleRichString = titleRichText;
            messageString = msg;
        }

        public PFBLogMessage(string date, string title, string msg)
        {
            pfbLogType = ePFBLogType.Log;
            dateString = date;
            titleString = title;
            titleRichString = title;
            messageString = msg;
        }
    }

    public enum ePFBLogSaveMode
    {
        [Tooltip("로그를 따로 저장하지 않습니다.")]
        DoNotSave,

        [Tooltip("로그를 발송할 때마다 저장합니다.")]
        OnRealTime,

        [Tooltip("에디터와 게임의 동작이 다릅니다. \n[Editor]PlayMode가 종료될 때 한꺼번에 저장합니다.\n[Game]프로그램이 종료될 때 한꺼번에 저장합니다.")]
        OnExit,
    }

    /// <summary>
    /// 로그용 클래스 입니다
    /// </summary>
    public class PFBLog
    {
        private PFBLogMessage message;
        public PFBLog(string name, Color32 color = default, bool isBoldTitle = true)
        {
            string str = "[" + name + "] ";

            str = "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + str + "</color>";

            if (isBoldTitle)
            {
                str = "<b>" + str + "</b>";
            }

            objectTitleRichText = str;
            objectTitle = PFBLogHelper.RemoveRichText(objectTitleRichText);
            message = new PFBLogMessage(string.Empty, objectTitleRichText, objectTitle, "Create!");
        }

        /// <summary>
        /// 서식이 포함된 타이틀 텍스트입니다.
        /// </summary>
        public string objectTitleRichText { get; private set; }

        /// <summary>
        /// 서식이 포함되지 않은 타이틀 텍스트입니다.
        /// </summary>
        public string objectTitle { get; private set; }


        public void Log(object msg, ePFBLogType logType)
        {
            message.dateString = PFBLogHelper.current.GetDateTime();
            message.messageString = msg.ToString();
            PFBLogHelper.current.LogType(message);
        }

        public void LogDebug(object msg)
        {
            message.dateString = PFBLogHelper.current.GetDateTime();
            message.messageString = msg.ToString();
            PFBLogHelper.current.LogDebug(message);
        }

        public void LogInfo(object msg)
        {
            message.dateString = PFBLogHelper.current.GetDateTime();
            message.messageString = msg.ToString();
            PFBLogHelper.current.LogInfo(message);
        }

        public void LogWarning(object msg)
        {
            message.dateString = PFBLogHelper.current.GetDateTime();
            message.messageString = msg.ToString();
            PFBLogHelper.current.LogWarning(message);
        }

        public void LogError(object msg)
        {
            message.dateString = PFBLogHelper.current.GetDateTime();
            message.messageString = msg.ToString();
            PFBLogHelper.current.LogError(message);
        }

    }

}