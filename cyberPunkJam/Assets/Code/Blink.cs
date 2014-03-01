using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour 
{
	public float blinkRate = 0.1f;

	void Start()
	{
		InvokeRepeating("blink", blinkRate, blinkRate);
	}

	void blink()
	{
		renderer.enabled = !renderer.enabled;
	}

}
