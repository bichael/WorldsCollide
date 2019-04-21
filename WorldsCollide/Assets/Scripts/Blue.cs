using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue : MonoBehaviour
{
	public GameObject BlueBlock;

	public void MakeActive(){
		BlueBlock.SetActive(true);
	}

	public void MakeInactive(){
		BlueBlock.SetActive(false);
	}
}
