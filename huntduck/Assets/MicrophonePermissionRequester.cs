using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class MicrophonePermissionRequester : MonoBehaviour
{
    void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Debug.Log("Requesting microphone permission...");
            Permission.RequestUserPermission(Permission.Microphone);
        }
        else
        {
            Debug.Log("Microphone permission already granted.");
        }
#else
        Debug.Log("Not running on Android (or in Editor), microphone permission is not requested at runtime.");
#endif
    }
}