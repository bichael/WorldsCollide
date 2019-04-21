using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red : MonoBehaviour
{
	public GameObject RedBlock;

	public void MakeActive(){
		RedBlock.SetActive(true);
	}

	public void MakeInactive(){
		RedBlock.SetActive(false);
	}
}
