using UnityEngine;
using System.Collections;

public class MouseManager : SceneSingleton
{
    public override void InternalAwake()
    {
    }

    // Use this for initialization
    void Start()
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
                    GameLogic.BallSelectedInIdle(ball);
                }

                break;
            case (E_GameState.DRAG):
                if (ge.CompareTag(E_Tags.TILE.ToString()))
                {
                    PositionData tile = ge.GetComponent<PositionData>();
                    GameLogic.MouseDraggingOverTile(tile);
                }

                break;
            default:
                break;
        }
    }

    void MouseUpOn(GameObject ge)
    {
        Debug.Log("Clicked up on: " + ge.name);
        switch (GameLogic.GetGameState())
        {
            case (E_GameState.DRAG):

                if (ge.CompareTag(E_Tags.TILE.ToString()))
                {
                    PositionData tile = ge.GetComponent<PositionData>();
                    GameLogic.BallDeselectedOnTile(tile);
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
            case (E_GameState.DRAG):
                Debug.Log("Mouse Released while dragging ball");
                GameLogic.BallDeselectedOnNothing();
                break;
            default:
                Debug.Log("Mouse Released");
                break;
        }
    }
}
