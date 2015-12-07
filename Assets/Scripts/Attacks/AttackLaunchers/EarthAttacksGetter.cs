using UnityEngine;
using System.Collections;

public class EarthAttacksGetter : AttacksGetter {

	protected override void init() {
		atks = new BasicAttack[4];
		atks[0] = GetComponent<RockBulletAttack>();
		atks[1] = GetComponent<BreakableRockWallAttack>();
		atks[2] = GetComponent<BreakableRockPillarAttack>();
		atks[3] = GetComponent<BreakableVerticalRockPillarAttack>();
	}
}
