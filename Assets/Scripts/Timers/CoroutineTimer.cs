using System;
using System.Collections;
using Interfaces;
using UnityEngine;

namespace Timers
{
    public class CoroutineTimer : ITimer
    {
        public bool IsRunning { get; private set; }
        public bool IsPaused { get; private set; }
        public float ElapsedTime { get; private set; }

        private readonly MonoBehaviour monoBehaviour;
        private Action callback;
        private float callbackInterval;
        private Coroutine timerCoroutine;

        public CoroutineTimer(MonoBehaviour monoBehaviour, Action callback = null, float callbackInterval = 0f)
        {
            this.monoBehaviour = monoBehaviour;
            this.callback = callback;
            this.callbackInterval = callbackInterval;
        }

        public void StartTimer(Action callback = null, float callbackInterval = 0f)
        {
            this.callback = callback;
            this.callbackInterval = callbackInterval;
            
            ElapsedTime = 0;
            IsRunning = true;
            IsPaused = false;
            timerCoroutine = monoBehaviour.StartCoroutine(TimerRoutine());
        }

        public void StopTimer()
        {
            if (timerCoroutine != null)
                monoBehaviour.StopCoroutine(timerCoroutine);
            IsRunning = false;
            ElapsedTime = 0;
        }

        public void PauseTimer()
        {
            if (IsRunning && !IsPaused)
                IsPaused = true;
        }

        public void ResumeTimer()
        {
            if (IsRunning && IsPaused)
                IsPaused = false;
        }

        private IEnumerator TimerRoutine()
        {
            var callbackTime = 0f;
            while (true)
            {
                if (!IsPaused)
                {
                    ElapsedTime += Time.deltaTime;
                    callbackTime += Time.deltaTime;
                
                    if (callback != null && callbackInterval > 0 && callbackTime >= callbackInterval)
                    {
                        callbackTime = 0;
                        callback.Invoke();
                    }
                }
                yield return null;
            }
        }
    }
}