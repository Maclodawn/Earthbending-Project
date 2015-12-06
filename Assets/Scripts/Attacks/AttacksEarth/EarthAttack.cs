using UnityEngine;
using System.Collections;

public abstract class EarthAttack : BasicAttack {

	public float m_OffsetForwardEarth = 1;
	[SerializeField]
	protected float m_attack1ForceUp = 32000;
	[SerializeField]
	protected float m_attack1ForceForward = 1000000;
}
