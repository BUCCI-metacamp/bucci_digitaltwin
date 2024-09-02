
using System;

namespace Factory
{
    public class CaseETC : CaseSubMachine
    {
        public string StartState;
        public string ResetState;

        public string EmergencyState;
        public string InputLimit;
        public string DataTime;

        public static long currentTick;
        public static long prevTick;
        public static long tick;
        DateTime cd;
        public static float deltaTime;
        public static string prevDate;
        public static string dateTime;
        public static long playTick;
        long startTick;
        bool init;
        internal Action onResetEvent;
        internal Action<bool> onGoStopEvent;

        protected override void VariableChangeEvent(string variableName, string variableValue)
        {
            switch(variableName)
            {
                case nameof(StartState):
                    onGoStopEvent?.Invoke(isTrue(variableValue));
                    break;
                case nameof(ResetState):
                    if (isTrue(variableValue))
                    {
                        onResetEvent?.Invoke();
                    }
                    break;
                case nameof(DataTime):
                    
                    prevDate = dateTime;
                    dateTime = variableValue;
                    cd = DateTime.Parse(variableValue);
                    prevTick = currentTick;
                    tick = cd.Ticks - prevTick;
                    currentTick = cd.Ticks;
                    if (!init)
                    {
                        init = true;
                        startTick = currentTick;
                    }
                    playTick = currentTick - startTick;
                    deltaTime = tick / TimeSpan.TicksPerMillisecond;
                    break;
            }
        }
    }
}