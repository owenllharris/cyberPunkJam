using UnityEngine;
using System.Collections;

public class Patroll : MonoBehaviour {

	public bool loop = false;
	public bool patroll = true;

	public float speed = 1f;

	public Transform[] waypoints;

	private int nextWaypoint;
	private Vector2 direction;

	void Start()
	{
		SetWaypoint(0);
	}

	void Update()
	{
		if(patroll)
			transform.Translate( direction * speed * Time.deltaTime );
		if( Vector2.Distance( transform.position, waypoints[nextWaypoint].position ) < 0.05f )
		{
			atWaypoint();
		}
	}

	void atWaypoint()
	{
		SnapPosition();
		nextWaypoint++;

		if( nextWaypoint  >= waypoints.Length )
		{
			nextWaypoint = 0;
			if(!loop)
				System.Array.Reverse(waypoints);
		}

		SetWaypoint( nextWaypoint );
	}

	private void SetWaypoint(int wayPointNumber)
	{
		nextWaypoint = wayPointNumber;
		direction = ( waypoints[nextWaypoint].position - transform.position ).normalized;
	}

	private void SnapPosition()
	{
		Vector2 pos = transform.position;
		pos.x = Mathf.RoundToInt(pos.x);
		pos.y = Mathf.RoundToInt(pos.y);
		transform.position = pos;
	}
}
