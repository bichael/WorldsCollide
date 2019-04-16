using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
	public GameObject key;
	private GameObject keyUI;

	void Awake ()
    {
        keyUI = GameObject.FindGameObjectWithTag("KeyUI");
    }

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.name == "Player"){
			key.SetActive(false);
			GameController.control.keys += 1;
			AssertKeyUI();
		}
	}

	void AssertKeyUI()
    {
        keyUI.transform.GetComponentInChildren<Text>().text = GameController.control.keys.ToString() + "/4";
    }
}
