using UnityEngine;

namespace Utils
{
    public static class TimeUtils
    {
        public static string PrintTime(float timeInSeconds)
        {
            var minutes = Mathf.FloorToInt(timeInSeconds / 60);
            var seconds = Mathf.FloorToInt(timeInSeconds % 60);
            return $"{minutes:00}:{seconds:00}";
        }
    }
}