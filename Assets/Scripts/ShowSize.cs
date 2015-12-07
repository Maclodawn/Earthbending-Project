using System.Collections;
using UnityEngine;
using UnityEditor;

class ShowSize : MonoBehaviour
{
    public static void PrintShowSize()
    {
        GameObject thisObject = Selection.activeObject as GameObject;
        if (thisObject == null)
        {
            return;
        }

        MeshFilter mf = thisObject.GetComponent(typeof(MeshFilter)) as MeshFilter;
        if (mf == null)
        { return; }

        Mesh mesh = mf.sharedMesh;
        if (mesh == null)
        { return; }

        Vector3 size = mesh.bounds.size;
        Vector3 scale = thisObject.transform.localScale;
        Debug.Log("Size\tX: " + size.x * scale.x + "\tY: " + size.y * scale.y + "\tZ: " + size.z * scale.z);
    }
}