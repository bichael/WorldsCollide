using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
	
	//public Red Redscript;
	//public Blue Bluescript;

    

	public void OnTriggerEnter2D(Collider2D col){
		if(col.name == "Player"){
			if(GameController.control.redactive == true){
				GameController.control.redactive = false;
				GameController.control.blueactive = true;

				//Redscript.MakeInactive();
				//Bluescript.MakeActive();
			}
			else if(GameController.control.redactive == false){
				GameController.control.redactive = true;
				GameController.control.blueactive = false;

				//Redscript.MakeActive();
				//Bluescript.MakeInactive();
			}
		}

	}
}
