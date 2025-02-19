using RPG.Attributes;
using System;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter fighter;
        [SerializeField] private TextMeshProUGUI enemyHealthText;
        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }
        private void Update()
        {
            if (fighter.GetTarget() == null)
            {
                enemyHealthText.text = "N/A";
                return;
            }
            Health health = fighter.GetTarget();
            enemyHealthText.text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
    }
}