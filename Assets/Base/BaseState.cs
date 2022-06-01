using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    public string name;
    protected StateMachine stateMachine;

    // Constructor
    public BaseState(string name, StateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }

    /// <summary>
    /// Logic executed on entering the state
    /// </summary>
    public virtual void Enter() { }

    /// <summary>
    /// Logic Executed on update while in the state every frame
    /// </summary>
    public virtual void UpdateLogic() { }

    /// <summary>
    /// Logic Executed on update while in the state every fixed tick
    /// </summary>
    public virtual void UpdatePhysics() { }

    /// <summary>
    /// Logic executed on exiting the state
    /// </summary>
    public virtual void Exit() { }
}
