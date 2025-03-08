using RPG.Control;
using UnityEngine;

namespace RPG.FSM
{
    public class SuspicionState : IState
    {
        private AIController ai;
        private EnemyStateMachine stateMachine;
        private float timeSpentInSuspicion = 0;

        public SuspicionState(AIController ai, EnemyStateMachine stateMachine)
        {
            this.ai = ai;
            this.stateMachine = stateMachine;
        }

        public void OnStateEnter()
        {
            timeSpentInSuspicion = 0;
            ai.GetMover().Cancel(); // Stop moving and look around
        }

        public void Update()
        {
            timeSpentInSuspicion += Time.deltaTime;
            if (ai.GetHealth().IsDead())
            {
                stateMachine.ChangeState(StateEnum.IDLE);
                return;
            }

            if (ai.IsAggrevated())
            {
                stateMachine.ChangeState(StateEnum.CHASING);
                return;
            }

            if (timeSpentInSuspicion >= ai.GetSuspicionTime())
            {
                stateMachine.ChangeState(StateEnum.PATROLLING);
                return;
            }
        }
        public void OnStateExit() { }
    }
}