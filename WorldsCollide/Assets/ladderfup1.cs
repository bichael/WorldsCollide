using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladderfup1 : MonoBehaviour
{
	/*void OnTriggerEnter2D(Collision2D col){
		if(col.gameObject.name == "Player"){
			col.gameObject.transform.position = new Vector3(0,0,0);(-33.74502,1.0576,0)
			GetComponent<CharacterController>().enabled = false;
		}
	}*/

	private void LateUpdate(){
		if(GetComponent<CharacterController> ().enabled == false){
			GetComponent<CharacterController> ().enabled = true;
		}
	}

}
