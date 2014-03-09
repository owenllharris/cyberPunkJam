using UnityEngine;
using System.Collections;

public class ShowData : MonoBehaviour 
{
	public static ShowData instance;

	private GUIText guiText;
	private int currentData;


	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		guiText = GetComponent<GUIText>();
		updateText(currentData);
	}

	public void updateText( int newData )
	{
		currentData = newData;
		guiText.text = "current data: " + currentData + "MB";
	}
}
