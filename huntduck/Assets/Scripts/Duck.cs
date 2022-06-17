using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BNG.Damageable))]
public class Duck : MonoBehaviour
{
    public int duckPoints = 500;
    public GameObject pointsTextObj;
    public float duckEggDamage = 34f; // egg damage overridden on per duck type basis
    public bool dropsEggs;
    public AudioClip pointsSound;

    private Transform player;
    private GameObject egg;

    public delegate void DuckDied(int points);
    public static event DuckDied onDuckDied;


    void Start()
    {
        player = ObjectManager.instance.player.transform;
    }

    void OnEnable()
    {
        StopBumps.onBump += dropThaEgg;
        BNG.Damageable.onDuckDie += Die;
    }

    void OnDisable()
    {
        StopBumps.onBump -= dropThaEgg;
        BNG.Damageable.onDuckDie -= Die;
    }

    public void dropThaEgg(Transform duck)
    {
        if (duck == transform && dropsEggs)
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

    public void Die(GameObject deadDuck)
    {
        if (deadDuck == gameObject)
        {
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
        BNG.VRUtils.Instance.PlaySpatialClipAt(pointsSound, transform.position, 1f, 1f);
    }
}