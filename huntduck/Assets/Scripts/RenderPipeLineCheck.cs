using UnityEngine;
using UnityEngine.Rendering;

public class RenderPipeLineCheck : MonoBehaviour
{
    void Start()
    {
        if (GraphicsSettings.renderPipelineAsset != null)
        {
            Debug.Log("RenderPipelineCheck.cs: Render pipeline is " + GraphicsSettings.renderPipelineAsset.GetType().Name);
        }
        else
        {
            Debug.Log("RenderPipelineCheck.cs: dude.. there ain't no render pipeline...");
        }

    }
}
