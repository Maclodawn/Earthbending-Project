using UnityEngine;
using System.Collections;

public class AIAttackLauncher : AttackLauncher {

    private float m_abortTimer = 0;

	protected override void updateInput() {
		//TODO AI thinking here
		atk = 0; //(Random.Range(0, 11) > 9) ? 0 : -1;

        bool atkFinished = atks[0].isFinished();

        if (hold == true) // we increase the abort timer while we're holding an attack
            m_abortTimer += Time.deltaTime;

        if (hold == true && (m_abortTimer >= 2.0f || atkFinished)) // Else we abort or the attack is finished
        {
            hold = false;
            m_abortTimer = 0;
		} 
        else if (atk == 0)
        {
            hold = true;
        }
	}
}
