using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static GameController control;
	public int keys = 0;
	public bool blueactive;
	public bool redactive;

	public Red redscript;
	public Blue bluescript;
	void Update()
	{
		// Restart scene at will
        if (Input.GetKey(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

		if((redscript != null) && redactive){
			redscript.MakeInactive();
			bluescript.MakeActive();
		}

		if((bluescript != null) && blueactive){
			redscript.MakeActive();
			bluescript.MakeInactive();
		}
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
