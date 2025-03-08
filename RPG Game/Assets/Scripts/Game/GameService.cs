using RPG.Control;
using RPG.Events;
using UnityEngine;

namespace RPG
{
    public class GameService : MonoBehaviour
    {
        // Event Service 
        private EventService eventService;
        [SerializeField]private PlayerController playerCore;
            
        // UI Service (manage HUD)


        // Start is called before the first frame update
        void Start()
        {
            Initialize();
            DependencyDistribution();
        }

        private void Initialize()
        {
           eventService = new EventService();
        }

        private void DependencyDistribution()
        {
            playerCore.Init(eventService);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}