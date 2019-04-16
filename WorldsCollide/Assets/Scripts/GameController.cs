using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static GameController control;
	public int keys = 0;

	void Update()
	{
		// Restart scene at will
        if (Input.GetKey(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void Awake(){
		if(control == null){
			DontDestroyOnLoad(gameObject);
			control = this;
		}
		else if(control != this){
			Destroy(gameObject);
		}
	}
}
