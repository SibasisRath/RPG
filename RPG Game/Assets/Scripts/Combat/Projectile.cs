using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.PickUp
{
    public class Projectile : MonoBehaviour
    {
        private Health target = null;
        private float damage = 0f;
        private GameObject instigator = null;

        [SerializeField] private float speed = 1f;
        [SerializeField] private bool isHoming = true;
        [SerializeField] private ParticleSystem hitEffect = null;

        [SerializeField] private float maxLifeTime = 10f;
        [SerializeField] private GameObject[] destroyOnHit = null;
        [SerializeField] private float lifeAfterImpact = 2f;

        [SerializeField] private UnityEvent OnHit;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }
        void Update()
        {
            if (isHoming)
            {
                if (!target.IsDead())
                {
                    transform.LookAt(GetAimLocation());
                }
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
            Destroy(gameObject, maxLifeTime);
        }
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }

            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) { return; }
            if (target.IsDead()) { return; }
            speed = 0;
            OnHit.Invoke();
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }
            target.TakeDamage(instigator, damage);
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}