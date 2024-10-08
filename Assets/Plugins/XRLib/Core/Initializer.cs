using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WI
{
    public partial class Initializer : MonoBehaviour,ISingle
    {
        Dictionary<MonoBehaviour, bool> activeTable;
        public List<MonoBehaviour> targets = new List<MonoBehaviour>();
        public List<BaseUI> baseUIs = new();
        HashSet<BaseUI> allUIs = new();
        HashSet<CanvasBase> canvasBases = new HashSet<CanvasBase>();
        Dictionary<CanvasBase, List<PanelBase>> canvasToPanels = new Dictionary<CanvasBase, List<PanelBase>>();
        void GetAllInitializer()
        {
            var sceneRootObjs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

            targets.Clear();
            foreach (var obj in sceneRootObjs)
            {
                var inits = obj.transform.FindAll<MonoBehaviour>();
                targets.AddRange(inits);
            }
        }

        void Awake()
        {
            //Debug.Log("Initializing");
            GetAllInitializer();
            ForceActive();
        }

        void ForceActive()
        {
            activeTable = new Dictionary<MonoBehaviour, bool>();
            foreach (var target in targets)
            {
                activeTable.Add(target, target.gameObject.activeSelf);
                target.gameObject.SetActive(true);
            }

            foreach (var a in activeTable)
            {
                //if (a.Key.gameObject == null)
                //{
                //    Debug.LogError($"Wrong Object!?");
                //    continue;
                //}

                if (a.Key is BaseUI ui)
                    SetUI(ui);

                if (a.Value == false)
                    a.Key.gameObject.SetActive(false);
            }
        }

        void SetUI(BaseUI ui)
        {
            if (!allUIs.Add(ui))
            {
                Debug.LogError($"Duplicate Initialize. {ui}");
                return;
            }

            if (ui is CanvasBase cb)
            {
                if (!canvasBases.Add(cb))
                {
                    Debug.LogError($"Canvas Initialize Failed! {cb}");
                    return;
                }
                canvasToPanels.Add(cb, new());
                return;
            }

            if (ui is PanelBase pb)
            {
                var parentCanvas = pb.GetComponentInParent<CanvasBase>();
                if (canvasBases.Add(parentCanvas))
                {
                    canvasToPanels.Add(parentCanvas, new List<PanelBase>());
                }

                canvasToPanels[parentCanvas].Add(pb);

                var panelType = pb.GetType();
                var canvasPanelField = parentCanvas.GetType().GetField(panelType.Name.ToLower(), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                if (canvasPanelField != null)
                    canvasPanelField.SetValue(parentCanvas, pb);
            }
        }
    }
}