using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterEnum characterType;
        [SerializeField] Progression progression = null;

        public float GetHealth()
        {
            if (progression == null) 
            {
                print("No progression Info.");
                return 0;
            }
            return progression.GetHealth(characterType, startingLevel);
        }

        public float GetExperienceReward()
        {
            return 10;
        }
    }
}