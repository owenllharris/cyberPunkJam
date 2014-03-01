using UnityEngine;
using System.Collections;

public class KeyboardSounds : MonoBehaviour {


	public AudioClip[] clicks;

	void Update()
	{
		if( Input.anyKeyDown )
			audio.PlayOneShot( clicks[Random.Range(0, clicks.Length) ] );
	}
}
