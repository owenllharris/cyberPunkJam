using UnityEngine;
using System.Collections;

public class RecieveText : MonoBehaviour {

	public int characterLimit = 50;
	public UpdateText lineAbove;
	public CommandProcessor cp;

	private string inputText = "";
	private GUIText myText;

	void Start()
	{
		myText = GetComponent<GUIText>();
	}

	void Update () 
	{
		if( Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace) )
		{
			if( inputText.Length > 0 )
			{
				updateConsole(inputText.Remove(  inputText.Length - 1));
			}
		}
		else if( Input.GetKeyDown( KeyCode.Return ) )
		{
			processCommand();
		}
		else if( Input.anyKeyDown )
		{
			if( inputText.Length <= characterLimit )
			{
				updateConsole(inputText + Input.inputString);
			}
		}
	}

	void processCommand()
	{
		cp.Command( inputText );

		lineAbove.updateText( inputText );
		updateConsole("");
	}

	void updateConsole( string newText)
	{
		inputText = newText;
		myText.text = "> " + inputText;
	}
}
