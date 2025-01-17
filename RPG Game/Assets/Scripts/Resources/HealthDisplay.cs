using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }
        private void Update()
        {
            GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}%", health.GetPercentage());
        }
    }
}