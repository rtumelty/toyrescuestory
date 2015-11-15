using UnityEngine;
using System.Collections;

public class TapUnlockWithBuyButtonControl : MonoBehaviour 
{
	private bool _unlocked = false;

	void OnMouseUp ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		handleTouched ();
	}

	void Update ()
	{
		if ( _unlocked ) return;
		if ( PlayerPrefs.GetInt ( "unlocklevel13Restored" ) == 1 )
		{
			callBackOnFinishedTransaction ( true );
		}
	}
	
	private void handleTouched ()
	{
		if ( FLGlobalVariables.TRANSACTION ) return;
		FLGlobalVariables.TRANSACTION = true;

#if UNITY_ANDROID
//		InAppBillingManager.getInstance ().initPurchase ( "unlocklevel13", callBackOnFinishedTransaction );
#else
//		InAppBillingManagerIOS.getInstance ().initPurchase ( "unlocklevel13ios02", callBackOnFinishedTransaction );
		//callBackOnFinishedTransaction ( true );
#endif
		GoogleAnalytics.instance.LogScreen ( "Button Click - Unlock Level 13" );
	}

	private void callBackOnFinishedTransaction ( bool success, bool timeOut = false )
	{
		FLGlobalVariables.TRANSACTION = false;
		if ( success )
		{
			_unlocked = true;
			GoogleAnalytics.instance.LogScreen ( "User bought UNLOCK LEVEL 13 and unlocked level 13" );
			FLMissionScreenMapDialogManager.getInstance ().startUnlockingProcedure ();
		}
		else
		{
			
		}
	}
}
