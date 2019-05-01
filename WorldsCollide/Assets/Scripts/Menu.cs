using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
	public GameObject MenuUI;
    // Update is called once per frame
	public void MainMenu()    {
		if(MenuUI){
			MenuUI.SetActive(false);

		}
		SceneManager.LoadScene("MainMenu");
    }
}
