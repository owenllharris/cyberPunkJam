using UnityEngine;
using System.Collections;

public class Reporter : MonoBehaviour 
{
	public static Reporter instance;

	private GUIText myText;

	void Awake()
	{
		if( instance == null )
			instance = this;
		else
			Destroy(gameObject);
	}

	void Start()
	{
		myText = GetComponent<GUIText>();
		myText.enabled = false;
	}

	public void ShowMessage( string message, bool goAway )
	{
		myText.text = message;
		myText.enabled = true;

		StartCoroutine(Blink(goAway));
	}

	IEnumerator Blink(bool goAway)
	{
		yield return new WaitForSeconds( 0.3f );
		myText.enabled = false;
		yield return new WaitForSeconds( 0.3f );
		myText.enabled = true;

		if( goAway )
		{
			yield return new WaitForSeconds( 0.3f );
			myText.enabled = false;
		}
	}


}
