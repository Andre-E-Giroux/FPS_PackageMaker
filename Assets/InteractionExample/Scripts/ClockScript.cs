using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClockScript : MonoBehaviour
{
    [SerializeField]
    private Transform secondHand, minuteHand, hourHand;

    [SerializeField]
    private const float
        MILISECONDS_TO_DEGREES = 360f / 60000f,
        SECONDS_TO_DEGREES = 360f / 60f,
        MINUTES_TO_DEGREES = 360f / 60f,
        HOURS_TO_DEGREES = 360f / 12f;

    [SerializeField]
    private bool m_isDigitalMode = true;

    [SerializeField]
    private AudioClip[] m_clockSounds = new AudioClip[2];

    [SerializeField]
    private AudioSource audioSource;

    private int previousSecond = 0;


    // Start is called before the first frame update
    void Start()
    {
        previousSecond = DateTime.Now.Second;

        if (!audioSource)
            audioSource = gameObject.GetComponent<AudioSource>();
        if (!audioSource.clip)
            audioSource.clip = m_clockSounds[0];
    }

    // Update is called once per frame
    void Update()
    {
        DateTime currentTime = DateTime.Now;

        if (m_isDigitalMode)
            DigitalModeUpdate(currentTime);
        else
            AnalogModeUpdate(currentTime);
    }

    private void AnalogModeUpdate(DateTime currentTime)
    {
        hourHand.localRotation =
        Quaternion.Euler(0f, 0f, ((float)currentTime.Hour + (((float)currentTime.Minute/60f)) + ((float)currentTime.Second / 3600)) * HOURS_TO_DEGREES);
        minuteHand.localRotation =
        Quaternion.Euler(0f, 0f, ((float)currentTime.Minute + ((float)currentTime.Second / 60f))  * MINUTES_TO_DEGREES);
        secondHand.localRotation =
        Quaternion.Euler(0f, 0f, ((float)currentTime.Second + ((float)currentTime.Millisecond / 1000f)) * SECONDS_TO_DEGREES);
    }

    private void DigitalModeUpdate(DateTime currentTime)
    {
        hourHand.localRotation =
        Quaternion.Euler(0f, 0f, currentTime.Hour * HOURS_TO_DEGREES);
        minuteHand.localRotation =
        Quaternion.Euler(0f, 0f, currentTime.Minute * MINUTES_TO_DEGREES);
        secondHand.localRotation =
        Quaternion.Euler(0f, 0f, currentTime.Second * SECONDS_TO_DEGREES);

        if(previousSecond != currentTime.Second)
        {
            previousSecond = currentTime.Second;

            if (audioSource.clip.Equals(m_clockSounds[0]))
                audioSource.clip = m_clockSounds[1];
            else
                audioSource.clip = m_clockSounds[0];
            audioSource.Play();
        }

    }

    public void ChangeClockMode()
    {
        m_isDigitalMode = !m_isDigitalMode;
    }

    public bool GetClockMode()
    {
        return m_isDigitalMode;
    }

}
