using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIControl : MonoBehaviour 
{
	//*************************************************************//
	public static GameObject currentPopupUI;
	public static GameObject currentBlackOutUI;
	public static GameObject currentAttackBarUI;
	//*************************************************************//
	public Texture2D textureYesUp;
	public Texture2D textureYesDown;
	public Texture2D textureYesGreyedOutUp;
	public Texture2D textureYesGreyedOutDown;
	public Texture2D textureNoUp;
	public Texture2D textureNoDown;
	public Texture2D textureUIButtonSelectedUp;
	public Texture2D textureUIButtonSelectedDown;
	public Texture2D textureUIButtonNotSelectedUp;
	public Texture2D textureUIButtonNotSelectedDown;
	public Texture2D textureUIClockTimePassed;
	public Texture2D textureIconNext;
	public Texture2D textureIconForward;
	public Texture2D attackGreenBoxDown;
	public Texture2D attackGreenBoxUp;
	public Texture2D[] powerBarTextures;
	public Material transparentMaterial;
	//*************************************************************//	
	public delegate bool ButtonCallBack ();
	public delegate bool ButtonCharacterCallBack ( CharacterData character );
	//*************************************************************//	
	private GameObject _buttonMenuPrefab;
	private GameObject _buttonPrefab;
	private List < GameObject > _buttonsCharacters;
	private GameObject _buttonRedirector;
	private Vector3 _buttonRedirectorInitialScale;
	private float _countTimeOutConnection;
	private GameObject _characterBoxComboUIPrefab;
	private GameObject _blackOutUIPrefab;
	private GameObject _timerPanel;
	//*************************************************************//	
	private static UIControl _meInstance;
	public static UIControl getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "Camera" ).transform.Find ( "UI" ).GetComponent < UIControl > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//	
	void Awake () 
	{
		_timerPanel = transform.Find ( "TimerPanel" ).gameObject;
		
		_blackOutUIPrefab = ( GameObject ) Resources.Load ( "UI/PopupBlackOutBackUI" );
		_characterBoxComboUIPrefab = ( GameObject ) Resources.Load ( "UI/tutorialComboUIChar01");
		_countTimeOutConnection = 15f;
		_buttonsCharacters = new List < GameObject > ();
		_buttonMenuPrefab = ( GameObject ) Resources.Load ( "UI/ButtonPrefab" );
		_buttonPrefab = ( GameObject ) Resources.Load ( "UI/Button" );
		_buttonRedirector = transform.Find ( "RedirectionButton" ).gameObject;
		_buttonRedirectorInitialScale = VectorTools.cloneVector3 ( _buttonRedirector.transform.localScale );
	}
	
	public GameObject createPopup ( GameObject popupPrefab, bool noCancelLayer = false, string onCloseCreatePopup = "NULL" )
	{
		if ( UIControl.currentPopupUI != null ) return null;
		
		GlobalVariables.POPUP_UI_SCREEN = true;
		blockClicksForAMomentAfterUIClicked ();
		
		GameObject popup = ( GameObject ) Instantiate ( popupPrefab, Camera.main.transform.position + Vector3.down * 4f + Vector3.forward * 12f, popupPrefab.transform.rotation );
		popup.transform.parent = Camera.main.transform;
		popup.AddComponent < PopupDelayer > ();
		if ( popup.transform.Find ( "closeButton" ) != null ) popup.transform.Find ( "closeButton" ).GetComponent < ScreenCloseButton > ().onCloseCreatePopupName = onCloseCreatePopup;
		UIControl.currentPopupUI = popup;
		
		GameObject blackOutScreen = ( GameObject ) Instantiate ( _blackOutUIPrefab, Camera.main.transform.position + Vector3.down * 5.5f, _blackOutUIPrefab.transform.rotation );
		blackOutScreen.transform.parent = Camera.main.transform;
		blackOutScreen.GetComponent < BlackOutTouchControl > ().onCloseCreatePopupName = onCloseCreatePopup;
		UIControl.currentBlackOutUI = blackOutScreen;
		
		if ( noCancelLayer ) blackOutScreen.collider.enabled = false;
		
		return popup;
	}
	
	public void manageTimer ( bool isOn )
	{
		if ( isOn )
		{
			_timerPanel.transform.localPosition = new Vector3 ( 0.083f, _timerPanel.transform.localPosition.y, -0.04f );
		}
		else
		{
			_timerPanel.transform.localPosition = new Vector3 ( 0.18f, _timerPanel.transform.localPosition.y, 2f );
		}
	}
	
	public void createButtonsCharacters ( List < CharacterData > charactersData )
	{
		for ( int i = 0; i < charactersData.Count; i++ )
		{
			if ( charactersData.Count == 3 )
			{
				_buttonsCharacters.Add (( GameObject ) Instantiate ( _buttonMenuPrefab, transform.position + Vector3.left * ( charactersData.Count - i - 1 ) * 2.1f + Vector3.up + Vector3.left * 0.25f + Vector3.forward * -0.04f, _buttonMenuPrefab.transform.rotation ));
			}
			else if ( charactersData.Count == 2 )
			{
				_buttonsCharacters.Add (( GameObject ) Instantiate ( _buttonMenuPrefab, transform.position + Vector3.left * ( charactersData.Count - i - 1 ) * 2.1f + Vector3.left * 2.5f + Vector3.up + Vector3.left * 0.25f + Vector3.forward * -0.04f, _buttonMenuPrefab.transform.rotation ));
			}
			else if ( charactersData.Count == 1 )
			{
				_buttonsCharacters.Add (( GameObject ) Instantiate ( _buttonMenuPrefab, transform.position + Vector3.left * ( charactersData.Count - i - 1 ) * 2.1f + Vector3.left * 4.15f + Vector3.up + Vector3.left * 0.25f + Vector3.forward * -0.04f, _buttonMenuPrefab.transform.rotation ));
			}

			CharacterButton currentCharacterButton = _buttonsCharacters[_buttonsCharacters.Count - 1].AddComponent < CharacterButton > ();
			
			switch ( charactersData[i].myID )
			{
				case GameElements.CHAR_CORA_1_IDLE:
					_buttonsCharacters[_buttonsCharacters.Count - 1].transform.Find ( "characterIcon" ).renderer.material.mainTexture = LevelControl.getInstance ().gameElements[GameElements.UI_CORA_ICON];
					break;
				case GameElements.CHAR_MADRA_1_IDLE:
					_buttonsCharacters[_buttonsCharacters.Count - 1].transform.Find ( "characterIcon" ).renderer.material.mainTexture = LevelControl.getInstance ().gameElements[GameElements.UI_MADRA_ICON];
					break;
				case GameElements.CHAR_FARADAYDO_1_IDLE:
					_buttonsCharacters[_buttonsCharacters.Count - 1].transform.Find ( "characterIcon" ).renderer.material.mainTexture = LevelControl.getInstance ().gameElements[GameElements.UI_FARADAYDO_ICON];
					break;
			}
			
			currentCharacterButton.setCharacterData ( charactersData[i] );
			charactersData[i].myCharacterButton = currentCharacterButton;
			_buttonsCharacters[_buttonsCharacters.Count - 1].transform.parent = this.transform;
		}
	}
	
	public void updateWithAdditionlMovesCharactersButtons ( int addMoves )
	{
		foreach ( CharacterData character in LevelControl.getInstance ().charactersOnLevel )
		{
			if ( character.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] + addMoves > character.myCharacterButton.initialPower )
			{
				character.myCharacterButton.initialPower = character.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] + addMoves;
			}
		}
	}
	
	public GameObject createButton ( string name, float x, float z, float y, Vector3 scale, Texture2D texture, Texture2D textureDown, ButtonCallBack callBack, GameObject attachToGameObject = null, bool destroyOnClick = true )
	{
		GameObject buttonInstant = ( GameObject ) Instantiate ( _buttonPrefab, Vector3.zero, _buttonPrefab.transform.rotation );
		buttonInstant.name = name;
		if ( attachToGameObject == null ) buttonInstant.transform.parent = Camera.main.transform;
		else buttonInstant.transform.parent = attachToGameObject.transform;
		
		buttonInstant.transform.localPosition = new Vector3 ( x, z, y );
		
		if ( texture == null )
		{
			buttonInstant.renderer.material = transparentMaterial;
		}
		else
		{
			buttonInstant.renderer.material.mainTexture = texture;
		}
		
		buttonInstant.transform.localScale = scale;
		ButtonManager currentButtonManager = buttonInstant.AddComponent < ButtonManager > ();
		currentButtonManager.myCallBack = callBack;
		currentButtonManager.destroyOnClick = destroyOnClick;
		
		OnMouseDownButtonEffectControl currentOnMouseDownButtonEffectControl = buttonInstant.AddComponent < OnMouseDownButtonEffectControl > ();
		currentOnMouseDownButtonEffectControl.myOnMouseUpTexture = texture;
		currentOnMouseDownButtonEffectControl.myOnMouseDownTexture = textureDown;
		
		return buttonInstant;
	}
	
	public GameObject createButtonCharacter ( float x, float z, float y, Vector3 scale, Texture2D texture, ButtonCharacterCallBack callBack, CharacterData character )
	{
		GameObject buttonInstant = ( GameObject ) Instantiate ( _buttonPrefab, Vector3.zero, _buttonPrefab.transform.rotation );
		buttonInstant.transform.parent = Camera.main.transform;
		buttonInstant.transform.localPosition = new Vector3 ( x, z, y );
		
		if ( texture == null )
		{
			buttonInstant.renderer.material = transparentMaterial;
		}
		else
		{
			buttonInstant.renderer.material.mainTexture = texture;
		}
		
		buttonInstant.transform.localScale = scale;
		ButtonCharacterManager currentButtonManager = buttonInstant.AddComponent < ButtonCharacterManager > ();
		
		currentButtonManager.myCallBackCharacter = callBack;
		currentButtonManager.character = character;
		
		return buttonInstant;
	}
	
	public void showRedirectorButtonUnderMe ( Vector3 positionOfButton, bool isOn )
	{
		if ( isOn && ! LevelControl.getInstance ().redirectorsAreNotNeeded )
		{
			_buttonRedirector.renderer.material.mainTexture = UIControl.getInstance ().textureUIButtonSelectedUp;
			_buttonRedirector.GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseUpTexture = UIControl.getInstance ().textureUIButtonSelectedUp;
			_buttonRedirector.GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseDownTexture = UIControl.getInstance ().textureUIButtonSelectedDown;
			
			_buttonRedirector.transform.localPosition = new Vector3 ( positionOfButton.x, _buttonRedirector.transform.localPosition.y, _buttonRedirector.transform.localPosition.z );
			iTween.MoveTo ( _buttonRedirector, iTween.Hash ( "time", 0.25f, "easetype", iTween.EaseType.easeOutCubic, "position", positionOfButton + Vector3.back * 1.1f + Vector3.down, "islocal", true ));
		}
		else
		{
			_buttonRedirector.renderer.material.mainTexture = UIControl.getInstance ().textureUIButtonNotSelectedUp;
			_buttonRedirector.GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseUpTexture = UIControl.getInstance ().textureUIButtonNotSelectedUp;
			_buttonRedirector.GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseDownTexture = UIControl.getInstance ().textureUIButtonNotSelectedDown;
			
			_buttonRedirector.transform.localScale = VectorTools.cloneVector3 ( _buttonRedirectorInitialScale );
			iTween.MoveTo ( _buttonRedirector, iTween.Hash ( "time", 0.25f, "easetype", iTween.EaseType.easeInCubic, "position", positionOfButton + Vector3.forward * 3f + Vector3.down, "islocal", true ));
		}
	}
	
	public void LateUpdate () 
	{
		if ( GlobalVariables.START_SEQUENCE || GlobalVariables.TOY_LAZARUS_SEQUENCE ) return;
		
		float uiSizeFactor = Camera.main.orthographicSize / 4.5f;
		transform.localScale = new Vector3 ( 24f * uiSizeFactor, 1f, 1f * uiSizeFactor );
		transform.localPosition = new Vector3 ( 0f, Camera.main.orthographicSize - 0.5f * uiSizeFactor, 6f );
	}
	
	public void blockClicksForAMomentAfterUIClicked ()
	{
		GlobalVariables.UI_CLICKED = true;
		StopCoroutine ( "unblockAfterDelay" );
		StartCoroutine ( "unblockAfterDelay" );
	}
	
	private IEnumerator unblockAfterDelay ()
	{
		yield return new WaitForSeconds ( 0.25f );
		GlobalVariables.UI_CLICKED = false;	
	}
	
	void OnGUI ()
	{
		if ( GameGlobalVariables.RELEASE ) return;
		if ( GlobalVariables.LOADING_SAVING_MENU )
		{
			_countTimeOutConnection -= Time.deltaTime;
			
			if ( _countTimeOutConnection <= 0f )
			{
				LevelControl.getInstance ().loadLevelLocalToThisGrid ( 1, LevelControl.getInstance ().levelGrid[LevelControl.GRID_LAYER_NORMAL] );
				GlobalVariables.LOADING_SAVING_MENU = false;
				return;
			}
			
			GUI.TextArea ( new Rect ( 10f, 70f, Screen.width - 20f, Screen.height - 20f ), "Loading Levels From Online Database... Please Wait" );
		}
	}
	
	public void createCharacterBox ( int characterID )
	{
		GlobalVariables.CHARACTER_BOX = true;
		GameObject tutorialUIComboObject = ( GameObject ) Instantiate ( _characterBoxComboUIPrefab, Vector3.zero, _characterBoxComboUIPrefab.transform.rotation );
		tutorialUIComboObject.transform.parent = Camera.main.transform;
		tutorialUIComboObject.transform.localPosition = new Vector3 ( 0f, -2.5f, 2f );
		//iTween.ScaleFrom ( tutorialUIComboObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutExpo, "scale", Vector3.zero, "islocal", true ));
		
		tutorialUIComboObject.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = "character_out_of_moves";
		if ( tutorialUIComboObject.transform.Find ( "speaker" ) != null ) 
		{
			tutorialUIComboObject.transform.Find ( "speaker" ).renderer.material.mainTexture = LevelControl.getInstance ().gameElements[characterID];
			if ( characterID == GameElements.CHAR_CORA_1_IDLE )
			{
				tutorialUIComboObject.transform.Find ( "speaker" ).transform.localScale *= 1.5f;
				tutorialUIComboObject.transform.Find ( "speaker" ).transform.position += Vector3.forward * 0.7f;
			}
		}
		
		tutorialUIComboObject.transform.Find ( "frame" ).gameObject.AddComponent < FrameTapControl > ();
		tutorialUIComboObject.transform.Find ( "tapText" ).GetComponent < TextMesh > ().text = "Tap Here!";
		
		TutorialsManager.getInstance ().setAlphaToZero ( tutorialUIComboObject );
	}

	public GameObject getFirstCharacterButton ()
	{
		return _buttonsCharacters[0];
	}
}
