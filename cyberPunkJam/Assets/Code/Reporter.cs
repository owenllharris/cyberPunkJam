using UnityEngine;
using System.Collections;

public class Reporter : MonoBehaviour 
{
	public AudioClip ErrorSound;

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
		audio.PlayOneShot(ErrorSound);

		StartCoroutine(Blink(goAway));
	}

	IEnumerator Blink(bool goAway)
	{
		yield return new WaitForSeconds( 0.3f );
		myText.enabled = false;
		yield return new WaitForSeconds( 0.3f );
		myText.enabled = true;
		audio.PlayOneShot(ErrorSound);
		if( goAway )
		{
			yield return new WaitForSeconds( 0.3f );
			myText.enabled = false;
		}
	}


}
