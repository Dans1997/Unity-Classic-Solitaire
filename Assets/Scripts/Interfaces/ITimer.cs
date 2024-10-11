using System;

namespace Interfaces
{
    public interface ITimer
    {
        bool IsRunning { get; }
        bool IsPaused { get; }
        float ElapsedTime { get; }
        
        void StartTimer(Action callback = null, float callbackInterval = 0f);
        void StopTimer();
        void PauseTimer();
        void ResumeTimer();
    }
}