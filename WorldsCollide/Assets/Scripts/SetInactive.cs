using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInactive : MonoBehaviour
{
	public GameObject thisobject;


	public void OnTriggerEnter2D(Collider2D other){
		if(other.name == "Player"){
		thisobject.SetActive(false);
		}
	}
}
