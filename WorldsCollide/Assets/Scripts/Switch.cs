using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
	
	public Red redscript;
	public Blue bluescript;

	public bool blueactive;
	public bool redactive;



	public void OnTriggerEnter2D(Collider2D col){
		if(col.name == "Player"){
			

			if((bluescript != null) && blueactive){
				Debug.Log("BlueDown");
				blueactive = false;
				redactive = true;
				redscript.MakeActive();
				bluescript.MakeInactive();
			}
			else{
				Debug.Log("Red down");
				blueactive = true;
				redactive = false;
				redscript.MakeInactive();
				bluescript.MakeActive();
			}
		}

	}
}
