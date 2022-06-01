using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Abstract state machine, used as a base for other state machine
/// </summary>
public class StateMachine : MonoBehaviour
{
    
    protected BaseState currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = GetInitialState();
        if (currentState != null)
            currentState.Enter();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
            currentState.UpdateLogic();
    }

    private void LateUpdate()
    {
        if (currentState != null)
            currentState.UpdatePhysics();
    }

    /// <summary>
    /// change currentState to the state in paramater
    /// </summary>
    /// <param name="newState">The new state that will replace the currentstate</param>
    public void ChangeState(BaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }

  

}
