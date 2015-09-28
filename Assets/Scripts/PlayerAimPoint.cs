using UnityEngine;
using System.Collections;

public class PlayerAimPoint : MonoBehaviour
{

    public Vector2 m_sizeAimPoint = new Vector2(100, 100);
    public Texture m_aimPoint;

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(Screen.width / 2 - m_sizeAimPoint.x / 2,
                                 Screen.height / 2 - m_sizeAimPoint.y / 2,
                                 m_sizeAimPoint.x, m_sizeAimPoint.y),
                        m_aimPoint);
    }
}
