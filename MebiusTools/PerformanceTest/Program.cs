using System;
using static System.Int32;

namespace PerformanceTest
{
    public interface ITest
    {
        void Test();
    }

    internal class Program
    {
        private enum Modes
        {
            Exit,
            DebugFile,
            DebugWindow,
            StringInternation,
            Generics,
            Unknown = MaxValue
        }
        static void Main()
        {
            do
            {
                foreach (Modes m in Enum.GetValues(typeof(Modes)))
                {
                    if(m == Modes.Unknown)continue;
                    Console.WriteLine($"{(int)m}-{m}");
                }

                var command = Console.ReadLine();

                
                int i;
                var mode = TryParse(command, out i) ? (Modes)Convert.ToInt32(command) : Modes.Unknown;
                ITest iTest = null;
                switch (mode)
                {
                    case Modes.Exit: return;
                    case Modes.DebugFile:
                        iTest = new BmDebugFile();
                        break;
                    case Modes.DebugWindow:
                        iTest = new BmDebugLogWindow();
                        break;
                    case Modes.StringInternation:
                        iTest = new OthersStringInternation();
                        break;
                    case Modes.Unknown:
                        break;
                    case Modes.Generics:
                        iTest = new OthersGenerics();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                iTest?.Test();
            } while (true);
        }
    }
}
