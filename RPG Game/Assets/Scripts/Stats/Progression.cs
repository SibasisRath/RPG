using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterTypes = null;

        private Dictionary<CharacterEnum, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, CharacterEnum characterType, int level)
        {
            BuildLookup();
            float[] levels = lookupTable[characterType][stat];
            if (levels.Length < level) { return 0; }
            return levels[level - 1];
        }

        public int GetLevels(Stat stat, CharacterEnum characterClass)
        {
            BuildLookup();
            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) { return; }
            lookupTable = new();
            foreach (ProgressionCharacterClass progressionClass in characterTypes)
            {
                Dictionary<Stat, float[]> statLookupTable = new();
                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                lookupTable[progressionClass.characterType] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterEnum characterType;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}