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
		/*data.MaxHealth=GameController.control.MaxHealth ;
		data.CurrentHealth=GameController.control.CurrentHealth;
		data.RedWeapon=GameController.control.RedWeapon ;
		data.BlueWeapon=GameController.control.BlueWeapon ;
		data.GreenWeapon=GameController.control.GreenWeapon;
		data.YellowWeapon=GameController.control.YellowWeapon ;
		data.healthupgrade1=GameController.control.healthupgrade1 ;
		data.healthupgrade2=GameController.control.healthupgrade2 ;
		data.healthupgrade3=GameController.control.healthupgrade3 ;
		data.healthupgrade4=GameController.control.healthupgrade4 ;
		data.healthupgrade5=GameController.control.healthupgrade5;
		data.healthupgrade6=GameController.control.healthupgrade6;
		data.healthupgrade7= GameController.control.healthupgrade7;
		data.healthupgrade8= GameController.control.healthupgrade8 ;
		data.whichsave = GameController.control.whichsave;*/
		formatter.Serialize (path, data);
		path.Close ();
	}


	public void LoadPlayer(){

		if (File.Exists (Application.persistentDataPath + "/player.boi")) {
			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/player.boi", FileMode.Open);
			PlayerData data = (PlayerData)formatter.Deserialize (file);
			file.Close ();
			/*if (data.CurrentHealth == '0') {
				data.CurrentHealth = data.MaxHealth;
			}*/
			/*GameController.control.MaxHealth = data.MaxHealth;
			GameController.control.CurrentHealth = 100;
			GameController.control.RedWeapon = data.RedWeapon;
			GameController.control.BlueWeapon = data.BlueWeapon;
			GameController.control.GreenWeapon = data.GreenWeapon;
			GameController.control.YellowWeapon = data.YellowWeapon;
			GameController.control.healthupgrade1 = data.healthupgrade1;
			GameController.control.healthupgrade2 = data.healthupgrade2;
			GameController.control.healthupgrade3 = data.healthupgrade3;
			GameController.control.healthupgrade4 = data.healthupgrade4;
			GameController.control.healthupgrade5 = data.healthupgrade5;
			GameController.control.healthupgrade6 = data.healthupgrade6;
			GameController.control.healthupgrade7 = data.healthupgrade7;
			GameController.control.healthupgrade8 = data.healthupgrade8;
			GameController.control.whichsave = data.whichsave;
			if (GameController.control.whichsave == "entrance") {
				Application.LoadLevel (5);

			}
			if (GameController.control.whichsave == "red") {
				Application.LoadLevel (12);

			}
			if (GameController.control.whichsave == "blue") {
				Application.LoadLevel (3);

			}
			if (GameController.control.whichsave == "green") {
				Application.LoadLevel (8);

			}*/


		} else {
			/*GameController.control.MaxHealth = 100;
			GameController.control.CurrentHealth = 100;
			GameController.control.RedWeapon = true;
			GameController.control.BlueWeapon =false;
			GameController.control.GreenWeapon = false;
			GameController.control.YellowWeapon = false;
			GameController.control.healthupgrade1 = false;
			GameController.control.healthupgrade2 =false;
			GameController.control.healthupgrade3 = false;
			GameController.control.healthupgrade4 = false;
			GameController.control.healthupgrade5 = false;
			GameController.control.healthupgrade6 =false;
			GameController.control.healthupgrade7 = false;
			GameController.control.healthupgrade8 = false;
			GameController.control.whichsave = "entrance";*/
			Application.LoadLevel (5);
		}


	}
}


[System.Serializable]
class PlayerData{
	/*public int MaxHealth;
	public  int CurrentHealth;
	public  bool RedWeapon;
	public  bool BlueWeapon;
	public  bool GreenWeapon;
	public  bool YellowWeapon;
	public  bool healthupgrade1;
	public  bool healthupgrade2;
	public  bool healthupgrade3;
	public  bool healthupgrade4;
	public  bool healthupgrade5;
	public  bool healthupgrade6;
	public  bool healthupgrade7;
	public  bool healthupgrade8;
	public string whichsave;*/
}

