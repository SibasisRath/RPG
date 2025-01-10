using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterTypes = null;

        public float GetHealth(CharacterEnum characterType, int level)
        {
            foreach (ProgressionCharacterClass progressionClass in characterTypes)
            {
               /* if (progressionClass == null)
                {
                    Debug.Log("no progression.");
                }*/
                if (progressionClass.characterType == characterType)
                {
                   // return progressionClass.health[level - 1];
                }
            }
            return 0;
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