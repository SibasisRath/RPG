using RPG.Attributes;
using RPG.Control;
using RPG.PickUp;
using UnityEngine;

namespace RPG.FSM
{
    public class AttackState : IState
    {

        private AIController ai;
        private EnemyStateMachine stateMachine;
        private Fighter fighter;
        private Health targetHealth;

        public AttackState(AIController ai, EnemyStateMachine stateMachine)
        {
            this.ai = ai;
            this.stateMachine = stateMachine;
        }

        public void OnStateEnter()
        {
            fighter = ai.GetFighter();
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
                stateMachine.ChangeState(StateEnum.FLEEING);
                return;
            }
            if (targetHealth == null || targetHealth.IsDead())
            {
                stateMachine.ChangeState(StateEnum.PATROLLING);
                return;
            }
            if (!fighter.CanAttack(targetHealth.gameObject))
            {
                stateMachine.ChangeState(StateEnum.CHASING);
                return;
            }
            else
            {
                ai.TimeSinceLastSawPlayer = 0;
                ai.GetFighter().StartAttackAction(ai.GetTarget().gameObject);
                AggrevateNearbyEnemies();
            }

        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(ai.transform.position, ai.GetAIConfig().shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;

                ai.Aggrevate();
            }
        }


        public void OnStateExit() => ai.GetFighter().Cancel();
    }
}