using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float currentHealth = 100;
    public const float MAX_HEALTH = 100;
    public void AddHealth(float increase)
    {
        if (currentHealth + increase > MAX_HEALTH)
            currentHealth = MAX_HEALTH;
        else if (currentHealth + increase < 0)
            currentHealth = 0;
        else
            currentHealth += increase;
    }

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = MAX_HEALTH;
    }
}
