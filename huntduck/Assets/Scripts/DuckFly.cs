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
    [SerializeField] float heightBuffer = 10f;
    [SerializeField] public bool returnToBase = false;
    [SerializeField] public float randomBaseOffset = 5, delayStart = 0f;

    private Animator animator;
    private Rigidbody body;
    [System.NonSerialized]
    public float changeTarget = 0f, changeAnim = 0f, timeSinceTarget = 0f, timeSinceAnim = 0f, prevAnim, currentAnim = 0f, prevSpeed, speed, zturn, prevz, turnSpeedBackup;
    private Vector3 rotateTarget, position, direction, velocity, randomizedBase;
    private Quaternion lookRotation, bodyRotation;
    [System.NonSerialized] public float distanceFromBase, distanceFromTarget, distanceFromStand;

    private float oldyMin;
    private bool isSwerving;
    private float swerveDelay;

    void Start()
    {
        // Initialize imp values
        //animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        turnSpeedBackup = turnSpeed;
        direction = Quaternion.Euler(transform.eulerAngles) * (Vector3.forward); // direction duck is facing
        if (delayStart < 0f)
        {
            body.velocity = idleSpeed * direction; // move duck forward @ idlespeed
        } 

        homeTarget = GameObject.Find("HomeBase").transform;
        flyingTarget = GameObject.Find("PlayerGuard").transform;

        oldyMin = yMinMax.y;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            returnToBase = !returnToBase;
        }

        if (isSwerving && swerveDelay > 0f)
        {
            swerveDelay -= Time.deltaTime;
        }
        else if (isSwerving && swerveDelay <= 0f)
        {
            turnSpeed = turnSpeedBackup;
            isSwerving = false;
            Debug.Log(transform.name + " STOPPED swerving!");
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

        // allow drastic turns close to base to ensure target can be reached, even if approaching at angle
        if (returnToBase && distanceFromBase < 10f)
        {
            DrasticTurn();
        }

        // Time for a new animation speed
        if (changeAnim < 0f)
        {
            ChangeAnimSpeed();
        }

        // Time for a new target position
        if (changeTarget < 0f)
        {
            ChangeTarget();
        }

        // Force Duck to turn up or down when reaching top or bottom of allowable height
        if (body.transform.position.y < yMinMax.x + heightBuffer || body.transform.position.y > yMinMax.y - heightBuffer)
        {
            FlyUpAndDown();
        }

        UpdateTimersAndStopWatches();

        // Rotate towards target
        if (rotateTarget != Vector3.zero)
        {
            TurnTowardsTarget();
        }

        // Rotate on z-axis to tilt body towards turn direction
        TiltIntoTurn();

        // Move duck
        FlyAround();

        // Limit rotation on X and Z axes so bird doesn't spiral
        //float xRot = transform.localRotation.eulerAngles.x;
        //float zRot = transform.localRotation.eulerAngles.z;
        //if (xRot < 90f || xRot <-90f || zRot > 90f || zRot < -90f)
        //{
        //    LimitRotationXZ();
        //}

        // Hard-limit the height, in case the limit is breached desptire turnaround attempt
        if (body.transform.position.y < yMinMax.x || body.transform.position.y > yMinMax.y)
        {
            ResetHeight();
        }
    }

    void OnEnable()
    {
        InfiniteWaveSpawner.onGameOver += FlyAway;
        //StopBumps.onBump += Swerve;
    }

    void OnDisable()
    {
        InfiniteWaveSpawner.onGameOver -= FlyAway;
    }

    // select new animation speed randomly
    private float ChangeAnim(float currentAnim)
    {
        return 1;

        // TODO: add this to make animation match flying later
        //float newState;
        //if (Random.Range(0f, 1f) < idleRatio) newState = 0f;
        //else
        //{
        //    newState = Random.Range(animSpeedMinMax.x, animSpeedMinMax.y);
        //}
        //if (newState != currentAnim)
        //{
        //    animator.SetFloat("flySpeed", newState);
        //    if (newState == 0) animator.speed = 1f; else animator.speed = newState;
        //}
        //return newState;
    }

    // Select a new direction to fly in randomly
    private Vector3 ChangeDirection(Vector3 currentPosition)
    {
        Vector3 newDir;
        
        // keep duck in specified radius around its target
        if (returnToBase)
        {
            // send the duck to its base
            randomizedBase = homeTarget.position;
            randomizedBase.y += Random.Range(-randomBaseOffset, randomBaseOffset);
            yMinMax.x = oldyMin; // make the ascent to the sky smooth
            yMinMax.y = 100f; // base in the sky
            newDir = randomizedBase - currentPosition;
        }
        // check distance between duck and its target
        else if (distanceFromTarget > radiusMinMax.y) // if larger than maximum allowable radius
        {
            // fly in direction of target
            newDir = flyingTarget.position - currentPosition;
        }
        else if (distanceFromTarget < radiusMinMax.x) // if too close to target
        {
            // fly away from target
            newDir = currentPosition  - flyingTarget.position;
        }
        else // flying towards target and within radius
        {
            // 360-degree freedom of choice on the horizontal plane
            float angleXZ = Random.Range(-Mathf.PI, Mathf.PI);

            // Limited max steepness of ascent/descent in the vertical direction
            // the larger the denomitor, the less steep the ascent / descent is (48 is arbitrary, could parameterize)
            // good idea to parameterize this as serializable parameter in future
            float angleY = Random.Range(-Mathf.PI / 48, Mathf.PI / 48);

            // calculate direction
            newDir = Mathf.Sin(angleXZ) * Vector3.forward + Mathf.Cos(angleXZ) * Vector3.right + Mathf.Sin(angleY) * Vector3.up;
        }

        return newDir.normalized;
    }

    void DrasticTurn()
    {
        if (turnSpeed != 300f && body.velocity.magnitude != 0f)
        {
            turnSpeedBackup = turnSpeed;
            turnSpeed = 300f;
        }
        else if (distanceFromBase <= 1f) // when close enough to base, 
        {
            //reset velocity to zero and restore original turn speed for next outing
            body.velocity = Vector3.zero;
            turnSpeed = turnSpeedBackup;
            return;
        }
    }

    void ChangeAnimSpeed()
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

    void ChangeTarget()
    {
        rotateTarget = ChangeDirection(body.transform.position); // get newDir back
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

    void FlyUpAndDown()
    {
        // ToDo: Adjust limit and "exit direction" by object's direction and velocity, instead of the 10f and 1f
        if (body.transform.position.y < yMinMax.x + heightBuffer)
        {
            rotateTarget.y = 1f;
        }
        else
        {
            rotateTarget.y = -1f;
        }
    }

    void UpdateTimersAndStopWatches()
    {
        // Update timers
        changeAnim -= Time.fixedDeltaTime;
        changeTarget -= Time.fixedDeltaTime;

        // Update stopwatches
        timeSinceTarget += Time.fixedDeltaTime;
        timeSinceAnim += Time.fixedDeltaTime;
    }

    void TurnTowardsTarget()
    {
        lookRotation = Quaternion.LookRotation(rotateTarget, Vector3.up); // required rotation
        Vector3 rotation = Quaternion.RotateTowards(body.transform.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime).eulerAngles;
        body.transform.eulerAngles = rotation;
    }

    void TiltIntoTurn()
    {
        zturn = Mathf.Clamp(Vector3.SignedAngle(rotateTarget, direction, Vector3.up), -45f, 45f);
        float temp = prevz;
        if (prevz < zturn)
        {
            prevz += Mathf.Min(turnSpeed * Time.fixedDeltaTime, zturn - prevz);
        }
        else
        {
            prevz -= Mathf.Min(turnSpeed * Time.fixedDeltaTime, prevz - zturn);
        }

        // Min and max rotation on z-axis - can also be parameterized
        prevz = Mathf.Clamp(prevz, -45f, 45f);

        // Remove temp if transform is rotated back earlier in FixedUpdate
        body.transform.Rotate(0f, 0f, prevz - temp, Space.Self);
    }

    void FlyAround()
    {
        direction = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;

        if (returnToBase && distanceFromBase < idleSpeed) // only a moment before landing at base
        {
            // gradually reduce velocity as you near base
            body.velocity = Mathf.Min(idleSpeed, distanceFromBase) * direction;
        }
        else
        {
            body.velocity = Mathf.Lerp(prevSpeed, speed, Mathf.Clamp(timeSinceAnim / switchSeconds, 0f, 1f)) * direction;
        }
    }

    void ResetHeight()
    {
        position = body.transform.position;
        position.y = Mathf.Clamp(position.y, yMinMax.x, yMinMax.y);
        body.transform.position = position;
    }

    //void LimitRotationXZ()
    //{
    //    float zAngle = transform.eulerAngles.z;
    //    float xAngle = transform.eulerAngles.x;
    //    transform.eulerAngles = new Vector3(ClampAngle(xAngle, -90f, 90f), 0f, ClampAngle(zAngle, -90f, 90f));
    //}

    //private float ClampAngle(float angle, float min, float max)
    //{
    //    if (angle < 90f || angle > 270f)
    //    {
    //        if (angle > 180)
    //        {
    //            angle -= 360f;
    //        }
    //        if (max > 180)
    //        {
    //            max -= 360f;
    //        }
    //        if (min > 180)
    //        {
    //            min -= 360f;
    //        }
    //    }
    //    angle = Mathf.Clamp(angle, min, max);
    //    if (angle < 0)
    //    {
    //        angle += 360f;
    //    }
    //    return angle;
    //}

    void LimitRotationXZ()
    {
        float minRotation = -90;
        float maxRotation = 90;
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        currentRotation.x = Mathf.Clamp(currentRotation.x, minRotation, maxRotation);
        currentRotation.z = Mathf.Clamp(currentRotation.z, minRotation, maxRotation);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    void FlyAway()
    {
        returnToBase = true;
    }

    public void Swerve()
    {
        //StartCoroutine(ISwerve());

        Debug.Log(transform.name + " is swerving!");
        isSwerving = true;
        swerveDelay = 0.5f;

        DrasticTurn();
        rotateTarget = body.transform.position - body.transform.forward; // change diretion to behind duck
        TurnTowardsTarget();
        TiltIntoTurn();
    }

    //void Swerve(Transform otherDuck)
    //{
    //    Debug.Log(otherDuck.name + "is otherDuck that DuckFly knows");

    //    isSwerving = true;
    //    swerveDelay = 0.5f;

    //    //distanceFromTarget = Vector3.Magnitude(flyingTarget.position - body.position);
    //    //float distanceFromOtherDuck = Vector3.Magnitude(otherDuck.position - body.position); // magnitude is the distance between the vector's origin (0,0,0) and its endpoint. If you think of the vector as a line, the magnitude is equal to its length.

    //    DrasticTurn();
    //    // change diretion to behind duck
    //    rotateTarget = body.transform.position - body.transform.forward;
    //    TurnTowardsTarget();
    //    TiltIntoTurn();

    //}

    //IEnumerator ISwerve()
    //{
    //    Debug.Log(transform.name + " is swerving!");
    //    isSwerving = true;
    //    swerveDelay = 0.5f;

    //    DrasticTurn();
    //    rotateTarget = body.transform.position - body.transform.forward; // change diretion to behind duck
    //    TurnTowardsTarget();
    //    TiltIntoTurn();
    //    //yield return new WaitForSeconds(0.5f);
    //    //turnSpeed = turnSpeedBackup;
    //    yield return null;
    //    Debug.Log(transform.name + " STOPPED swerving!");
    //}
}