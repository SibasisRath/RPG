using RPG.SceneManagement;
using RPG.Utils;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        LazyValue<SavingWrapper> savingWrapper;
        [SerializeField] private TMP_InputField newGameName;
        private void Awake()
        {
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }
        public void ContinueGame()
        {
            savingWrapper.value.ContinueGame();
        }
        public void NewGame()
        {
            savingWrapper.value.NewGame(newGameName.text);
        }

         public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
       
    }
}