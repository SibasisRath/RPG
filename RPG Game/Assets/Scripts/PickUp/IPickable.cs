using UnityEngine;

namespace RPG.PickUp
{
    [System.Serializable]
    public abstract class IPickable : MonoBehaviour
    {
        public abstract void ApplyEffect(GameObject player);
    }
}