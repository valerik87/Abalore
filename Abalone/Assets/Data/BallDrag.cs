using UnityEngine;
using System.Collections;

public class BallDrag : MonoBehaviour {

    private Vector3 OldPosition;

    public void StartDrag()
    {
        Debug.Log("Start dragging ball : " + name);
        OldPosition = this.transform.position;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, this.transform.position.z);
	}

    public void StopDrag()
    {
        Debug.Log("Stop dragging ball : " + name);
        BallData ballData = GetComponent<BallData>();
        if (ballData != null)
        {
            PositionData positionData = ballData.GetPositionData();
            if (positionData != null)
            {
                this.transform.position = new Vector3(positionData.transform.position.x, positionData.transform.position.y, this.transform.position.z);
                Reset();
            }
            else
            {
                this.transform.position = new Vector3(OldPosition.x, OldPosition.y, this.transform.position.z);
                Reset();
                Debug.Log("StopDrag(): no position data");

            }
        }
        else
        {
            Debug.Log("StopDrag(): no ball data");
        }
    }

    public void Reset()
    {
        GetComponent<Collider2D>().enabled = true;
        this.enabled = false;
    }
}
