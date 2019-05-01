using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkquest : MonoBehaviour
{
	public GameObject knightpreGO;
	public bool knightpre = true;
	public GameObject knightpostGO;
	public bool knightpost = false;
	public GameObject knightrepeatGO;
	public bool knightrepeat = false;
	public GameObject magepreGO;
	public bool magepre = true;
	public GameObject magepostGO;
	public bool magepost = false;
	public GameObject magerepeatGO;
	public bool magerepeat = false;
	static public Checkquest ch;


	public void Update(){
		if(GameController.control.grenade == true){
			knightpreGO.SetActive(false);
			knightpostGO.SetActive(true);
		}
		if(knightrepeat == true){
			knightpostGO.SetActive(true);
			knightrepeatGO.SetActive(true);
		}


		if(GameController.control.staff == true){
			magepreGO.SetActive(false);
			magepostGO.SetActive(true);
		}

		if(magerepeat == true){
			magepostGO.SetActive(false);
			magerepeatGO.SetActive(true);

		}

	}
}
