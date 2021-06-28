using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Die StateMachine dient der Strukturierung des Programms in Teilaufgaben.
/// Sie sollte höchtens einmal pro Manager initialisiert werden.
/// </summary>
public class StateMachine
{
    private IState currentState;
    private IState previousState;

    /// <summary>
    /// Change StateMachine to the new State. Calls the "Exit" function of the previous state. 
    /// and the "Enter" function of the new State.
    /// </summary>
    /// <param name="newState"> New State, which inherits from IState. </param>
    public void ChangeState(IState newState)
    {
        if (this.currentState != null)
        {
            this.currentState.Exit();
        }
        // set previous State
        this.previousState = this.currentState;

        // set current State
        this.currentState = newState;
        this.currentState.Enter();
    }

    /// <summary>
    /// Called by the Monobehaviour Update Method
    /// </summary>
    public void ExecuteStateUpdate()
    {
        var runningState = this.currentState;
        if (runningState != null)
        {
            runningState.Execute();
        }
    }

    /// <summary>
    /// support method for user button
    /// </summary>
    public void SwitchToNextState()
    {
        if(currentState.GetType() == typeof(PriceTest))
        {
            ChangeState(new PriceEstimation()); 
        }
        else if(currentState.GetType() == typeof(PriceEstimation))
        {
            ChangeState(new Pause());
        }
        else if (currentState.GetType() == typeof(LocationTest))
        {
            ChangeState(new LocationEstimation());
        }
        else if (currentState.GetType() == typeof(LocationEstimation))
        {
            ChangeState(new End());
        }
        else
        {
            Debug.LogError("StateMachine::SwitchToNextState current state not supported."); 
        }
    }

    /// <summary>
    /// Comment: Not used at this point, but maybe on later versions.
    /// </summary>
    public void SwitchToPreviousState()
    {
        this.currentState.Exit();
        this.currentState = this.previousState;
        this.currentState.Enter();
    }

    /// <summary>
    /// Set StateMachine into Idle, e.g. on the end of the game
    /// </summary>
    public void SwitchToIdle()
    {
        this.previousState = currentState;
        if (currentState != null)
        {
            this.currentState.Exit();
            this.currentState = null;
        }
    }
}
