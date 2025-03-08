using RPG.Control;
using System.Collections.Generic;

namespace RPG.FSM
{
    public class GenericStateMachine<T> where T : AIController
    {
        protected T Owner;
        protected IState currentState;
        protected Dictionary<StateEnum, IState> States = new();

        public GenericStateMachine(T owner)
        {
            Owner = owner;
            InitializeStates();
        }
        public void Update() => currentState?.Update();

        protected virtual void InitializeStates()
        {
            // Override this method in derived classes to set up specific states
        }

        protected void ChangeState(IState newState)
        {
            currentState?.OnStateExit();
            currentState = newState;
            currentState?.OnStateEnter();
        }

        public void ChangeState(StateEnum newState) => ChangeState(States[newState]);

        public StateEnum GetCurrentState()
        {
            foreach (var pair in States)
            {
                if (pair.Value == currentState) return pair.Key;
            }
            return StateEnum.IDLE;
        }
    }
}
