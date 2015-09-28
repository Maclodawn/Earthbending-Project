using UnityEngine;
using System.Collections;

public class PlayerLook : MonoBehaviour
{

    public GameObject m_target;
    public GameObject m_targetHead;
    public Vector2 m_rotateSpeed = new Vector2(2, 1);

    public float m_smooth = 3;

    Vector3 m_initialLocalPosition;

	// Use this for initialization
	void Start ()
    {
        m_initialLocalPosition = transform.localPosition;
	}
	
	void LateUpdate ()
    {
        float horizontal = Input.GetAxis("Mouse X") * m_rotateSpeed.x;
        float vertical = Input.GetAxis("Mouse Y") * m_rotateSpeed.y;
        
        m_target.transform.Rotate(0, horizontal, 0);
        m_targetHead.transform.Rotate(vertical, 0, 0);

        transform.localPosition = m_initialLocalPosition;

        RaycastHit hit = new RaycastHit();
        int maskID = LayerMask.NameToLayer("Player");
        int mask = 1 << maskID;
        
        if (Physics.Linecast(m_target.transform.position, transform.position, out hit, ~mask))
        {
            transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
	}
}
