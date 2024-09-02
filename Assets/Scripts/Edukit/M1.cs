
using System;
using TMPro;
using UnityEngine;

namespace Edukit
{
    public class M1 : SubMachine
    {
        public string No1ChipEmpty;
        public string No1Push;
        public string No1PowerState;
        public string No1DelayTime;
        public string No1Count;
        public string No1ChipFull;

        [SerializeField]
        EdukitChip stackChip;
        [SerializeField]
        Transform chipSpawnPoint;
        [SerializeField]
        Transform pusher;

        bool pushing;
        int prevCount;

        public event Action onCountReset;
        public event Action<EdukitChip> onCreateChip;

        private int initialValue = int.MaxValue;

        public void StartRun()
        {                            
            GetComponent<Animator>().Play("Push");  
        }
        protected override void VariableChangeEvent(string variableName, string variableValue)
        {
            switch (variableName)
            {
                case nameof(No1ChipEmpty):
                    stackChip.gameObject.SetActive(!isTrue(variableValue));
                    break;
                case nameof(No1Push):
                    Debug.Log("Push");
                    if (isTrue(variableValue))
                    {
                        if (!pushing)
                        {
                            GetComponent<Animator>().Play("Push");
                            pushing = true;
                        }
                    }
                    else
                    {
                        GetComponent<Animator>().Play("Idle");
                        pushing = false;
                    }
                    break;

                case nameof(No1Count):
                    int parse = int.Parse(variableValue);
                    
                    if (initialValue > parse)
                    {
                        initialValue = parse;
                        prevCount = initialValue;
                        return;
                    }

                    if (prevCount > parse)
                    {
                        onCountReset?.Invoke();
                        return;
                    }

                    prevCount = parse;
                    var chip = Instantiate(chipSpawnPoint).GetComponent<EdukitChip>();
                    chip.gameObject.SetActive(true);
                    onCreateChip?.Invoke(chip);
                    break;
            }
        }
    }
}