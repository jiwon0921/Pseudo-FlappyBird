using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;


namespace PFB
{
    public enum ePFBLogType
    {
        [HideInInspector]
        FollowLogHelperCurrent,
        Log,
        Info,
        Warning,
        Error,
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

        [Tooltip("PlayMode�� ����� �� �Ѳ����� �����մϴ�.")]
        OnExitPlayMode,

        [Tooltip("������ ����� �� �Ѳ����� �����մϴ�.")]
        OnExitGame,


    }
    /// <summary>
    /// �α׿� Ŭ���� �Դϴ�
    /// </summary>
    public class PFBLog
    {
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
            objectTitle = GetOnlyTitle(objectTitleRichText);
        }
        /// <summary>
        /// ������ ���Ե� Ÿ��Ʋ �ؽ�Ʈ�Դϴ�.
        /// </summary>
        public string objectTitleRichText { get; private set; }

        /// <summary>
        /// ������ ���Ե��� ���� Ÿ��Ʋ �ؽ�Ʈ�Դϴ�.
        /// </summary>
        public string objectTitle { get; private set; }


        /// <summary>
        /// ���� �ؽ�Ʈ�� ���� ��ȯ�մϴ�.
        /// </summary>
        private string GetOnlyTitle(string name)
        {
            Regex rich = new Regex(@"<[^>]*>");

            if (rich.IsMatch(name))
            {
                name = rich.Replace(name, string.Empty);
            }
            return name;
        }

        public void Log(object msg, ePFBLogType logType)
        {
            PFBLogHelper.current.Log(objectTitleRichText + msg, logType);
        }
        public void Log(object msg)
        {
            PFBLogHelper.current.Log(objectTitleRichText + msg);
        }
        public void LogInfo(object msg)
        {
            PFBLogHelper.current.LogInfo(objectTitleRichText + msg);
        }
        public void LogWarning(object msg)
        {
            PFBLogHelper.current.LogWarning(objectTitleRichText + msg);
        }

        public void LogError(object msg)
        {
            PFBLogHelper.current.LogError(objectTitleRichText + msg);
        }

    }

}