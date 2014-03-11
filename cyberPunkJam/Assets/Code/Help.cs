using UnityEngine;
using System.Collections;

public class Help : MonoBehaviour {

	private GUIText gui;

	// Use this for initialization
	void Start () 
	{
		gui = GetComponent<GUIText>();

		gui.text = "commands\n" +
			"\n" +
			"left.......left\n" +
			"right......right\n" +
			"forward....forward\n" +
			"back.......back\n" +
			"access.....access";
	}

	void Update()
	{
		if( Input.anyKeyDown && gui.enabled )
			gui.enabled = false;
	}


	

}
