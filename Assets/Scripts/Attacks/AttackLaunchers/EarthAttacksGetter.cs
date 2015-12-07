using UnityEngine;
using System.Collections;

public class EarthAttacksGetter : AttacksGetter {

	protected override void init() {
		atks = new BasicAttack[2];
		atks[0] = GetComponent<RockBulletAttack>();
		atks[1] = GetComponent<BreakableRockWallAttack>();
		//atks[2] = GetComponent<>();
		//atks[3] = GetComponent<>();
	}
}
