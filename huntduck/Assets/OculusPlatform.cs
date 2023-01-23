using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;

public class OculusPlatform : MonoBehaviour
{
	public static OculusPlatform instance;

	// my Application-scoped Oculus ID
	private ulong m_myID;
	// my Oculus user name
	private string m_myOculusID;

	#region Initialization and Shutdown

	void Awake()
	{
		// make sure only one instance of this manager ever exists
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
		}

		DontDestroyOnLoad(gameObject);

		Core.Initialize();
	}


	void Start()
	{
		// First thing we should do is perform an entitlement check to make sure we successfully connected to the Oculus Platform Service.
		Entitlements.IsUserEntitledToApplication().OnComplete(IsEntitledCallback);
	}

	void IsEntitledCallback(Message msg)
	{
		if (msg.IsError)
		{
			TerminateWithError(msg);
			return;
		}

		// Next get the identity of the user that launched the Application.
Users.GetLoggedInUser().OnComplete(GetLoggedInUserCallback);
	}

	void GetLoggedInUserCallback(Message<User> msg)
	{
		if (msg.IsError)
		{
			TerminateWithError(msg);
			return;
		}

		// get the IDs
		m_myID = msg.Data.ID;
		m_myOculusID = msg.Data.OculusID;
	}

	// In this example, for most errors, we terminate the Application.  A full App would do
	// something more graceful.
	public static void TerminateWithError(Message msg)
	{
		Debug.Log("Error: " + msg.GetError().Message);
		UnityEngine.Application.Quit();
	}

	public void QuitButtonPressed()
	{
		UnityEngine.Application.Quit();
	}

	void OnApplicationQuit()
	{
		// be a good matchmaking citizen and leave any queue immediately
		//Matchmaking.LeaveQueue();
	}

	#endregion

	#region Properties

	public static ulong MyID
	{
		get
		{
			if (instance != null)
			{
				return instance.m_myID;
			}
			else
			{
				return 0;
			}
		}
	}

	public static string MyOculusID
	{
		get
		{
			if (instance != null && instance.m_myOculusID != null)
			{
				return instance.m_myOculusID;
			}
			else
			{
				return string.Empty;
			}
		}
	}

	#endregion
}