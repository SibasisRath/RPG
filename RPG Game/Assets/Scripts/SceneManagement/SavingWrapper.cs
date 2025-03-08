using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        //const string defaultSaveFile = "save";
        private const string currentSaveKey = "CurrentSaveName";
        [SerializeField] int firstLevelBuildIndex = 1;
        [SerializeField] int menuLevelBuildIndex = 0;
        [SerializeField] float fadeInTime = 0.2f;
        [SerializeField] float fadeOutTime = 0.2f;

        public void ContinueGame()
        {
            if (!PlayerPrefs.HasKey(currentSaveKey)) return;
            if (!GetComponent<SavingSystem>().SaveFileExists(GetCurrentSave())) return ;
            StartCoroutine(LoadLastScene());
        }

        public IEnumerator LoadLastScene()
        {
            Fade fader = FindObjectOfType<Fade>();
            yield return fader.FadeOut(fadeOutTime);
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());
            yield return fader.FadeIn(fadeInTime);
        }

        public void NewGame(string newGameName)
        {
            if (String.IsNullOrEmpty(newGameName)) return;
            SetCurrentSave(newGameName);
            StartCoroutine (LoadFastScene());
        }
        public void LoadMenu()
        {
            StartCoroutine(LoadMenuScene());
        }
        private IEnumerator LoadMenuScene()
        {
            Fade fader = FindObjectOfType<Fade>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(menuLevelBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }

        private void SetCurrentSave(string newGameName)
        {
            PlayerPrefs.SetString(currentSaveKey, newGameName);
        }
        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        private IEnumerator LoadFastScene()
        {
            Fade fader = FindObjectOfType<Fade>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(firstLevelBuildIndex); // this is default first scene.
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0) return;
            if (Input.GetKeyDown(KeyCode.S)) Save();
            if (Input.GetKeyDown(KeyCode.L)) Load();
            if (Input.GetKeyDown(KeyCode.Delete)) Delete();
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(GetCurrentSave());
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(GetCurrentSave());
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(GetCurrentSave());
        }

        public void LoadGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            ContinueGame();
        }
        public IEnumerable<string> ListSaves()
        {
            return GetComponent<SavingSystem>().ListSaves();
        }
    }
}
