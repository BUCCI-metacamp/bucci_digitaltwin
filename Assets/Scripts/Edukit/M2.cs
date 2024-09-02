using System;
using UnityEngine;

namespace Edukit
{
    public class M2 : SubMachine
    {
        public string No2Standby;
        public string No2SensingMemory;
        public string No2PowerState;
        public string No2Count;
        public string No2Chip;
        public string No2CubeFull;
        public string No2InPoint;
        public string No2OutPoint;
        public string No2Sol;
        public string No2SolAction;
        public string No2BackToSquare;
        public string No2OperationMode;

        public Transform stackCube;
        public EdukitDice unknownCube;
        public event Action<EdukitDice> onCreateDice;
        
        Animator animator;

        private new void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            animator.speed = 0.2f;
        }

        protected override void VariableChangeEvent(string variableName, string variableValue)
        {
            switch (variableName)
            {
                case nameof(No2Sol):
                    //Debug.Log($"No2SolAction:{variableValue}");
                    if (!isTrue(variableValue))
                    {
                        return;
                    }
                    var nc = Instantiate(unknownCube, unknownCube.transform.parent);
                    nc.SetValue(-1);

                    stackCube.gameObject.SetActive(!isTrue(No2CubeFull));

                    unknownCube.gameObject.SetActive(false);
                    onCreateDice?.Invoke(nc);
                    break;

                //case nameof(No2SolAction):
                //    //Debug.Log($"No2SolAction:{variableValue}");
                //    break;

                case nameof(No2BackToSquare):
                    if (isTrue(variableValue))
                    {
                        animator.Play("In");
                    }
                    break;

                case nameof(No2InPoint):
                    //Debug.Log($"BB_{variableValue}");
                    if (isTrue(variableValue))
                    {
                        if(isTrue(No2OutPoint))
                        {
                            animator.Play("Out");
                        }
                        return;
                    }

                    SetDice();
                    break;
            }
        }

        internal void SetDice()
        {
            //animator.Play("Out");
            unknownCube.gameObject.SetActive(true);
        }
    }
}