using UnityEngine;
using System.Collections;

public class CharacterReloactionComponent : MonoBehaviour 
{
	//*************************************************************//
	public delegate bool HandleRelocate ( Vector3 position );
	//*************************************************************//	
	public bool blockMove;
	//*************************************************************//	
	private IComponent _myIComponent;
	private bool _waitBeforeProcceedHandleTouched = false;
	//*************************************************************//

	IEnumerator Start () 
	{
		_myIComponent = gameObject.GetComponent < IComponent > ();
		
		yield return new WaitForSeconds ( 0.05f );
		
		if ( _myIComponent.myCharacterData.myID == GameElements.CHAR_FARADAYDO_1_IDLE )
		{
			//===========================Daves Edit==================================
			//This was causing undesired SFX on start.
			//gameObject.SendMessage ( "handleTouched" );
			Main.getInstance ().unselectCurrentCharacter ();
			gameObject.GetComponent < SelectedComponenent > ().setSelectedCharacter ( true,true );
			Main.getInstance ().handleCharacterChosen ( _myIComponent.myCharacterData, handleRelocateToPosition );
			//===========================Daves Edit==================================
		}
	}
	
	public void OnMouseUp ()
	{
		if ( GlobalVariables.checkForMenus ()) return;
		handleTouched ();
	}

	private void handleTouchedFromStart ()
	{
		Main.getInstance ().unselectCurrentCharacter ();
		gameObject.GetComponent < SelectedComponenent > ().setSelectedCharacter ( true, true );
		Main.getInstance ().handleCharacterChosen ( _myIComponent.myCharacterData, handleRelocateToPosition );
	}

	private void handleTouched ()
	{
		if ( _waitBeforeProcceedHandleTouched ) return;
		_waitBeforeProcceedHandleTouched = true;
		StartCoroutine ( "waitBeforeUnlockHandleTouched" );

		Main.getInstance ().unselectCurrentCharacter ();
		gameObject.GetComponent < SelectedComponenent > ().setSelectedCharacter ( true );
		Main.getInstance ().handleCharacterChosen ( _myIComponent.myCharacterData, handleRelocateToPosition );
	}

	private IEnumerator waitBeforeUnlockHandleTouched ()
	{
		yield return new WaitForSeconds ( 0.5f );
		_waitBeforeProcceedHandleTouched = false;
	}

	private bool handleRelocateToPosition ( Vector3 position )
	{
		if ( blockMove ) return false;
		if ( _myIComponent.myCharacterData.blocked ) return false;
		
		if ( GlobalVariables.checkForMenus ())
		{
			if ( GlobalVariables.TUTORIAL_MENU )
			{
				if ( TutorialsManager.getInstance ().getCurrentTutorialStep ().type == TutorialsManager.TUTORIAL_OBJECT_TYPE_TAP_SCREEN || TutorialsManager.getInstance ().getCurrentTutorialStep ().type == TutorialsManager.TUTORIAL_OBJECT_TYPE_MOVE_CHARACTER_BY_TAP )
				{

				}
				else return false;
			}
			else return false;
		}

		int[] targetTile = new int[2] { Mathf.RoundToInt ( position.x ), Mathf.RoundToInt ( position.z )};
		int[][] returnedPath = AStar.search ( _myIComponent.position, targetTile, false, _myIComponent.myID, this.transform.root.gameObject );
		
		gameObject.GetComponent < SelectedComponenent > ().resetObject ();
		
		MoveComponent moveComponent = gameObject.transform.root.GetComponent < MoveComponent > ();
		moveComponent.initMove ( returnedPath, targetTile, false, _myIComponent.myCharacterData, null );
		
		return true;
	}
}
