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
        [HideInInspector]
        Log = 0,
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
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
            pfbLogType = PFBLogHelper.current.logLevel;
            dateString = date;
            titleString = title;
            titleRichString = title;
            messageString = msg;
        }
    }

    //public enum eLogFileCreateMode
    //{
    //    [Tooltip("�Ϸ縶�� ������ ���� �����մϴ�.")]
    //    OneDay,
    //    [Tooltip("�÷����� ������ ������ ���� �����մϴ�.")]
    //    Play,
    //}

    public enum ePFBLogSaveMode
    {
        [Tooltip("�α׸� ���� �������� �ʽ��ϴ�.")]
        DoNotSave,

        [Tooltip("�α׸� �߼��� ������ �����մϴ�.")]
        OnRealTime,

        [Tooltip("�����Ϳ� ������ ������ �ٸ��ϴ�. \n[Editor]PlayMode�� ����� �� �Ѳ����� �����մϴ�.\n[Game]������ ����� �� �Ѳ����� �����մϴ�.")]
        OnExit,

        //[Tooltip("������ ����� �� �Ѳ����� �����մϴ�.")]
        //OnExitGame,


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

            //if (color.a != default(Color32).a)
            //{

            //}

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
            //PFBLogHelper.current.LogType(new PFBLogMessage(PFBLogHelper.current.GetDateTime(), objectTitleRichText, objectTitle, msg.ToString()));
        }

        public void Log(object msg)
        {
            message.dateString = PFBLogHelper.current.GetDateTime();
            message.messageString = msg.ToString();
            PFBLogHelper.current.Log(message);
            //PFBLogHelper.current.Log(new PFBLogMessage(PFBLogHelper.current.GetDateTime(), objectTitleRichText, objectTitle, msg.ToString()));
        }

        public void LogInfo(object msg)
        {
            message.dateString = PFBLogHelper.current.GetDateTime();
            message.messageString = msg.ToString();
            PFBLogHelper.current.LogInfo(message);
            //PFBLogHelper.current.LogInfo(new PFBLogMessage(PFBLogHelper.current.GetDateTime(), objectTitleRichText, objectTitle, msg.ToString()));
        }

        public void LogWarning(object msg)
        {
            message.dateString = PFBLogHelper.current.GetDateTime();
            message.messageString = msg.ToString();
            PFBLogHelper.current.LogWarning(message);
            //PFBLogHelper.current.LogWarning(new PFBLogMessage(PFBLogHelper.current.GetDateTime(), objectTitleRichText, objectTitle, msg.ToString()));
        }

        public void LogError(object msg)
        {
            message.dateString = PFBLogHelper.current.GetDateTime();
            message.messageString = msg.ToString();
            PFBLogHelper.current.LogError(message);
            //PFBLogHelper.current.LogError(new PFBLogMessage(PFBLogHelper.current.GetDateTime(), objectTitleRichText, objectTitle, msg.ToString()));
        }

    }

}