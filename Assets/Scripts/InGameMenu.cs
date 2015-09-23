using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour
{

    public bool m_isEscape = false;
    bool m_isScore = false;

	// Use this for initialization
	void Start ()
    {
	
	}
	
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
            GetComponent<Player_AimPoint>().enabled = false;
            GetComponent<Player_Look>().enabled = false;
            if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2, 150, 50), "Exit"))
            {
                Application.Quit();
            }
        }
        else
        {
            GetComponent<Player_AimPoint>().enabled = true;
            GetComponent<Player_Look>().enabled = true;
        }

        if (m_isScore)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject p in players)
                print(p.GetComponent<Player_Movement>().m_username);
        }
    }
}
