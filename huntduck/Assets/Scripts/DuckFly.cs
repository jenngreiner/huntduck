using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // Requires Rigidbody to move around
public class DuckFly : MonoBehaviour
{
    [SerializeField] float idleSpeed, turnSpeed, switchSeconds, idleRatio;
    [SerializeField] Vector2 animSpeedMinMax, moveSpeedMinMax, changeAnimEveryFromTo, changeTargetEveryFromTo;
    [SerializeField] Transform homeTarget, flyingTarget;
    [SerializeField] Vector2 radiusMinMax;
    [SerializeField] Vector2 yMinMax;
    [SerializeField] public bool returnToBase = false;
    [SerializeField] public float randomBaseOffset = 5, delayStart = 0f;

    private Animator animator;
    private Rigidbody body;
    [System.NonSerialized]
    public float changeTarget = 0f, changeAnim = 0f, timeSinceTarget = 0f, timeSinceAnim = 0f, prevAnim, currentAnim = 0f, prevSpeed, speed, zturn, prevz, turnSpeedBackup;
    private Vector3 rotateTarget, position, direction, velocity, randomizedBase;
    private Quaternion lookRotation;
    [System.NonSerialized] public float distanceFromBase, distanceFromTarget;

    void Start()
    {
        // Initialize imp values
        //animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        turnSpeedBackup = turnSpeed;
        direction = Quaternion.Euler(transform.eulerAngles) * (Vector3.forward); // direction duck is facing
        if (delayStart < 0f)
        {
            body.velocity = idleSpeed * direction;
        }
    }

    void FixedUpdate()
    {
        // Wait if start should be delayed (useful to add small differences in large flocks)
        if (delayStart > 0f)
        {
            delayStart -= Time.fixedDeltaTime;
            return;
        }

        // calculate distances between duck & base, duck and target
        distanceFromBase = Vector3.Magnitude(randomizedBase - body.position);
        distanceFromTarget = Vector3.Magnitude(flyingTarget.position - body.position);

        // TIme for a new animation speed
        if (changeAnim < 0f)
        {
            prevAnim = currentAnim;
            currentAnim = ChangeAnim(currentAnim);
            changeAnim = Random.Range(changeAnimEveryFromTo.x, changeAnimEveryFromTo.y);
            timeSinceAnim = 0f;
            prevSpeed = speed;
            if (currentAnim == 0)
            {
                speed = idleSpeed;
            }
            else
            {
                speed = Mathf.Lerp(moveSpeedMinMax.x, moveSpeedMinMax.y, (currentAnim - animSpeedMinMax.x) / (animSpeedMinMax.y - animSpeedMinMax.x));
            }
        }

        if (changeTarget < 0f)
        {
            rotateTarget = ChangeDirection(body.transform.position);
            if (returnToBase)
            {
                changeTarget = 0.2f; // if bird instructed to return to base, update target every 0.2s (more quickly) to be more straight
            }
            else
            {
                changeTarget = Random.Range(changeTargetEveryFromTo.x, changeTargetEveryFromTo.y);
                timeSinceTarget = 0f;
            }
        }

        // Turn when approahcing height limits
        // ToDo: Adjust limit and "exit direction" by object's direction and velocity, instead of the 10f and 1f

        // Update timers
        changeAnim -= Time.fixedDeltaTime;
        changeTarget -= Time.fixedDeltaTime;

        // Update stopwatches
        timeSinceTarget += Time.fixedDeltaTime;
        timeSinceAnim += Time.fixedDeltaTime;

        // Rotate towards target
        if (rotateTarget != Vector3.zero)
        {
            lookRotation = Quaternion.LookRotation(rotateTarget, Vector3.up); // required rotation
            Vector3 rotation = Quaternion.RotateTowards(body.transform.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime).eulerAngles;
            body.transform.eulerAngles = rotation;
        }

        // Rotate on z-axis to tilt body towards turn direction

        // Min and max rotation on z-axis - can also be parameterized

        // Remove temp if transform is rotated back earlier in FixedUpdate

        // Move duck forward
        direction = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;
        body.velocity = Mathf.Lerp(prevSpeed, speed, Mathf.Clamp(timeSinceAnim / switchSeconds, 0f, 1f)) * direction;

        // Hard-limit
    }

    // select new animation speed randomly
    private float ChangeAnim(float currentAnim)
    {
        return 1; // make this random later
    }

    // Select a new direction to fly in randomly
    private Vector3 ChangeDirection(Vector3 currentPosition)
    {
        // 360-degree freedom of choice on the horizontal plane

        // Limited max steepness of ascent/descent in the vertical direction

        // calculate direction

        return Vector3.forward; // Temporary
    }
}