using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopup : MonoBehaviour
{
    public Vector3 movePopup = new Vector3(0f, 0.1f, 0f);
    public float scaleAmount = 1f;

    public float disappearTimerMax = 1f;
    public float disappearSpeed = 3f;
    private float disappearTimer = 1f;

    private Text scoreText;
    private Color textColor;

    void Start()
    {
        //Destroy(gameObject, disappearTimer);

        scoreText = GetComponentInChildren<Text>();
        textColor = scoreText.color;
        disappearTimer = disappearTimerMax;
    }

    void Update()
    {
        // convert options to functions make a dropdown?
        if (disappearTimer > disappearTimerMax * 5/6)
        {
            // first half of popup lifetime
            transform.localScale += Vector3.one * scaleAmount * Time.deltaTime;
            transform.position += movePopup * Time.deltaTime;
        }
        else if (disappearTimer < disappearTimerMax * 1/3)
        {
            // second half of popup lifetime
            //transform.localScale -= Vector3.one * scaleAmount * Time.deltaTime; // make smaller

            //disappear
            textColor.a -= disappearSpeed * Time.deltaTime;
            scoreText.color = textColor;

        }

        disappearTimer -= Time.deltaTime;

        //if(disappearTimer < 0)
        //{
        //    textColor.a -= disappearSpeed * Time.deltaTime;
        //    scoreText.color = textColor;
        //}
        if (textColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    // 16:45 sort order https://www.youtube.com/watch?v=iD1_JczQcFY&t=514s
}
