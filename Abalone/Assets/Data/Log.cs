using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum E_LogContext
{
    MOUSE_MANAGER,
    GAME_LOGIC,
    BALL
}

public class Log : SceneSingleton {

    //EDITOR_SET
    public List<E_LogContext> LogContext;

    //-----------------Private
    private static List<E_LogContext> m_vLogContext;

    public override void InternalAwake()
    {
        if(LogContext.Count > 0)
        {
            m_vLogContext = new List<E_LogContext>();
            foreach (E_LogContext context in LogContext)
            {
                m_vLogContext.Add(context);
            }
        }
    }

    public static void Text(string inText)
    {
        Debug.Log(inText);
    }

    public static void Text(string inText,E_LogContext inContext)
    {
        if(m_vLogContext != null && m_vLogContext.Count > 0 && m_vLogContext.Contains(inContext)) Debug.Log(inText);
    }
}

