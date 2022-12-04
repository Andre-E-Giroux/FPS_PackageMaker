using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockButtonInteraction : Interaction
{
    [SerializeField]
    private ClockScript m_clockScript;

    [SerializeField]
    private Animator m_buttonAnimator;

    [SerializeField]
    private TextMesh m_textMesh;


    private void Start()
    {
        if (m_clockScript.GetClockMode())
        {
            m_textMesh.text = "Digital";
        }
        else
        {
            m_textMesh.text = "Analog";
        }
    }

    public override void Interacted()
    {
        m_clockScript.ChangeClockMode();

        m_buttonAnimator.SetTrigger("Pressed");


        // true = digital
        // false = analog
        if(m_clockScript.GetClockMode())
        {
            m_textMesh.text = "Digital";
        }
        else
        {
            m_textMesh.text = "Analog";
        }

    }
}
