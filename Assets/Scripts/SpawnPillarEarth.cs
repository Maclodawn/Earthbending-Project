using UnityEngine;
using System.Collections;

public class SpawnPillarEarth : MonoBehaviour
{
    [SerializeField]
    float m_maxLength;

    float m_length = 0.1f;
    Mesh m_mesh;

	// Use this for initialization
	void Start ()
    {
        m_mesh = transform.GetChild(0).GetComponent<MeshFilter>().mesh;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Destroy(this.gameObject);
        }

        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        TestMeshTubeDeformation.expand(m_mesh, 1);

// 	    if (m_length < m_maxLength)
//         {
//             mesh.bounds.size = new Vector3();
//         }
	}
}
