using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the abstract class of Manager assistant that will assist the GameManager when on scene
/// </summary>
public abstract class ManagerAssistant : MonoBehaviour
{
    public GameManager headManager;

    /// <summary>
    /// Start the Manager assistant
    /// </summary>
    public abstract void StartManager();

}
