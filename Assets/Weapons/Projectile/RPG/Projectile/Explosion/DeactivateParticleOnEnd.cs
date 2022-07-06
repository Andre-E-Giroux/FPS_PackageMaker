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
            foreach (ParticleSystem particleSystems in m_particleSystem)
            {
                if(particleSystems.IsAlive())
                {
                    return;
                }
            }

            m_particleGameObject.SetActive(false);
        }
    }
}
