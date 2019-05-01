using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasFirstTalk : MonoBehaviour
{
	

		public bool requireButtonPress;

		public bool destroyWhenActivated;
		private bool waitForPress =true;
		public bool firsttalk = false;
	public bool secondtalk = false;
		public bool repeattime = false;
	//static public Checkquest ch;
		// Start is called before the first frame update

	void Update()
	{
		if(waitForPress && Input.GetKeyDown("g")){
			firsttalk = true;
			if(GameController.control.grenade == true && firsttalk){
				Checkquest.ch.knightpre = false;
				Checkquest.ch.knightpost = true;
				//Checkquest.ch.Check();
				firsttalk = false;
				secondtalk = true;

			}

			if(GameController.control.grenade == true && secondtalk){
				Checkquest.ch.knightrepeat = true;
				Checkquest.ch.knightpre = false;
				Checkquest.ch.knightpost = false;
				firsttalk = false;
				//Checkquest.ch.Check();
			}

			if (destroyWhenActivated) {
				Destroy (gameObject);

			}
		}

		}

		// Update is called once per frame
	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "Player") {
			

				if (requireButtonPress) {
					waitForPress = true;
					return;
				}
		}
			
		}





		

	
	


}