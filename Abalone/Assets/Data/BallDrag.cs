using UnityEngine;
using System.Collections;

public class BallDrag : MonoBehaviour {

    private Vector3 OldPosition;

	// Use this for initialization
	void Start () {
        OldPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, this.transform.position.z);
        this.transform.position = newPosition;
	}
}
