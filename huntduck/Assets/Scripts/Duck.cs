using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BNG.Damageable))]
public class Duck : MonoBehaviour
{
    public int duckPoints = 500;
    public GameObject pointsTextObj;
    public int duckEggDamage = 34; // egg damage overridden on per duck type basis
    public bool dropsEggs;
    public AudioClip quackSound;
    public AudioClip pointsSound;
    private bool alive = true;

    private Transform player;
    private GameObject egg;
    private BNG.Damageable damageable;

    public delegate void DuckDied(int points);
    public static event DuckDied onDuckDied;


    void Start()
    {
        player = ObjectManager.instance.player.transform;
        damageable = GetComponent<BNG.Damageable>();
        StartCoroutine(Quack());
    }

    void OnEnable()
    {
        StopBumps.onBump += dropThaEgg;
        BNG.Damageable.onDuckDie += Die;
        RestartGameMode.onRestartMode += EnterFlyAwayMode;
    }

    void OnDisable()
    {
        StopBumps.onBump -= dropThaEgg;
        BNG.Damageable.onDuckDie -= Die;
        RestartGameMode.onRestartMode -= EnterFlyAwayMode;
    }

    public void dropThaEgg(Transform duck, Transform transformHit)
    {
        // if the duck hits the playerguard and can drop eggs, drop eggs
        if (duck == transform && transformHit == ObjectManager.instance.playerGuard && dropsEggs)
        {
            egg = Instantiate(ObjectManager.instance.egg, transform.position, Quaternion.identity);
            Egg eggScript = egg.GetComponent<Egg>();
            eggScript.eggDamage = duckEggDamage;

            if (transform.tag == TagManager.GOLDENGOOSE_TAG)
            {
                eggScript.isHeatSeeking = true;
                egg.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(237 / 255f, 57 / 255f, 57 / 255f);
            }
        }
    }

    IEnumerator Quack()
    {
        while (alive)
        {
            yield return new WaitForSecondsRealtime(Random.Range(0.5f, 5f));
            BNG.VRUtils.Instance.PlayLinearSpatialClipAt(quackSound, transform.position, 1f, 1f);
        }
    }

    public void Die(GameObject deadDuck)
    {
        if (deadDuck == gameObject)
        {
            alive = false;
            EnterFlyAwayMode();
            onDuckDied?.Invoke(duckPoints); // subscribe in PlayerScore.cs
            CreatePointsText(duckPoints);
        }
    }

    public void CreatePointsText(int duckPoints)
    {
        GameObject pointsObj = Instantiate(pointsTextObj, transform.position, Quaternion.identity);
        pointsObj.transform.LookAt(player);
        Text pointsText = pointsObj.GetComponentInChildren<Text>();
        pointsText.text = "$" + duckPoints.ToString();
        BNG.VRUtils.Instance.PlayLinearSpatialClipAt(pointsSound, transform.position, 1f, 1f);
    }

    void EnterFlyAwayMode()
    {
        StopAllCoroutines(); // stop quacking
        damageable.enabled = false; // player can't shoot duck
    }
}