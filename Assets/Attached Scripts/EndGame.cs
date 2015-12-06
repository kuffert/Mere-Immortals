using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void onClick(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		Physics.Raycast (ray, out hit);
		if (this.GetComponent<BoxCollider> ().Raycast (ray, out hit, 100)) {
			print ("CLICKED");
			Application.Quit();	
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			onClick ();
		}
	}
}
