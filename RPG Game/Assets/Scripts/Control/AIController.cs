using RPG.PickUp;
using RPG.Movement;
using UnityEngine;
using RPG.Attributes;
using RPG.FSM;
using RPG.Saving;

namespace RPG.Control
{
    public class AIController : MonoBehaviour, ISaveable
    {
        [SerializeField] protected AIConfig aiConfig;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private Fighter fighter;
        [SerializeField] private Health health;
        [SerializeField] private Mover mover;
        protected GameObject player;
        private float timeSinceAggrevated = Mathf.Infinity;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        protected EnemyStateMachine stateMachine;

        public float TimeSinceAggrevated { get => timeSinceAggrevated; set => timeSinceAggrevated = value; }
        public float TimeSinceLastSawPlayer { get => timeSinceLastSawPlayer; set => timeSinceLastSawPlayer = value; }
        public float TimeSinceArrivedAtWaypoint { get => timeSinceArrivedAtWaypoint; set => timeSinceArrivedAtWaypoint = value; }

        private void UpdateTimers()
        {
            TimeSinceLastSawPlayer += Time.deltaTime;
            TimeSinceArrivedAtWaypoint += Time.deltaTime;
            TimeSinceAggrevated += Time.deltaTime;
        }

        protected virtual void Awake()
        {
            player = GameObject.FindWithTag("Player");
            stateMachine = new EnemyStateMachine(this);
        }

        protected virtual void Update()
        {
            stateMachine.Update();
            UpdateTimers();
        }
        public bool IsAggrevated()
        {
           /* print("Distance between player and enemy = " + Vector3.Distance(player.transform.position, transform.position));
            print("enemy chase Distance = " + aiConfig.chaseDistance);
            print("Time Sience aggr = " + TimeSinceAggrevated);
            print("aggro cool down time = " + aiConfig.agroCooldownTime);*/
            return Vector3.Distance(player.transform.position, transform.position) < aiConfig.chaseDistance || TimeSinceAggrevated < aiConfig.agroCooldownTime;
        }
        public void Aggrevate() => TimeSinceAggrevated = 0;


        public Health GetTarget() => player.GetComponent<Health>();
        public float GetSuspicionTime() => aiConfig.suspicionTime;
        public float GetChaseDistance() => aiConfig.chaseDistance;
        public bool ShouldFlee() => health.GetFraction() < aiConfig.fleeHealthThreshold;
        public void Recover() => stateMachine.ChangeState(StateEnum.PATROLLING);
        public Mover GetMover() => mover;
        public Fighter GetFighter() => fighter;
        public Health GetHealth() => health;
        public PatrolPath GetPatrolPath() => patrolPath;
        public AIConfig GetAIConfig() => aiConfig;

        public object CaptureState()
        {
            return new SaveData
            {
                savedState = stateMachine.GetCurrentState(),
                position = new SerializableVector3(transform.position),
                isDead = health.IsDead()
            };
        }

        public void RestoreState(object state)
        {
            SaveData saveData = (SaveData)state;
            transform.position = saveData.position.ToVector();
            if (saveData.isDead)
            {
                health.TakeDamage(gameObject, health.GetHealthPoints()); // Kill enemy instantly
                stateMachine.ChangeState(StateEnum.IDLE);
            }
            else
            {
                stateMachine.ChangeState(saveData.savedState);
            }
        }

        [System.Serializable]
        struct SaveData
        {
            public StateEnum savedState;
            public SerializableVector3 position;
            public bool isDead;
        }
    }
}