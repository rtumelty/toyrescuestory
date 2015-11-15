using UnityEngine;
using System.Collections;

public class RestartLevelButton : MonoBehaviour 
{
	void OnMouseUp ()
	{
		gameObject.collider.enabled = false;
		SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			if ( GameGlobalVariables.RELEASE )
			{
				LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.RESCUE );
				LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
				Application.LoadLevel ( "00ChooseLevel" );
			}
			else
			{
				LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.RESCUE );
				LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
				Application.LoadLevel ( "01" );
			}
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.MINING );
			LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
			
			Application.LoadLevel ( "MN01" );
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.TRAIN )
		{
			LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.TRAIN );
			LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
			
			Application.LoadLevel ( "TR01" );
		}
	}
}
