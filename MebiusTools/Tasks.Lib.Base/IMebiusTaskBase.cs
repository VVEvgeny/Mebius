using System.Collections.Generic;

namespace Tasks.Lib.Base
{
    public interface IMebiusTaskBase
    {
        string Name { get; }
        string ErrorResult { get; }
        string Exec(string param);
        IEnumerable<string> GetResults { get; }
    }
}
