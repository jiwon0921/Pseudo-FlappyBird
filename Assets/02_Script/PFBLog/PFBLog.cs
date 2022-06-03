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
    //    [Tooltip("하루마다 파일을 새로 생성합니다.")]
    //    OneDay,
    //    [Tooltip("플레이할 때마다 파일을 새로 생성합니다.")]
    //    Play,
    //}

    public enum ePFBLogSaveMode
    {
        [Tooltip("로그를 따로 저장하지 않습니다.")]
        DoNotSave,

        [Tooltip("로그를 발송할 때마다 저장합니다.")]
        OnRealTime,

        [Tooltip("PlayMode가 종료될 때 한꺼번에 저장합니다.")]
        OnExitPlayMode,

        [Tooltip("게임이 종료될 때 한꺼번에 저장합니다.")]
        OnExitGame,


    }
    /// <summary>
    /// 로그용 클래스 입니다
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
        /// 서식이 포함된 타이틀 텍스트입니다.
        /// </summary>
        public string objectTitleRichText { get; private set; }

        /// <summary>
        /// 서식이 포함되지 않은 타이틀 텍스트입니다.
        /// </summary>
        public string objectTitle { get; private set; }


        /// <summary>
        /// 서식 텍스트를 빼서 반환합니다.
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