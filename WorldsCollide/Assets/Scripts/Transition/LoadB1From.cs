using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadB1From : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.name == "Player"){
			SceneManager.LoadScene("FloorB1FromWestLadder");
		}
	}

   
}
