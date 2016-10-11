using System;
using static System.Int32;

namespace PerformanceTest
{
    public interface ITest
    {
        void Test();
    }

    public enum Modes
    {
        Exit,
        DebugFile,
        DebugWindow,
        StringInternation,
        Generics,
        Unknown = MaxValue
    }
    public static class ModesExtension
    {
        public static Modes GetMode(this string value)
        {
            Modes i;
            // ReSharper disable once RedundantTypeArgumentsOfMethod
            return Enum.TryParse<Modes>(value, out i) ? i : Modes.Unknown;
        }
    }

    internal class Program
    {
        static void Main()
        {
            do
            {
                foreach (Modes m in Enum.GetValues(typeof(Modes)))
                {
                    if(m == Modes.Unknown)continue;
                    Console.WriteLine($"{(int)m}-{m}");
                }
                
                ITest iTest = null;
                switch (Console.ReadLine().GetMode())
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
