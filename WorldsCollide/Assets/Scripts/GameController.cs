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
	public bool grenade = false;
	public bool shield = false;
	public bool shotgun = false;
	public bool staff = false;
	public bool watch = false;
	public bool soda = false;
	public bool fantasydone = false;
	public bool scifidone = false;
	public bool wastelanddone = false;
	public bool cannon = false;
	//public Red redscript;
	//public Blue bluescript;
	
	void Update()
	{

		if (Input.GetKey(KeyCode.R))
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);

		
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || (Input.GetKeyDown(KeyCode.Keypad1)))
            {
                SetAllItemsTrue();
                SceneManager.LoadScene("Hub");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || (Input.GetKeyDown(KeyCode.Keypad2)))
            {
                SetAllItemsTrue();
                SceneManager.LoadScene("Gary");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || (Input.GetKeyDown(KeyCode.Keypad3)))
            {
                SetAllItemsTrue();
                SceneManager.LoadScene("KALI");
            }
        }
	}

	public void SetAllItemsTrue()
	{
		grenade = true;
		shield = true;
		shotgun = true;
		staff = true;
		watch = true;
		soda = true;
		fantasydone = true;
		scifidone = true;
		wastelanddone = true;
		cannon = true;
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
