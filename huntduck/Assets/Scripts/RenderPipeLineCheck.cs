using UnityEngine;
using UnityEngine.Rendering;

public class RenderPipeLineCheck : MonoBehaviour
{
    void Start()
    {
        if (GraphicsSettings.renderPipelineAsset != null)
        {
            Debug.Log("Render pipeline is " + GraphicsSettings.renderPipelineAsset.GetType().Name);
        }
        else
        {
            Debug.Log("dude.. there ain't no render pipeline...");
        }

    }
}
