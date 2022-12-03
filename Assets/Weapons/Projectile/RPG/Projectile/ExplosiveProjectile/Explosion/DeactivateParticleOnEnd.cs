using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateParticleOnEnd : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] m_particleSystem = null;
    [SerializeField]
    private GameObject m_particleGameObject = null;

    // Update is called once per frame
    void Update()
    {
        if(m_particleGameObject.activeInHierarchy)
        {
            for(int i = 0; i < m_particleSystem.Length; i++)
            {
                if(m_particleSystem[i].IsAlive())
                {
                    return;
                }
            }

            m_particleGameObject.SetActive(false);
        }
    }
}
