using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadF1FromF2North : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.name == "PlayerSprite"){
			SceneManager.LoadScene("Floor1FromF2North");
		}
	}
}
