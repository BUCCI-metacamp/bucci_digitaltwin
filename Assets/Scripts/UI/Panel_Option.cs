
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Edukit
{
    public class Panel_Option : MonoBehaviour
    {
        public TMP_InputField ipInput;
        public TMP_InputField portInput;
        public TMP_InputField topicInput;

        public event Action<string, string, string> onClickConnect;
        private void Awake()
        {
            ipInput = transform.Find(nameof(ipInput)).GetComponentInChildren<TMP_InputField>();
            portInput = transform.Find(nameof(portInput)).GetComponentInChildren<TMP_InputField>();
            topicInput = transform.Find(nameof(topicInput)).GetComponentInChildren<TMP_InputField>();
            
            Button save = transform.Find(nameof(save)).GetComponent<Button>();
            Button close = transform.Find(nameof(close)).GetComponent<Button>();
            Button connect = transform.Find(nameof(connect)).GetComponent<Button>();

            save.onClick.AddListener(OnClickSave);
            close.onClick.AddListener(OnClickClose);
            connect.onClick.AddListener(OnClickConnect);
        }

        public event Action<string, string, string> onClickSave;
        void OnClickSave()
        {
            //Debug.Log($"OnClickSave: {ipInput.text}, {portInput.text}, {topicInput.text}");
            onClickSave?.Invoke(ipInput.text, portInput.text, topicInput.text);
        }

        void OnClickConnect()
        {
            //Debug.Log($"OnClickConnect: {ipInput.text}, {portInput.text}, {topicInput.text}");
            onClickConnect?.Invoke(ipInput.text, portInput.text, topicInput.text);
        }

        void OnClickClose()
        {
            gameObject.SetActive(false);
        }

        internal void SetDefaultValues(string host, string port, string v)
        {
            ipInput.text = "";
            ipInput.text= host;

            portInput.text = "";
            portInput.text = port;
            
            topicInput.text = "";
            topicInput.text= v;
            //ipInput.SetTextWithoutNotify(host);
            //portInput.SetTextWithoutNotify(port.ToString());
            //topicInput.SetTextWithoutNotify(v);
        }
    }
}