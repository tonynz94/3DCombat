using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//if you add abstact to MonoBehaviour you can't add to component directly 
public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    public void SwitchState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    private void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }
}
