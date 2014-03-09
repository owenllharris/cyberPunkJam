using UnityEngine;
using System.Collections;

public class CommandProcessor : MonoBehaviour 
{
	public GameObject Hero;
	public float movment = 0.5f;

	private AccessData heroAccessData;

	public void Start()
	{
		heroAccessData = Hero.GetComponent<AccessData>();
	}

	public void Command( string command )
	{
		command.ToLower();

		if( command.Contains("left") )
			move( -Vector2.right );
		else if ( command.Contains("right") )
			move ( Vector2.right );
		else if( command.Contains("forward") )
			move ( Vector2.up );
		else if( command.Contains("back") )
			move ( -Vector2.up );
		else if( command.Contains("access") )
			tryAccess();
		else
			Reporter.instance.ShowMessage("Unknown Command", true);
	}


	private void move( Vector3 direction )
	{
		RaycastHit2D hit = Physics2D.Raycast( Hero.transform.position + (direction * movment), direction, 0.1f );

		if( hit.collider == null )
		{
			Hero.transform.position += (direction * movment);
			heroAccessData.canAccess = false;
		}

		else if( hit.collider.isTrigger == true )
		{
			Hero.transform.position += (direction * movment);
			heroAccessData.canAccess = true;
		}
	}

	private void tryAccess()
	{
		heroAccessData.Access();
	}
}
