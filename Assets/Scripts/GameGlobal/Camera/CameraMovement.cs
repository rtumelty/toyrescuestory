using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour 
{
	//*************************************************************//	
	private Transform _target;
	private float _countTimeToFollow;
	private GameObject staticCam;
	private bool camDropped = false;
	private float staticCamsX = 0;
	private GameObject defaultTarget;
	//*************************************************************//	
	private static CameraMovement _meInstance;
	public static CameraMovement getInstance ()
	{
		if ( _meInstance == null )
		{	
			if(Camera.main != null)
			{
				_meInstance = Camera.main.GetComponent < CameraMovement > ();
			}
			else
			{
				_meInstance = GameObject.Find("Camera").GetComponent < CameraMovement > ();
			}
		}
		
		return _meInstance;
	}

	void Start()
	{
		defaultTarget = GameObject.Find ("defaultCamPosition");
		if(Application.loadedLevelName == "TR01" )
		{
			print ("InStart");
			staticCam = GameObject.Find ("camHolderStatic");
			if(staticCamsX != null)
			{
				staticCamsX = staticCam.transform.position.x;
				staticCam.GetComponent <Camera> ().enabled = false;
			}
		}
	}
	//*************************************************************//
	
	public void followObjectForSeconds ( Transform toFollowObject, float time )
	{
		if ( GlobalVariables.TUTORIAL_MENU ) return;
		_countTimeToFollow = time;
		_target = toFollowObject;
	}
	//================================Daves Edit==================================
	public void dropCam ()
	{
		staticCam.GetComponent <Camera> ().enabled = true;
		GameObject.Find ("Camera").GetComponent <Camera> ().enabled = false;
		camDropped = true;
	}

	public void centreCam (float time)
	{
		if ( GlobalVariables.TUTORIAL_MENU ) return;
		_countTimeToFollow = time;
		_target = defaultTarget.transform;
	}

	public void resetCam ()
	{
		staticCam.GetComponent <Camera> ().enabled = false;
		GameObject.Find ("Camera").GetComponent <Camera> ().enabled = true;
	}
	//================================Daves Edit==================================
	void Update () 
	{
		if ( _countTimeToFollow > 0f )
		{
			_countTimeToFollow -= Time.deltaTime;
			transform.position = Vector3.Lerp ( transform.position, new Vector3 ( _target.position.x, transform.position.y, _target.position.z ), 0.04f );
		}
		//================================Daves Edit==================================
		if(Application.loadedLevelName == "TR01" )
		{
			if (transform.position.x >= staticCamsX && camDropped == false && staticCamsX != null) 
			{
				dropCam();
			}
		}
		//================================Daves Edit==================================
	}
}
