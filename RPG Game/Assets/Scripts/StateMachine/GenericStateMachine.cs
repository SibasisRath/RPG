using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericStateMachine<T> where T : MonoBehaviour
{
    private IState currentState;
    protected T owner;
    protected Dictionary<States, IState> States = new Dictionary<States, IState>();

    public GenericStateMachine(T owner)
    {
        this.owner = owner;
    }

    public void SetState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.HandleInput();
        currentState?.UpdateLogic();
    }

    public void FixedUpdate()
    {
        currentState?.UpdatePhysics();
    }
}
