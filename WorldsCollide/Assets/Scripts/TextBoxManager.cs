using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour
{
	public GameObject textBox;

	public Text theText;

	public TextAsset textFile;
	public bool isActive;
	public string[] textLines;
	public int currentLine;
	public int endAtLine;
	public Player player;
	public bool signdestroy = false;

	public bool stopPlayerMovement;
    // Start is called before the first frame update
    void Start()
    {
		player = FindObjectOfType<Player> ();

		if (textFile != null) {
			textLines = (textFile.text.Split ('\n'));
		}
		if (endAtLine == 0) {
			endAtLine = textLines.Length - 1;
		  
		}
		if (isActive) {
			EnableTextBox ();
		} else {
			DisableTextBox ();
		
		}



    }

    // Update is called once per frame
    void Update()
    {

		if (!isActive) {
			return;
		}


		theText.text = textLines [currentLine];

		if (Input.GetKeyDown (KeyCode.Space)) {
			currentLine += 1;
		}

		if (currentLine > endAtLine) {
			DisableTextBox ();
			signdestroy = true;

		}
    }



	public void EnableTextBox(){
		textBox.SetActive (true);
		isActive = true;
		if (stopPlayerMovement) {
			player.playercanmove = false;
		}

	}

	public void DisableTextBox(){
		textBox.SetActive (false);
		isActive = false;


		player.playercanmove = true;

	}

	public void ReloadScript(TextAsset theText){
		if (theText != null) {
			textLines = new string[1];
			textLines = (theText.text.Split('\n'));
		}
	}

}
