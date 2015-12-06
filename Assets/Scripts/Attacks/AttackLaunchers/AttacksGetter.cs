using UnityEngine;
using System.Collections;

public abstract class AttacksGetter : MonoBehaviour {

	protected BasicAttack[] atks = null;

	protected abstract void init();

	public BasicAttack[] getOrderedAttacks() {
		if (atks == null || atks.Length == 0)
			init();

		return atks;
	}
}
