using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoad : MonoBehaviour
{

	public static SaveLoad saveld;

	void Awake(){
		saveld = this;
	}
	public void SavePlayer(){
		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream path = File.Create(Application.persistentDataPath + "/player.boi");
		//FileStream stream = new FileStream (path, FileMode.Create);

		PlayerData data = new PlayerData ();
		data.keys = GameController.control.keys;
		data.blueactive = GameController.control.blueactive;
		data.redactive = GameController.control.redactive;
		data.grenade = GameController.control.grenade;
		data.staff = GameController.control.staff;
		data.watch = GameController.control.watch;
		data.cola = GameController.control.cola;
		data.fantasydone = GameController.control.fantasydone;
		data.scifidone = GameController.control.scifidone;
		data.wastelanddone = GameController.control.wastelanddone;
		formatter.Serialize (path, data);
		path.Close ();
	}


	public void LoadPlayer(){

		if (File.Exists (Application.persistentDataPath + "/player.boi")) {
			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/player.boi", FileMode.Open);
			PlayerData data = (PlayerData)formatter.Deserialize (file);
			file.Close ();
			GameController.control.keys = GameController.control.keys;
			GameController.control.blueactive = data.blueactive;
			GameController.control.redactive = data.redactive;
			GameController.control.grenade = data.grenade;
			GameController.control.staff = data.staff;
			GameController.control.watch = data.watch;
			GameController.control.cola = data.cola;
			GameController.control.fantasydone = data.fantasydone;
			GameController.control.scifidone = data.scifidone;
			GameController.control.wastelanddone = data.wastelanddone;



		} else {
			//Application.LoadLevel (5);
		}


	}
}


[System.Serializable]
class PlayerData{
	public int keys;
	public bool blueactive;
	public bool redactive;
	public bool grenade;
	public bool staff;
	public bool watch;
	public bool cola;
	public bool fantasydone;
	public bool scifidone;
	public bool wastelanddone;
}

