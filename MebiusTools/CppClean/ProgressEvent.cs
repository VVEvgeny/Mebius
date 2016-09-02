namespace CppClean
{
    public enum ProgressEventType
    {
        Update = 1,
        End = 2,
        Start = 3
    }

    // A delegate type for hooking up change notifications.
    public delegate void ProgressEventHandler(string text, int percent, ProgressEventType type);

    public static class ProgressEvent
    {
        public static event ProgressEventHandler Event;
        public static void Exec(string text, int percent, ProgressEventType eventType)
        {
            Event?.Invoke(text, percent, eventType);
        }
    }
}
