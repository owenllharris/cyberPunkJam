using UnityEngine;
using System.Collections;

public class AccessData : MonoBehaviour 
{
	public bool canAccess = false;

	public int CurrentData = 0;


	public void Access()
	{
		if( canAccess )
		{
			CurrentData++;
			ShowData.instance.updateText( CurrentData );
		}
	}

}
