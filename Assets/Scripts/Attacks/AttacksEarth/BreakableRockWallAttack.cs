using UnityEngine;
using System.Collections;

public class BreakableRockWallAttack : EarthAttack {

	public GameObject rockWall;

	protected override void updateMe() {
		basicAttack4();
	}

	protected override float WAIT_TIME() {
		return 0.3f;
	}

    protected void basicAttack4()
    {
        Ray ray = GetComponent<AttackLauncher>().getAimRay();
        RaycastHit hit;
        bool collided = Physics.Raycast(ray, out hit, 5000);

        if (!collided)
            hit.point = ray.direction * 5000;

        Quaternion rotation = Quaternion.FromToRotation(transform.up, hit.normal) * Quaternion.FromToRotation(rockWall.transform.forward, transform.forward);
        Vector3 newDirection = rotation * rockWall.transform.up;

        float ySize = 0;
        for (int i = 0; i < rockWall.transform.childCount; ++i)
        {
            MeshRenderer meshRenderer = rockWall.transform.GetChild(i).GetComponent<MeshRenderer>();
            ySize += meshRenderer.bounds.size.y;
        }

        Vector3 vect = newDirection * ySize / 2.0f;
        Instantiate(rockWall, hit.point - vect, rotation);
    }
}
