using System.Collections.Generic;

namespace Tasks.Lib.Base
{
    public interface IMebiusTaskBase
    {
        string Name { get; }
        string ErrorResult { get; }
        string Exec(string param, string settings);
        IEnumerable<string> GetResults { get; }
        bool HaveSettings { get; }
        string GetSettings();
    }
}
