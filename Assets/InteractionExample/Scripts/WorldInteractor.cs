using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldInteractor : MonoBehaviour
{
    [SerializeField]
    private Camera m_camera = null;
    [SerializeField]
    private float m_rangeOfRaycast = 5.0f;

    [SerializeField]
    private float m_maxTimeToInteract = 0.5f;
    [SerializeField]
    private float m_currentTimeFromInteract = 0.0f;

    [SerializeField]
    private bool m_allowInteraction = true;

    [SerializeField]
    private Transform m_interactionIcon;
    private Image m_interactionBackground;


    private void Start()
    {
        m_currentTimeFromInteract = 0;
        m_interactionBackground = m_interactionIcon.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        Debug.DrawRay(m_camera.transform.position, m_camera.transform.TransformDirection(Vector3.forward) * m_rangeOfRaycast, Color.red);
        if (Physics.Raycast(m_camera.transform.position, m_camera.transform.TransformDirection(Vector3.forward), out hit, m_rangeOfRaycast))
        {
            Debug.DrawRay(m_camera.transform.position, m_camera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (hit.transform.tag == "Interactable")
            {
                m_interactionIcon.gameObject.SetActive(true);
                m_interactionIcon.position = m_camera.WorldToScreenPoint(hit.transform.position);

                if (Input.GetKey(KeyCode.E) && m_allowInteraction)
                {
                    m_currentTimeFromInteract += Time.deltaTime;

                    
                    if (m_currentTimeFromInteract >= m_maxTimeToInteract)
                    {
                        hit.transform.gameObject.GetComponent<Interaction>().Interacted();
                        m_currentTimeFromInteract = 0;
                        m_allowInteraction = false;
                    }
                }
                if (Input.GetKeyUp(KeyCode.E))
                {
                    if (!m_allowInteraction)
                        m_allowInteraction = true;

                    if (m_currentTimeFromInteract > 0)
                    {
                        m_currentTimeFromInteract = 0;
                    }
                }
            }
            else
            {
                if (m_currentTimeFromInteract > 0)
                {
                    m_currentTimeFromInteract = 0;
                }
                if (!m_allowInteraction)
                    m_allowInteraction = true;

                if (m_interactionIcon.gameObject.activeInHierarchy)
                    m_interactionIcon.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!m_allowInteraction)
                m_allowInteraction = true;

            if (m_interactionIcon.gameObject.activeInHierarchy)
                m_interactionIcon.gameObject.SetActive(false);
        }
        
       m_interactionBackground.fillAmount = m_currentTimeFromInteract / m_maxTimeToInteract;
    }
}