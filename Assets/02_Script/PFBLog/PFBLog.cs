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
        [Tooltip("�α׸� ���� �������� �ʽ��ϴ�.")]
        DoNotSave,

        [Tooltip("�α׸� �߼��� ������ �����մϴ�.")]
        OnRealTime,

        [Tooltip("�����Ϳ� ������ ������ �ٸ��ϴ�. \n[Editor]PlayMode�� ����� �� �Ѳ����� �����մϴ�.\n[Game]���α׷��� ����� �� �Ѳ����� �����մϴ�.")]
        OnExit,
    }

    /// <summary>
    /// �α׿� Ŭ���� �Դϴ�
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
        /// ������ ���Ե� Ÿ��Ʋ �ؽ�Ʈ�Դϴ�.
        /// </summary>
        public string objectTitleRichText { get; private set; }

        /// <summary>
        /// ������ ���Ե��� ���� Ÿ��Ʋ �ؽ�Ʈ�Դϴ�.
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