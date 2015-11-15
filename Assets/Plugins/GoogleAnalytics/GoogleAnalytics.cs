using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoogleAnalytics : MonoBehaviour {
	
	private string propertyID;
	
	public static GoogleAnalytics instance;
	
	private string bundleID;
	public string appName;
	public string appVersion;
	
	private string screenRes;
	private string clientID;
	
	void Awake()
	{
		if(instance)
			DestroyImmediate(gameObject);
		else
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}

#if UNITY_IPHONE
		propertyID = "UA-45713022-3";
		bundleID = "com.sugra-games.toyrescuestory";
#elif UNITY_ANDROID
		propertyID = "UA-45713022-2";
		bundleID = "com.sugragames.toyrescuestory";
#endif
	}
	
	void Start() 
	{
		screenRes = Screen.width + "x" + Screen.height;
		
		clientID = SystemInfo.deviceUniqueIdentifier;
		
		switch ( Application.loadedLevelName )
		{
			case "00":
				//FlurryAnalytics.Instance ().LogEvent ( "Game starts" );
				LogScreen ( "Game starts" );
				break;
		}
	}

	public void LogScreen(string title)
	{
		title = WWW.EscapeURL(title);
		
		var url = "http://www.google-analytics.com/collect?v=1&ul=en-us&t=appview&sr="+screenRes+"&an="+WWW.EscapeURL(appName)+"&a=448166238&tid="+propertyID+"&aid="+bundleID+"&cid="+WWW.EscapeURL(clientID)+"&_u=.sB&av="+appVersion+"&_v=ma1b3&cd="+title+"&qt=2500&z=185";
		
		WWW request = new WWW(url);
		
		/*if(request.error == null)
		{
			if (request.responseHeaders.ContainsKey("STATUS"))
			{
				if (request.responseHeaders["STATUS"] == "HTTP/1.1 200 OK")	
				{
					Debug.Log ("GA Success");
				}else{
					Debug.LogWarning(request.responseHeaders["STATUS"]);	
				}
			}else{
				Debug.LogWarning("Event failed to send to Google");	
			}
		}else{
			Debug.LogWarning(request.error.ToString());	
		}*/
		
	}
}