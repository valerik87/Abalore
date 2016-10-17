using UnityEngine;
using System.Collections;
using System;

public enum E_GameState
{
    IDLE,
    DRAG
}

public enum E_Tags
{
    BALL,
    TILE,
    STATIC_SCRIPS
}

public enum E_Player
{
    PLAYER_ONE,
    PLAYER_TWO
}

public abstract class SceneSingleton : MonoBehaviour
{
    void Awake()
    {
        if(tag.CompareTo(E_Tags.STATIC_SCRIPS.ToString()) == 0)
        {
            if(DoubleInstance())
            {
                Debug.LogError("DOUBLE ATTACHED TO STATIC_SCRIPTS: " + this.name);
            }
        }
        else
        {
            Debug.LogError("NOT ATTACHED TO STATIC_SCRIPTS: " + this.name);
        }
        InternalAwake();
    }

    public bool DoubleInstance()
    {
        Type Type = GetType();
        if (GetComponents(Type).Length > 1)
        {
            return true;
        }
        return false;
    }

    abstract public void InternalAwake();
}

public class Enumerations : SceneSingleton
{

    public override void InternalAwake()
    {
    }
}
