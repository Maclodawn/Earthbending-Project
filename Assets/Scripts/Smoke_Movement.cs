using UnityEngine;
using System.Collections;

public class Smoke_Movement : MonoBehaviour
{

    float m_x = 0.05f;

	// Use this for initialization
	void Start ()
    {
        transform.localScale = new Vector3(0, 0, 0);
        
        Color color = GetComponent<Renderer>().material.color;
        color.a = 0.9f;
        GetComponent<Renderer>().material.color = color;
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_x += 0.009f;

        transform.localScale = new Vector3(1, 1, 1) * ((-1 / m_x / 10) + 2) / 2;

        Color color = GetComponent<Renderer>().material.color;
        color.a -= 0.03f;
        color.b -= 0.02f;
        color.r -= 0.02f;
        color.g -= 0.02f;
        GetComponent<Renderer>().material.color = color;
        
        if (color.a < 0)
            Destroy(gameObject);

        //Rotation
        transform.Rotate(Vector3.up + Vector3.right, Space.World);
	}
}
