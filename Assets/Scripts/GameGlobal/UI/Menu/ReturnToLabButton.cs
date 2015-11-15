using UnityEngine;
using System.Collections;

public class ReturnToLabButton : MonoBehaviour 
{
	//*************************************************************//	
	void OnMouseUp ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		//FlurryAnalytics.Instance ().LogEvent ( "Return to map button on - level " + LevelControl.SELECTED_LEVEL_NAME );
		if ( LevelControl.CURRENT_LEVEL_CLASS != null ) GoogleAnalytics.instance.LogScreen ( "Return to map button on - level " + ( LevelControl.CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_REGULAR_NODE ? "" : "B" ) + LevelControl.SELECTED_LEVEL_NAME );

		LoadingScreenControl.getInstance ().changeTipForGamePart ( GameGlobalVariables.LABORATORY );
		LoadingScreenControl.getInstance ().turnOnLoadingScreen ();
		
		GlobalVariables.POPUP_UI_SCREEN = false;
		
		GameGlobalVariables.Stats.NewResources.reset ();
		FLMissionRoomManager.AFTER_INTRO = false;
		Application.LoadLevel ( "FL00ChooseLevel" );
	}
}