using UnityEngine;
using System.Collections;

public class KeyboardSounds : MonoBehaviour {

	public static KeyboardSounds instance;

	public AudioClip[] clicks;

	void Awake()
	{
		if ( instance == null )
			instance = this;
		else
			Destroy(gameObject);
	}

	void Update()
	{
		if( Input.anyKeyDown )
			audio.PlayOneShot( clicks[Random.Range(0, clicks.Length) ] );
	}
}
