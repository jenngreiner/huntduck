using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    public void CombineMeshes()
    {
        // store original rotation and position
        Quaternion oldRot = transform.rotation;
        Vector3 oldPos = transform.position;

        // reset rotation and position to 0, 0, 0 
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;

        // get all mesh filters in children
        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();

        Debug.Log(name + " is combining " + filters.Length + " meshes!");

        // create empty mesh object we will later combine meshes into
        Mesh finalMesh = new Mesh();

        CombineInstance[] combiners = new CombineInstance[filters.Length];

        // go through all filters, skip self
        for (int a = 0; a < filters.Length; a++)
        {
            // skip mesh on parent (object script is on)
            if (filters[a].transform == transform)
                continue;

            combiners[a].subMeshIndex = 0;
            combiners[a].mesh = filters[a].sharedMesh;
            combiners[a].transform = filters[a].transform.localToWorldMatrix; // set transform to world rotation and position of every mesh
        }

        // we got a shit ton of meshes, gotta handle that load with UInt32
        finalMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        finalMesh.CombineMeshes(combiners); // combine meshes
        GetComponent<MeshFilter>().sharedMesh = finalMesh; // set meshes into existing mesh asset (so we can see it)

        // set rotation and position back to where we put it in our level
        transform.rotation = oldRot;
        transform.position = oldPos;

        // turn off children to eliminate z-fighting
        for (int a = 0; a < transform.childCount; a++)
        {
            transform.GetChild(a).gameObject.SetActive(false);
        }

    }
}
