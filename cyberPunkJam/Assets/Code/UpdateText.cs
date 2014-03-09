using UnityEngine;
using System.Collections;

public class UpdateText : MonoBehaviour {

	public UpdateText LineAbove;

	private GUIText myText;
	private string inputText = "";

	void Start()
	{
		myText = GetComponent<GUIText>();
	}

	public void updateText( string theNewText )
	{
		if( LineAbove != null )
			LineAbove.updateText( inputText );

		updateConsole(theNewText);
	}

	void updateConsole(string theNewText)
	{
		inputText = theNewText;
		myText.text = "> " + inputText;
	}


}
