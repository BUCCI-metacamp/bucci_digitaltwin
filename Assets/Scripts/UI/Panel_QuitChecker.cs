
using UnityEngine;
using UnityEngine.UI;

namespace Edukit
{
    public class Panel_QuitChecker : MonoBehaviour
    {
        private void Awake()
        {
            Button quit = transform.Find(nameof(quit)).GetComponent<Button>();
            Button close = transform.Find(nameof(close)).GetComponent<Button>();

            quit.onClick.AddListener(OnClickQuit);
            close.onClick.AddListener(OnClickClose);
        }

        void OnClickQuit()
        {
            Application.Quit();
        }

        void OnClickClose()
        {
            gameObject.SetActive(false);
        }
    }
}