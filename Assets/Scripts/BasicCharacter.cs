using UnityEngine;
using System.Collections;

public class BasicCharacter : MonoBehaviour {

	protected bool m_executingAtk1 = false;
	protected bool m_executingAtk2 = false;
	protected bool m_executingAtk3 = false;
	public GameObject m_attack1Object;
	public GameObject m_attack2Object;
	public GameObject m_attack3Object;

	public void init(Manager manager) {

	}
}
