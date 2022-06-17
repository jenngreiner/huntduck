using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopup : MonoBehaviour
{
    public Vector3 movePopup = new Vector3(0f, 1f, 0f);
    public float moveUpSpeed = 0.2f;
    public float scaleSpeed = 10f;
    public float scaleAmount = 10f;

    public float effectTimer = 3f;
    private float effectTimerLeft = 1f;
    public float disappearSpeed = 0.75f;

    private Text scoreText;
    private Color textColor;

    // dropdown to select effect you want
    public enum EffectType { ScaleAndFade, ScaleAndShrink, FloatAndFade }
    public EffectType effectType;

    public delegate void SelectedDelegate();
    SelectedDelegate effectMethod;

    void Start()
    {
        scoreText = GetComponentInChildren<Text>();
        textColor = scoreText.color;

        // setup popup
        effectTimerLeft = effectTimer;
        movePopup *= moveUpSpeed;
        SetSelectedEffect(effectType);
    }

    void Update()
    {
        effectMethod();
    }

    void SetSelectedEffect(EffectType _effectType)
    {
        // set delegate method in update via enum selected in inspector
        switch (effectType)
        {
            case EffectType.ScaleAndShrink:
                effectMethod = ScaleAndShrink;
                break;
            case EffectType.ScaleAndFade:
                effectMethod = ScaleAndFade;
                break;
            case EffectType.FloatAndFade:
                effectMethod = FloatAndFade;
                break;
            default:
                break;
        }
    }

    void ScaleAndFade()
    {
        if (effectTimerLeft > effectTimer / 2) // first half of popup lifetime
        {
            // scale up, quickly at first then slower
            transform.localScale += Vector3.one * scaleAmount * Time.deltaTime;
            scaleAmount -= scaleAmount * scaleSpeed * Time.deltaTime;
        }
        else // second half of popup lifetime
        {
            // move up
            transform.position += movePopup * Time.deltaTime;

            // disappear
            textColor.a -= disappearSpeed * Time.deltaTime;
            scoreText.color = textColor;

            if (textColor.a <= 0)
            {
                // if completely faded, destroy
                Destroy(gameObject);
            }
        }

        effectTimerLeft -= Time.deltaTime;
    }

    void ScaleAndShrink()
    {
        transform.position += movePopup * Time.deltaTime;
        movePopup -= movePopup * scaleSpeed * Time.deltaTime;
        Debug.Log("movePopup - movePopup * moveSpeedSlow is " + movePopup);

        if (effectTimerLeft > effectTimer / 2)
        {
            transform.localScale += Vector3.one * scaleAmount * Time.deltaTime;
        }
        else
        {
            transform.localScale -= Vector3.one * scaleAmount * Time.deltaTime;
        }

        effectTimerLeft -= Time.deltaTime;

        if (effectTimerLeft < 0)
        {
            textColor.a -= disappearSpeed * Time.deltaTime;
            scoreText.color = textColor;
            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void FloatAndFade()
    {
        transform.position += movePopup * Time.deltaTime;

        effectTimerLeft -= Time.deltaTime;

        if (effectTimerLeft < 0)
        {
            textColor.a -= disappearSpeed * Time.deltaTime;
            scoreText.color = textColor;
            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
