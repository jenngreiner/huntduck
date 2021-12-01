using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGameWorld : MonoBehaviour
{
    private Vector3 startPosition;
    public Transform movePosition1;

    private float timeElasped;
    public float timeToNewPosition;
    public float delayTime;

    public delegate void WorldPositionReached();
    public static event WorldPositionReached onWorldPosition1Reached;

    private void Start()
    {
        StartMoveToPosition();
    }

    void StartMoveToPosition()
    {
        StartCoroutine(MoveToPosition(transform, movePosition1, timeToNewPosition));
    }

    IEnumerator MoveToPosition(Transform transform, Transform position1, float timeToReachTarget)
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

        onWorldPosition1Reached?.Invoke();
    }
}
