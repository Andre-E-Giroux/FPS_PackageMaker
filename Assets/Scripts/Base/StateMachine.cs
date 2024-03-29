using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract state machine, used as a base for other state machine
/// </summary>
public class StateMachine : MonoBehaviour
{
    
    protected BaseState currentState;

    public bool allowStatemachine = true;

    // Start is called before the first frame update
    void Start()
    {
        currentState = GetInitialState();
        if (currentState != null)
            currentState.Enter();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (currentState != null && allowStatemachine)
            currentState.UpdateLogic();
    }

    private void LateUpdate()
    {
        if (currentState != null && allowStatemachine)
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

    /// <summary>
    /// Get the initial state of the state machine
    /// </summary>
    /// <returns>Initial state</returns>
    protected virtual BaseState GetInitialState()
    {
        return null;
    }


    /// <summary>
    /// Get the current state of the state machine
    /// </summary>
    /// <returns>Initial state</returns>
    public BaseState GetCurrentState()
    {
         return currentState;
    }

    /// <summary>
    /// Stop the state machine
    /// </summary>
    /// <param name="stop"></param>
    public virtual void StopStateMachine(bool stop)
    {
        allowStatemachine = !stop;
    }
}
