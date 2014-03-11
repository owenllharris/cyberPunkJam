using UnityEngine;
using System.Collections;

public class CheckForPlayer : MonoBehaviour {

	private GameObject player;
	private Patroll p;
	private GoToObject gto;

	public bool canSee = false;
	void Start () 
	{
		player = GameObject.Find("Hero");
		p = GetComponent<Patroll>();
		gto = GetComponent<GoToObject>();
	}
	

	void Update () 
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 100);

		if(hit.collider.name == "Hero")
		{

			p.patroll = false;

			if( canSee == false )
			{
				if( gto.path.Count == 0 )
					gto.goTo ( player.transform );


			}

			canSee = true;

		}
		else
		{
			canSee = false;
		}

		Debug.DrawRay(transform.position, player.transform.position - transform.position);
	}

	void checkAgain()
	{
		if( !canSee )
			gto.goTo(p.waypoints[0]);
	}
}
