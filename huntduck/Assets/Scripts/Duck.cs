using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BNG.Damageable))]
public class Duck : MonoBehaviour
{
    public int duckPoints = 500;
    public GameObject pointsTextObj;
    public float duckEggDamage = 34f; // egg damage overridden on per duck type basis
    public bool dropsEggs;

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

            // CHANGE transform.tag to GOLDEN GOOSE for heat seeking
            //if (transform.tag == TagManager.ANGRYDUCK_TAG)
            //{
            //    eggScript.isHeatSeeking = true;
            //    egg.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(246 / 255f, 156 / 255f, 156 / 255f);
            //}
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
        Debug.Log("Creating duck points for " + transform.name);
    }
}