using UnityEngine;
using System.Collections;

public class AccessData : MonoBehaviour 
{
	public bool canAccess = false;

	public int CurrentData = 0;

	public AccessPoint ap;

	public void Access()
	{
		if( canAccess )
		{
			CurrentData++;
			ShowData.instance.updateText( CurrentData );
			Reporter.instance.ShowMessage("new data verified", true);
			if(ap != null )
				ap.Accessed();
		}
		else
		{
			Reporter.instance.ShowMessage("no access point detected", true);
		}
	}

}
