using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoToObject : MonoBehaviour 
{
	public Transform target;
	public float speed = 1f;

	public List<Vector2> path;

	public void goTo(Transform whereToGo)
	{
		path = NavMesh2D.GetPath(transform.position, whereToGo.position);
	}

	void Update()
	{

		if(path != null && path.Count != 0)
		{
			Debug.Log(Vector2.Distance(transform.position,path[0]));
			transform.position = Vector2.MoveTowards(transform.position, path[0],  speed * Time.deltaTime);

			if(Vector2.Distance(transform.position,path[0]) < 0.01f)
			{
				path.RemoveAt(0);
			}
		}
	}
}
