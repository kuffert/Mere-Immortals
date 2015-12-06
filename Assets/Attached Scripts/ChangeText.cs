using UnityEngine;
using System.Collections;

public class ChangeText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<TextMesh> ().text = GameInfo.endGameNotification;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
