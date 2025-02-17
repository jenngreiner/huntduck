using UnityEngine;
using UnityEngine.UI;
using Photon.Voice.Unity;

public class VoiceLevelUI : MonoBehaviour
{
    [Tooltip("Reference to the Recorder component (should be the one that's transmitting).")]
    public Recorder recorder;

    [Header("UI Slider Option")]
    [Tooltip("Optional: UI Slider to display voice level (range 0-1).")]
    public Slider voiceSlider;

    [Header("UI Image Option")]
    [Tooltip("Optional: UI Image (with Fill Type) to display voice level.")]
    public Image voiceFillImage;

    //scaling factor to adjust amplitude
    public float scaleFactor = 1f; //adjust as needed

    void Update()
    {
        float voiceLevel = 0f;

        // Check if the recorder and its LevelMeter are available.
        if (recorder != null && recorder.LevelMeter != null)
        {
            //retrieve raw amplitude
            float rawAmp = recorder.LevelMeter.CurrentAvgAmp;

            //multiply by scaling factor
            float scaledVoiceLevel = Mathf.Clamp(rawAmp * scaleFactor, 0f, 1f);
            voiceLevel = scaledVoiceLevel;

            //voiceLevel = recorder.LevelMeter.CurrentAvgAmp;
            // If your version exposes a different property (e.g., CurrentAvg), use that:
            // voiceLevel = recorder.LevelMeter.CurrentAvg;

            // Update the slider, if assigned.
            if (voiceSlider != null)
            {
                voiceSlider.value = voiceLevel;
            }

            // Update the fill amount on an image, if assigned.
            if (voiceFillImage != null)
            {
                voiceFillImage.fillAmount = voiceLevel;
            }
        }

        Debug.Log("Voice Level: " + voiceLevel);
    }
}
