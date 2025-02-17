using UnityEngine;
using Photon.Voice.Unity;

public class RecorderReinitializer : MonoBehaviour
{
    public Recorder recorder; // Assign your global recorder (or local recorder) here.

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && recorder != null)
        {
            // If the recorder is not currently transmitting, try restarting it.
            if (!recorder.IsCurrentlyTransmitting)
            {
                Debug.Log("Application regained focus. Restarting recorder...");
                recorder.RestartRecording();
            }
        }
    }
}
