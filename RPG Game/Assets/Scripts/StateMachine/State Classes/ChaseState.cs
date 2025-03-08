using RPG.Attributes;
using RPG.Control;
using RPG.Movement;
using RPG.PickUp;
using UnityEngine;

namespace RPG.FSM
{
    public class ChaseState : IState
    {
        private AIController ai;
        private EnemyStateMachine stateMachine;
        private Mover mover;
        private AIConfig aiConfig;
        private Fighter fighter;
        private Health targetHealth;
        

        public ChaseState(AIController ai, EnemyStateMachine stateMachine)
        {
            this.ai = ai;
            this.stateMachine = stateMachine;
        }

        public void OnStateEnter()
        {
            // Cache commonly used components
            mover = ai.GetMover();
            aiConfig = ai.GetAIConfig();
            fighter = ai.GetFighter();

            //Cache the target to avoid repeated calls
            targetHealth = ai.GetTarget();
        }

        public void Update()
        {
            if (ai.GetHealth().IsDead())
            {
                stateMachine.ChangeState(StateEnum.IDLE);
                return;
            }
            if (ai.ShouldFlee())
            {
                StopMoving();
                stateMachine.ChangeState(StateEnum.FLEEING);
                return;
            }

            if (fighter.CanAttack(targetHealth.gameObject))
            {
                StopMoving();
                stateMachine.ChangeState(StateEnum.ATTACKING);
                return;
            }

            else if (ai.TimeSinceLastSawPlayer < ai.GetSuspicionTime())
            {
                StopMoving();
                stateMachine.ChangeState(StateEnum.SUSPICION);
                return;
            }


            // Move toward the player
            mover.StartMoveAction(targetHealth.transform.position, 1f);
        }

        public void OnStateExit()
        {
            ai.TimeSinceLastSawPlayer = 0;
            StopMoving(); // Stop movement when switching states
        }
        private void StopMoving()
        {
            mover.Cancel(); //  Stops AI movement before switching states
        }
    }
}