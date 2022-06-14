using UnityEngine;

public class HideMe : MonoBehaviour
{
    private GameObject me;

    void Start()
    {
        HideThis();
    }

    public void HideThis()
    {
        me = this.gameObject;
        me.SetActive(false);
    }
}