using UnityEngine;
using System.Collections;

public class RecieveText : MonoBehaviour {

	private string input;
	private GUIText myText;

	void Start()
	{
		myText = GetComponent<GUIText>();
	}

	void Update () 
	{
		if( Input.anyKeyDown )
		{
			input = input + Input.inputString;
			myText.text = input;
		}
	}
}
