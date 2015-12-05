using UnityEngine;
using System.Collections;

public abstract class AttacksGetter : MonoBehaviour {

	protected BasicAttack[] atks;

	protected abstract void init();

	public BasicAttack[] getOrderedAttacks() {
		if (atks == null || atks.Length == 0)
			init();

		return atks;
	}
}
