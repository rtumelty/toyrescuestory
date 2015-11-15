using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MNDestroyableObjectComponent : ObjectTapControl 
{
	//*************************************************************//	
	private Main.HandleChoosenCharacter _myHandleChoosenCharacter;
	private IComponent _myIComponent;
	private int _hitNumber = 0;
	private bool clickedAfter = false;
	private GameObject _bozObject;
	private int myID;
	//*************************************************************//	
	void Start () 
	{
		_myIComponent = gameObject.GetComponent < IComponent > ();
		_myHandleChoosenCharacter = handleDemolish;
		myID = transform.parent.Find("tile").GetComponent < IComponent > ().myID;
		if(myID == 71 || myID == 74 || myID == 92)
		{
			MNLevelControl.getInstance().objectsOfValue += 1;
		}
	}
	
	void OnMouseUp ()
	{
		if ( MNGlobalVariables.TUTORIAL_MENU ) return;
		CharacterData selectedCharacter = MNLevelControl.getInstance ().getSelectedCharacter ();

		if ( Array.IndexOf ( GameElements.BUILDERS, selectedCharacter.myID ) != -1 )
		{
			if ( GetComponent < TipOnClickComponent > ()) GetComponent < TipOnClickComponent > ().deActivate ();
		}
		else
		{
			GameObject faradaydoObject = MNLevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_FARADAYDO_1_IDLE );

			if ( faradaydoObject != null )
			{
				faradaydoObject.transform.Find ( "tile" ).SendMessage ( "handleTouched" );
			}
			else
			{
				GameObject bozObject = MNLevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_BOZ_1_IDLE );
				bozObject.transform.Find ( "tile" ).SendMessage ( "handleTouched" );
			}
		}

		if ( MNGlobalVariables.checkForMenus ()) return;
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if ( _alreadyTouched ) return;
		if ( MNGlobalVariables.TUTORIAL_MENU )
		{
			clickedAfter = true;
		}
		CharacterData selectedCharacter = MNLevelControl.getInstance ().getSelectedCharacter ();
		if ( selectedCharacter.interactAction ) return;
		if ( Array.IndexOf ( GameElements.BUILDERS, selectedCharacter.myID ) != -1 )
		{
			_alreadyTouched = true;
		}

		if ( SelectedComponenent.CURRENT_SELECTED_OBJECT ) SelectedComponenent.CURRENT_SELECTED_OBJECT.setSelected ( false );

		if ( MNGlobalVariables.TUTORIAL_MENU ) gameObject.GetComponent < SelectedComponenent > ().setSelected ( true,true );
		else gameObject.GetComponent < SelectedComponenent > ().setSelected ( true );
		MNMain.getInstance ().interactWithCurrentCharacter ( _myHandleChoosenCharacter, CharacterData.CHARACTER_ACTION_TYPE_DEMOLISH_RATE, _myIComponent );
	}
	
	public void handleDemolish ( CharacterData characterToBuild, bool onlyUnblocking = false )
	{
		if ( onlyUnblocking )
		{
			_alreadyTouched = false;
			return;
		}
		if(characterToBuild.myID == 40)
		{
			SoundManager.getInstance ().playSound ( SoundManager.DESTROYABLE_DEMOLISH, _myIComponent.myID );
		}
		StartCoroutine ( "startBuildProcedure", characterToBuild );
	}
	
	private IEnumerator startBuildProcedure ( CharacterData characterToBuild )
	{	
		int differenceX = _myIComponent.position[0] - characterToBuild.position[0];
		
		characterToBuild.interactAction = true;

		bool isBoz = false;

		if ( characterToBuild.myID == GameElements.CHAR_BOZ_1_IDLE )
		{
			isBoz = true;

			_bozObject = MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][characterToBuild.position[0]][characterToBuild.position[1]];
			_bozObject.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().borrowTileMarkerForThisObject ( transform.parent.gameObject );
			iTween.MoveTo ( _bozObject, iTween.Hash ( "time", 0.2f, "easetype", iTween.EaseType.linear, "position", this.transform.position + Vector3.forward * 1.5f + Vector3.up, "oncompletetarget", this.gameObject, "oncomplete", "finishedJumpOnRockAbove" ));
			_bozObject.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.JUMP_TO_DRILL, 0.3f, differenceX );
			SoundManager.getInstance ().playSound ( SoundManager.BOZ_JUMP_TO_DRILL );
		}
		else
		{
			MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][characterToBuild.position[0]][characterToBuild.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.INTERACT_02_ANIMATION, 1.6f, differenceX );
		}

		gameObject.GetComponent < SelectedComponenent > ().electrickShock ();

		if ( isBoz )
		{
			yield return new WaitForSeconds ( 0.75f );
			_bozObject.transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.INTERACT_UP_ANIMATION );
			SoundManager.getInstance ().playSound ( SoundManager.BOZ_DRILLING );
			yield return new WaitForSeconds ( 0.85f );
		}
		else
		{
			yield return new WaitForSeconds ( 1.6f );
		}

		if ( isBoz )
		{
			_bozObject.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().borrowTileMarkerForThisObject ( _bozObject );

			MNGridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, characterToBuild.position[0], characterToBuild.position[1], _bozObject, characterToBuild.myID );
			MNGridReservationManager.getInstance ().fillTileWithMe ( GameElements.EMPTY, _myIComponent.position[0], _myIComponent.position[1], this.transform.parent.gameObject, _myIComponent.myID );
			MNGridReservationManager.getInstance ().fillTileWithMe ( characterToBuild.myID, _myIComponent.position[0], _myIComponent.position[1], _bozObject, characterToBuild.myID );

			characterToBuild.position = ToolsJerry.cloneTile ( _myIComponent.position );
			_bozObject.transform.Find ( "tile" ).GetComponent < IComponent > ().position = ToolsJerry.cloneTile ( _myIComponent.position );

			iTween.MoveTo ( _bozObject, iTween.Hash ( "time", 0.2f, "easetype", iTween.EaseType.linear, "position", new Vector3 ((float) _myIComponent.position[0], (float) ( MNLevelControl.LEVEL_HEIGHT - _myIComponent.position[1] ), (float) _myIComponent.position[1] - 0.5f )));
			SoundManager.getInstance ().playSound ( SoundManager.BOZ_DESTROYED_ROCK );
			yield return new WaitForSeconds ( 0.3f );
		}

		if ( MNGlobalVariables.TUTORIAL_MENU && clickedAfter == true || isBoz )
		{
			_hitNumber = 3;
		}
		else _hitNumber++;

		if ( Array.IndexOf ( GameElements.CRACKED_ROCKS_OBJECTS, _myIComponent.myID ) != -1 )
		{
			_hitNumber = 3;
		}

		Texture2D myTextureDestroyedLevel1 = null;
		switch ( _hitNumber )
		{
			case 1:
				switch ( _myIComponent.myID )
				{
					case GameElements.ENVI_CRACKED_1_ALONE:
						myTextureDestroyedLevel1 = MNLevelControl.getInstance ().gameElements[GameElements.ENVI_CRACKED_2_ALONE];
						break;
					case GameElements.ENVI_CRACKED_1_LEFT:
						myTextureDestroyedLevel1 = MNLevelControl.getInstance ().gameElements[GameElements.ENVI_CRACKED_2_LEFT];
						break;
					case GameElements.ENVI_CRACKED_1_MID:
						myTextureDestroyedLevel1 = MNLevelControl.getInstance ().gameElements[GameElements.ENVI_CRACKED_2_MID];
						break;
					case GameElements.ENVI_CRACKED_1_RIGHT:
						myTextureDestroyedLevel1 = MNLevelControl.getInstance ().gameElements[GameElements.ENVI_CRACKED_2_RIGHT];
						break;
					
					case GameElements.ENVI_METAL_1_ALONE:
						myTextureDestroyedLevel1 = MNLevelControl.getInstance ().gameElements[GameElements.ENVI_METAL_2_ALONE];
						break;
					case GameElements.ENVI_PLASTIC_1_ALONE:
						myTextureDestroyedLevel1 = MNLevelControl.getInstance ().gameElements[GameElements.ENVI_PLASTIC_2_ALONE];
						break;
					case GameElements.ENVI_TECHNOEGG_1_ALONE:
						myTextureDestroyedLevel1 = MNLevelControl.getInstance ().gameElements[GameElements.ENVI_TECHNOEGG_2_ALONE];
						break;
				}
				
				renderer.material.mainTexture = myTextureDestroyedLevel1;
				break;
			case 2:
				switch ( _myIComponent.myID )
				{
					case GameElements.ENVI_CRACKED_1_ALONE:
						myTextureDestroyedLevel1 = MNLevelControl.getInstance ().gameElements[GameElements.ENVI_CRACKED_3_ALONE];
						break;
					case GameElements.ENVI_CRACKED_1_LEFT:
						myTextureDestroyedLevel1 = MNLevelControl.getInstance ().gameElements[GameElements.ENVI_CRACKED_3_LEFT];
						break;
					case GameElements.ENVI_CRACKED_1_MID:
						myTextureDestroyedLevel1 = MNLevelControl.getInstance ().gameElements[GameElements.ENVI_CRACKED_3_MID];
						break;
					case GameElements.ENVI_CRACKED_1_RIGHT:
						myTextureDestroyedLevel1 = MNLevelControl.getInstance ().gameElements[GameElements.ENVI_CRACKED_3_RIGHT];
						break;
					case GameElements.ENVI_METAL_1_ALONE:
						myTextureDestroyedLevel1 = MNLevelControl.getInstance ().gameElements[GameElements.ENVI_METAL_3_ALONE];
						break;
					case GameElements.ENVI_PLASTIC_1_ALONE:
						myTextureDestroyedLevel1 = MNLevelControl.getInstance ().gameElements[GameElements.ENVI_PLASTIC_3_ALONE];
						break;
					case GameElements.ENVI_TECHNOEGG_1_ALONE:
						myTextureDestroyedLevel1 = MNLevelControl.getInstance ().gameElements[GameElements.ENVI_TECHNOEGG_3_ALONE];
						break;
				}
				
				renderer.material.mainTexture = myTextureDestroyedLevel1;
				break;
			case 3:
				break;
		}
		
		switch ( _myIComponent.myID )
		{
			case GameElements.ENVI_METAL_1_ALONE:
			print (_hitNumber);
//			print ("boosh1");
				Instantiate ( MNMain.getInstance ().rockParticles, transform.root.position + Vector3.up, Quaternion.identity );
				int numberOfRandomResourcesMetal = UnityEngine.Random.Range ( MNMiningResourcesControl.MINIMUM_METAL_RESOURCES, MNMiningResourcesControl.MAXIMUM_METAL_RESOURCES );
				if ( isBoz ) numberOfRandomResourcesMetal *= 3;
				MNMiningResourcesControl.getInstance ().createResourcesOnLevelAroundPosition ( GameElements.ICON_METAL, numberOfRandomResourcesMetal, _myIComponent.position );
				break;
			case GameElements.ENVI_PLASTIC_1_ALONE:
			print (_hitNumber);
//			print ("boosh2");
				Instantiate ( MNMain.getInstance ().rockParticles, transform.root.position + Vector3.up, Quaternion.identity );
				int numberOfRandomResourcesPlastic = UnityEngine.Random.Range ( MNMiningResourcesControl.MINIMUM_PLASTIC_RESOURCES, MNMiningResourcesControl.MAXIMUM_PLASTIC_RESOURCES );
				if ( isBoz ) numberOfRandomResourcesPlastic *= 3;
				MNMiningResourcesControl.getInstance ().createResourcesOnLevelAroundPosition ( GameElements.ICON_PLASTIC, numberOfRandomResourcesPlastic, _myIComponent.position );
				break;
			case GameElements.ENVI_TECHNOEGG_1_ALONE:
				Instantiate ( MNMain.getInstance ().tentacleParticles, transform.root.position + Vector3.up, Quaternion.identity );
				if ( _hitNumber == 3 )
				{
					int numberOfRandomResourcesVines = UnityEngine.Random.Range ( MNMiningResourcesControl.MINIMUM_VINES_RESOURCES, MNMiningResourcesControl.MAXIMUM_VINES_RESOURCES );
					MNMiningResourcesControl.getInstance ().createResourcesOnLevelAroundPosition ( GameElements.ICON_VINES, numberOfRandomResourcesVines, _myIComponent.position );

					if ( UnityEngine.Random.Range ( 0, 2 ) < 1 )
					{
						MNMiningResourcesControl.getInstance ().createResourcesOnLevelAroundPosition ( GameElements.ICON_TECHNOSEED, 1, _myIComponent.position );
					}
				}
				break;
			default:
			{
			//print (_hitNumber);
			//print ("boosh3");
				Instantiate ( MNMain.getInstance ().rockParticles, transform.root.position + Vector3.up, Quaternion.identity );
			}
				break;
		}

		characterToBuild.interactAction = false;
		
		gameObject.GetComponent < SelectedComponenent > ().setSelected ( false );
		
		MNLevelControl.getInstance ().gameElementsOnLevel[MNLevelControl.GRID_LAYER_NORMAL][characterToBuild.position[0]][characterToBuild.position[1]].transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
		
		MNLevelControl.getInstance ().totalRocksPartsCleared++;
		
		if ( _hitNumber == 3 )
		{
			switch ( _myIComponent.myID )
			{
			case GameElements.ENVI_METAL_1_ALONE:
				break;
			case GameElements.ENVI_PLASTIC_1_ALONE:
				MNLevelControl.getInstance ().totalPlasticOreRocksDestroyed++;
				break;
			}

			MNMain.getInstance ().handleObjectDestory ( _myIComponent.myID, transform.root.gameObject );
		}


		
		_alreadyTouched = false;
	}

	private void finishedJumpOnRockAbove ()
	{
		iTween.MoveTo ( _bozObject, iTween.Hash ( "time", 0.1f, "easetype", iTween.EaseType.linear, "position", this.transform.position + Vector3.forward * 0.5f + Vector3.up ));
	}
	public void OnDestroy ()
	{
		if((myID == 71 || myID == 74 || myID == 92) && MNLevelControl.getInstance () != null)
		{
			MNLevelControl.getInstance ().destroyedObjectsOfValue += 1;
		

			if ( MNLevelControl.getInstance ().destroyedObjectsOfValue >= MNLevelControl.getInstance().objectsOfValue && MNLevelControl.getInstance () != null)
			{
				print ("Step 1");
				MNMain.getInstance(). StartCoroutine("waitForInactiveCharsForPopUp");
			}
		}
	}
}
