using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Enemy : MonoBehaviour 
{
	public Transform player;
	public float PatrollSpeed;
	public float ChaseSpeed;

	public List<Vector2> path = new List<Vector2>();
	public List<Transform> waypoints;

	public enum State { patroll, createPath, chase, goBack };
	public State currentState = State.patroll;

	public bool canSeePlayer = false;
	public bool loop = false;

	private Vector2 direction;
	private int nextWaypoint = 0;


	void Start()
	{
		SetWaypoint(0);
	}

	void Update()
	{
		switch( currentState )
		{
		case State.patroll:
			doPatroll();
			break;
		case State.createPath:
			createPath();
			break;
		case State.chase:
			doChase();
			break;
		case State.goBack:
			doGoBack();
			break;
		}

		canSeePlayer = CheckForPlayer();
	}

	bool CheckForPlayer()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 100);
		
		if(hit.collider.name == "Hero")
			return true;
		else
			return false;
	}

	void doPatroll()
	{
		transform.Translate( direction * PatrollSpeed * Time.deltaTime );

		if( Vector2.Distance( transform.position, waypoints[nextWaypoint].position ) < 0.02f )
		{
			atWaypoint();
		}

		if( canSeePlayer )
			StartCoroutine( createPath() );
	}

	void doGoBack()
	{
		if(path != null && path.Count != 0)
		{
			transform.position = Vector2.MoveTowards(transform.position, path[0], ChaseSpeed * Time.deltaTime);
			if(Vector2.Distance(transform.position,path[0]) < 0.01f)
			{
				path.RemoveAt(0);
			}
		}
		else if( path.Count == 0 )
		{
			StopCoroutine( "createPath" );
			currentState = State.patroll;
		}

	}

	IEnumerator createPath()
	{
		Vector2 target;
		while (true)
		{
	
			if( canSeePlayer )
			{
				target = player.position;
				currentState = State.chase;
			}
			else
			{
				target = waypoints[nextWaypoint].position;
				currentState = State.goBack;
			}

			path = NavMesh2D.GetSmoothedPath(transform.position, target);

			yield return new WaitForSeconds (0.1f);
		}
	}

	void doChase()
	{
		if(path != null && path.Count != 0)
		{
			transform.position = Vector2.MoveTowards(transform.position, path[0], ChaseSpeed * Time.deltaTime);
			if(Vector2.Distance(transform.position,path[0]) < 0.01f)
			{
				path.RemoveAt(0);
			}
		}

		if( Vector2.Distance(transform.position, player.position) < 0.4f )
		{
			// GAME OVER
			// TODO break this out into its own script

			Reporter.instance.ShowMessage("GAME OVER", false);
			RecieveText.instance.enabled = false;
			KeyboardSounds.instance.enabled = false;
			this.enabled = false;
		}
	}

	void atWaypoint()
	{
		SnapPosition();
		nextWaypoint++;
		
		if( nextWaypoint  >= waypoints.Count )
		{
			nextWaypoint = 0;
			if(!loop)
				waypoints.Reverse();
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
