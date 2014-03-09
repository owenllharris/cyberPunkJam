using UnityEngine;
using System.Collections;

public class CheckForPlayer : MonoBehaviour {

	private GameObject player;
	private Patroll p;
	void Start () 
	{
		player = GameObject.Find("Hero");
		p = GetComponent<Patroll>();
	}
	

	void Update () 
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 100);

		if(hit.collider.name == "Hero")
		{
			Debug.Log("I can see him");

		}
		else
		{
			Debug.Log("Where is her?");
		}

		Debug.DrawRay(transform.position, player.transform.position - transform.position);
	}
}
