using UnityEngine;

[RequireComponent(typeof(BNG.Damageable))]
public class EggFlight : MonoBehaviour
{
    public float eggSpeed = 2f;

    private GameObject playerController;
    private BNG.Damageable damageable;

    void Start()
    {
        playerController = ObjectManager.instance.playerController;
        damageable = transform.GetComponent<BNG.Damageable>();
    }

    void Update()
    {
        if (transform.position == playerController.transform.position)
        {
            //TODO: do damage to player
            damageable.DestroyThis();
        }
        else
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, playerController.transform.position, eggSpeed * Time.deltaTime);
            transform.position = newPos;
        }
    }
}