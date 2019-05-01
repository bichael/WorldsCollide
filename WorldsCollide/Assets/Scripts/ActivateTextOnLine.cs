using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActivateTextOnLine : MonoBehaviour
{

	public TextAsset theText;
	public int startLine;
	public int endLine;

	public TextBoxManager theTextBox;

	public bool requireButtonPress;

	public bool destroyWhenActivated;
	private bool waitForPress;


    // Start is called before the first frame update
    void Start()
    {
		theTextBox = FindObjectOfType<TextBoxManager> ();
    }

    // Update is called once per frame
    void Update()
    {
		if(waitForPress && Input.GetKeyDown("g")){
			theTextBox.ReloadScript (theText);
			theTextBox.currentLine = startLine;
			theTextBox.endAtLine = endLine;
			theTextBox.EnableTextBox ();

			if (destroyWhenActivated) {
				Destroy (gameObject);

			}
		}
    }

	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "Player") {

			if (requireButtonPress) {
				waitForPress = true;
				return;
			}

			theTextBox.ReloadScript (theText);
			theTextBox.currentLine = startLine;
			theTextBox.endAtLine = endLine;
			theTextBox.EnableTextBox ();

			if (destroyWhenActivated) {
				Destroy (gameObject);
			
			}

		}
	}


	void OnTriggerExit2D(Collider2D other){
		if (other.name == "Player") {
			waitForPress = false;
		}
	}


}
