using UnityEngine;
using System.Collections;

public class TimerControl : MonoBehaviour 
{
	//*************************************************************//	
	private TextMesh _myText;
	private bool _startCounting = false;
	private float _secondsPassed = 0f;
	private bool _timePassedAlready = false;
	//*************************************************************//	
	private static TimerControl _meInstance;
	public static TimerControl getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "Camera" ).transform.Find ( "UI" ).transform.Find ( "TimerPanel" ).GetComponent < TimerControl > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
		_myText = transform.Find ( "text" ).GetComponent < TextMesh > ();
	}
	
	public void startTimer ( bool isOn )
	{
		if ( GlobalVariables.TUTORIAL_MENU ) return;
		_startCounting = isOn;
		iTween.ScaleFrom ( gameObject, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.easeOutQuad, "scale", transform.localScale * 1.2f ));
	}
	
	public int getCurrentTime ()
	{
		return (int) _secondsPassed;
	}
	
	void Update () 
	{
		if ( ! _startCounting ) return;
		if ( LevelControl.CURRENT_LEVEL_CLASS == null ) return;

		_secondsPassed += Time.deltaTime;
		_myText.text = TimeScaleManager.getTimeString ((int) _secondsPassed );
		
		if (( ! _timePassedAlready ) && ( _secondsPassed > LevelControl.CURRENT_LEVEL_CLASS.star04 ))
		{
			_timePassedAlready = true;
			renderer.material.mainTexture = UIControl.getInstance ().textureUIClockTimePassed; 
		}
	}
}
