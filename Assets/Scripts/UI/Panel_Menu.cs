
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Edukit
{
    public class Panel_Menu : MonoBehaviour
    {
        public event Action onClickQuit;
        public event Action onClickOption;
        private void Awake()
        {
            Button option = transform.Find(nameof(option)).GetComponent<Button>();
            Button forceReset = transform.Find(nameof(forceReset)).GetComponent<Button>();
            Button quit = transform.Find(nameof(quit)).GetComponent<Button>();
            option.onClick.AddListener(OnClickOption);
            forceReset.onClick.AddListener(OnClickForceReset);
            quit.onClick.AddListener(OnClickQuit);
        }

        void OnClickOption()
        {
            onClickOption?.Invoke();
        }
        void OnClickForceReset()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
        void OnClickQuit()
        {
            onClickQuit?.Invoke();
        }
    }
}