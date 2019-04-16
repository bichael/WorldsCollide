using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossDoor : MonoBehaviour
{
	public GameObject door;


	/*public void Awake()
    {
        occupants = new List<Collider2D>();
    }*/

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.name == "Player"){
			if(GameController.control.keys == 4){
			door.SetActive(false);
		}


	}

	/*public void OnTriggerStay2D(Collider2D collision)
    {
        if (ZoneContinuesToBeOccupied.GetPersistentEventCount() != 0)
            ZoneContinuesToBeOccupied.Invoke();
    }*/

}
}