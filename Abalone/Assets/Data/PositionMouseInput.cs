using UnityEngine;
using System.Collections;

public class PositionMouseInput : MonoBehaviour {

    public PositionData PosData;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        Debug.Log("Clicked down on: " + PosData.ID);
    }

    void OnMouseUp()
    {
        Debug.Log("Clicked up on: " + PosData.ID);
    }
}
