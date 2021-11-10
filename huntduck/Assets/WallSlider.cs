using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlider : MonoBehaviour
{
    private Vector3 startPosition;
    public Transform movePosition1;
    public Transform movePosition2;

    private float timeElasped;
    public float timeToNewPosition;
    public float delayTime;

    public delegate void PositionReached();
    public static event PositionReached onPosition1Reached;

    void OnEnable()
    {
        SelectModeTrigger.onSelectModeTriggered += StartMoveToPosition;
    }

    private void OnDisable()
    {
        SelectModeTrigger.onSelectModeTriggered -= StartMoveToPosition;
    }

    void StartMoveToPosition()
    {
        StartCoroutine(MoveToPosition(transform, movePosition1, movePosition2, timeToNewPosition));
    }

    IEnumerator MoveToPosition(Transform transform, Transform position1, Transform position2, float timeToReachTarget)
    {
        yield return new WaitForSeconds(delayTime);

        startPosition = transform.position;
        timeElasped = 0f;

        while (timeElasped < 1)
        {
            timeElasped += Time.deltaTime / timeToReachTarget;
            transform.position = Vector3.Lerp(startPosition, position1.position, timeElasped);
            yield return null;
        }

        onPosition1Reached?.Invoke();

        startPosition = position1.position;
        timeElasped = 0f;

        while (timeElasped < 1)
        {
            timeElasped += Time.deltaTime / timeToReachTarget;
            transform.position = Vector3.Lerp(startPosition, position2.position, timeElasped);
            yield return null;
        }
    }
}
