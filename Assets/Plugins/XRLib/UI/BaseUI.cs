using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WI
{
    public abstract class BaseUI : MonoBehaviour
    {
        public RectTransform rectTransform => GetComponent<RectTransform>();
        public event Action<BaseUI> onEnableEvent;
        public event Action<BaseUI> onDisableEvent;

        public void SetActive(bool value)
        {
            if (value)
            {
                onEnableEvent?.Invoke(this);
            }
            else
            {
                onDisableEvent?.Invoke(this);
            }
            gameObject.SetActive(value);
        }
    }
}
