using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour
{

    public bool m_isEscape = false;
    bool m_isScore = false;
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Cancel"))
            m_isEscape = !m_isEscape;

        if (!m_isEscape && Input.GetButton("Next"))
            m_isScore = true;
        else
            m_isScore = false;
	}

    void OnGUI()
    {
        if (m_isEscape)
        {
            GetComponent<PlayerAimPoint>().enabled = false;
            GetComponent<PlayerLook>().enabled = false;
            if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2, 150, 50), "Exit"))
            {
                Application.Quit();
            }
        }
        else
        {
            GetComponent<PlayerAimPoint>().enabled = true;
            GetComponent<PlayerLook>().enabled = true;
        }

        if (m_isScore)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

			int i = 0;

            foreach (GameObject p in players) {
               	//print(p.GetComponent<CharacterMovement>().m_username); //TODO check that...
				print("Player " + (++i));
			}
        }
    }
}
