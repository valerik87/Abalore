using UnityEngine;
using System.Collections;
using System;

public class MouseManager : SceneSingleton
{
    public override void InternalAwake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 Origin = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(Origin, Vector2.zero, 0f);

            if (hit.collider != null)
            {
                GameObject ge = hit.collider.gameObject;
                MouseDownOn(ge);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 Origin = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(Origin, Vector2.zero, 0f);

            if (hit.collider != null)
            {
                GameObject ge = hit.collider.gameObject;
                MouseUpOn(ge);
            }
            else
            {
                MouseReleased();
            }
        }

        if(Input.GetMouseButton(0))
        {
            Vector2 Origin = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(Origin, Vector2.zero, 0f);

            if (hit.collider != null)
            {
                GameObject ge = hit.collider.gameObject;
                MouseDownOn(ge);
            }
            else
            {
                MouseUpOnNothing();
            }
        }
    }

    void MouseDownOn(GameObject ge)
    {
        switch (GameLogic.GetGameState())
        {
            case (E_GameState.IDLE):

                if (ge.CompareTag(E_Tags.BALL.ToString()))
                {
                    BallData ball = ge.GetComponent<BallData>();
                    if(ball)
                    {
                        GameLogic.BallSelectedInIdle(ball);
                    }
                    else
                    {
                        Log.Text("BallData doesn't exist while selecting a ball in IDLE",E_LogContext.MOUSE_MANAGER);
                    }
                }

                break;
            case (E_GameState.PUSHING):
            case (E_GameState.DRAG):
                if (ge.CompareTag(E_Tags.TILE.ToString()))
                {
                    PositionData tile = ge.GetComponent<PositionData>();
                    if(tile)
                    {
                        Log.Text("MouseDownOn a tile", E_LogContext.MOUSE_MANAGER);
                        GameLogic.BallDraggingOverTile(tile);
                    }
                    else
                    {
                        Log.Error("PositionData doesn't exist");
                    }
                }
                else if (ge.CompareTag(E_Tags.BALL.ToString()))
                {
                    BallData ball = ge.GetComponent<BallData>();
                    if (ball)
                    {
                        Log.Text("MouseDownOn a ball", E_LogContext.MOUSE_MANAGER);
                        GameLogic.BallDraggingOverBall(ball);
                    }
                    else
                    {
                        Log.Text("BallData doesn't exist while mouse over a ball in DRAG", E_LogContext.MOUSE_MANAGER);
                    }
                }
                break;
            default:
                break;
        }
    }

    void MouseUpOn(GameObject ge)
    {
        Log.Text("Clicked up on: " + ge.name, E_LogContext.MOUSE_MANAGER);
        switch (GameLogic.GetGameState())
        {
            case (E_GameState.PUSHING):
            case (E_GameState.DRAG):
                if (ge.CompareTag(E_Tags.TILE.ToString()))
                {
                    PositionData tile = ge.GetComponent<PositionData>();
                    if(tile)
                    {
                        GameLogic.BallDeselectedOnTile(tile);
                    }
                    else
                    {
                        Log.Text("TileData doesn't exist while mouse release over a tile in DRAG", E_LogContext.MOUSE_MANAGER);
                    }
                }
                else if (ge.CompareTag(E_Tags.BALL.ToString()))
                {
                    BallData ball = ge.GetComponent<BallData>();
                    if (ball)
                    {
                        GameLogic.BallDeselectedOnBall(ball);
                    }
                    else
                    {
                        Log.Text("BallData doesn't exist while mouse release over a ball in DRAG", E_LogContext.MOUSE_MANAGER);
                    }
                }     
                break;
            
            default:
                break;
        }
    }

    void MouseReleased()
    {
        switch (GameLogic.GetGameState())
        {
            case (E_GameState.PUSHING):
            case (E_GameState.DRAG):
                Log.Text("Mouse Released while dragging ball", E_LogContext.MOUSE_MANAGER);
                GameLogic.BallDeselectedOnNothing();
                break;
            default:
                break;
        }
        Log.Text("Mouse Released", E_LogContext.MOUSE_MANAGER);
    }

    void MouseUpOnNothing()
    {
        switch (GameLogic.GetGameState())
        {
            case (E_GameState.PUSHING):
            case (E_GameState.DRAG):
                Log.Text("Mouse Up On Nothing while pushing ball", E_LogContext.MOUSE_MANAGER);
                GameLogic.BallDraggingOnNothing();
                break;
            default:
                break;
        }
    }
}
