using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door: MonoBehaviour
{
	public GameObject door;


    /*public void Awake()
    {
        occupants = new List<Collider2D>();
    }*/

    public void OnTriggerEnter2D(Collider2D collision)
    {
		if(collision.name == "Player"){
			door.SetActive(false);
		}

        
    }

    /*public void OnTriggerStay2D(Collider2D collision)
    {
        if (ZoneContinuesToBeOccupied.GetPersistentEventCount() != 0)
            ZoneContinuesToBeOccupied.Invoke();
    }*/

    public void OnTriggerExit2D(Collider2D collision)
    {
		if(collision.name == "Player"){
			door.SetActive(true);
		}
        
    }
}