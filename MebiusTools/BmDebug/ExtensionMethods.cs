namespace BMTools
{
    public static class BmDebugExtension
    {
        public static object ToDebug(this object obj)
        {
            BmDebug.Info(obj);
            return obj;
        }
    }
}
