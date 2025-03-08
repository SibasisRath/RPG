using UnityEngine;

namespace RPG.UI
{
    public class ShowOrHideUI : MonoBehaviour
    {
        [SerializeField] KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] GameObject uiContainer;

        // Start is called before the first frame update
        void Start()
        {
            uiContainer.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                Toggle();
            }
        }

        public void Toggle()
        {
            if (uiContainer == null) return;
            uiContainer.SetActive(!uiContainer.activeSelf);
        }
    }
}