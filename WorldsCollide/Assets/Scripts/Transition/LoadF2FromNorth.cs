using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadF2FromNorth : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.name == "PlayerSprite"){
			SceneManager.LoadScene("Floor1FromNorthLadder");
		}
	}
}
