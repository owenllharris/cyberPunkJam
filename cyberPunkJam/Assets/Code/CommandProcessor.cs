using UnityEngine;
using System.Collections;

public class CommandProcessor : MonoBehaviour 
{
	public GameObject Hero;
	public GUIText helpGUI;
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
		else if( command.Contains("help") )
			help();
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
			heroAccessData.ap = null;
		}

		else if( hit.collider.isTrigger == true )
		{
			Hero.transform.position += (direction * movment);
			heroAccessData.canAccess = true;
			heroAccessData.ap = hit.collider.GetComponent<AccessPoint>();
		}
	}

	void help()
	{
		helpGUI.enabled = true;
	}

	private void tryAccess()
	{
		heroAccessData.Access();
	}
}
