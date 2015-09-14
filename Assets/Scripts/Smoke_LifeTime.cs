using UnityEngine;
using System.Collections;

public class Smoke_LifeTime : MonoBehaviour
{

    public float m_lifeTime = 1;
	
	// Update is called once per frame
	void Update ()
    {
        m_lifeTime -= Time.deltaTime;

        if (m_lifeTime < 0)
            Destroy(this.gameObject);
	}
}
