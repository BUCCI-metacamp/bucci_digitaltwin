
using UnityEngine;
using UnityEngine.UI;

namespace Edukit
{
    public class MainPanel_QuitChecker : MonoBehaviour
    {
        public Button quit;
        private void Awake()
        {           
            quit.onClick.AddListener(OnClickQuit);           
        }

        void OnClickQuit()
        {
            Application.Quit();
        }

     
    }
}