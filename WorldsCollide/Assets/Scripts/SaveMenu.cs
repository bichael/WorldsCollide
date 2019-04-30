using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
	public Player player;
	public GameObject save;
	public HUD HUD2;
	public void OnTriggerEnter2D(Collider2D col){
		if(col.name == "Player"){
			player.playercanmove = false;
			HUD2.OpenSavePanel();

		}
	}

	public void Return(){
		player.playercanmove = true;
		HUD2.CloseSavePanel();


	}

}
