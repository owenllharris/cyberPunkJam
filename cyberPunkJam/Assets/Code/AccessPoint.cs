using UnityEngine;
using System.Collections;

public class AccessPoint : MonoBehaviour 
{
	int currentData = 3;
	private GameObject[] aps;
	private AccessData ad;


	void Start()
	{
		aps = GameObject.FindGameObjectsWithTag("AccessPoint");
		ad = GameObject.Find("Hero").GetComponent<AccessData>();
	}

	public void Accessed()
	{
		currentData -- ;

		if( currentData == 0 )
		{
			foreach( GameObject g in aps )
				g.BroadcastMessage("enable");
			disable();
			ad.canAccess = false;

		}
	}

	void enable()
	{
		renderer.enabled = true;
		collider2D.enabled = true;
	}

	void disable()
	{
		renderer.enabled = false;
		collider2D.enabled = false;
	}

}
