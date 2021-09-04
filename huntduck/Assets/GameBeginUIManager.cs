using UnityEngine;
using UnityEngine.UI;

public class GameBeginUIManager : MonoBehaviour
{
    public Text beginGameText;

    void Start()
    {
        this.gameObject.SetActive(true);
    }

    void theGameBegins()
    {
        theGameBegins();
        Debug.Log("LET THE GAMES BEGIN!!");
        // turn on the GameBeginsUI
        beginGameText.enabled = true;
        beginGameText.text = "Welcome to the Practice Round";



    }

    // make an ienumerator that loops through UIs
}
