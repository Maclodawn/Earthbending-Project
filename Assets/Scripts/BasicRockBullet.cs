using UnityEngine;
using System.Collections;

public class BasicRockBullet : FlingableRock
{

	// Use this for initialization
    // init() method has been created instead of Start() method to be able to
    // initialize the bullet when it is used again by a character
	public void init()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        m_isSpawning = true;
        m_height = transform.position.y;
        m_risingDone = false;
        m_risingStarted = false;
        m_flingDone = false;
        Instantiate(m_smokeStartToMove, transform.position, Quaternion.identity);
        m_collider.enabled = false;
	}

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
