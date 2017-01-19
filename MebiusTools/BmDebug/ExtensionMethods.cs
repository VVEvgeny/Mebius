using static BMTools.BmDebug;

namespace BMTools
{
    public static class BmDebugExtension
    {
        public static object ToDebug(this object obj)
        {
            Debug.InfoAsync(obj);
            return obj;
        }
    }
}
