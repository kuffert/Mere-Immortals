using UnityEngine;
using System.Collections;

public class HideWeatherAndMarker : MonoBehaviour {

	public GameObject button;
	public GameObject weatherboard;
	public GameObject marker;
	private static int clicks = 0;
	// Use this for initialization
	void Start () {
	
	}

	void onClick(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		Physics.Raycast (ray, out hit);
		if (button.GetComponent<BoxCollider> ().Raycast (ray, out hit, 100)) {

			if(clicks == 0) {
				clicks++;
				weatherboard.GetComponent<Renderer>().enabled = false;
				marker.GetComponent<Renderer>().enabled = false;
			}
			else{
				clicks = 0;
				weatherboard.GetComponent<Renderer>().enabled = true;
				marker.GetComponent<Renderer>().enabled = true;
			}
		}
	}


	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			onClick ();
		}
	}

}
