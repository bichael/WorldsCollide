using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlock : MonoBehaviour
{
	public GameObject Key;
	public GameObject block;
    // Update is called once per frame
    void Update()
    {
		
		if(!Key.active){
			block.SetActive(false);
		}
    }
}
