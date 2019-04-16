using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
	public GameObject key;
	public void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.name == "Player"){
			key.SetActive(false);
			GameController.control.keys += 1;
		}


	}
}
