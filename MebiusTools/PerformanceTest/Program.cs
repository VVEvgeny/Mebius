using System.Collections;
using System.Collections.Generic;

namespace PerformanceTest
{
    class Program
    {
        static void Main()
        {
            //string internation

            string s1 = "adsasdasd";
            string[]
                ms =
                {
                    "adsasdasd",
                    "aaaa",
                    "adsasdasd",
                    "aaaaa",
                    "adsasdasd",
                    "aaaaaaaaaaaa",
                    "adsasdasd",
                    "aaaaaaaaa"
                };

            var iterations = 100000000;
            using (new OperationTimer("compare strings"))
            {
                for (int i = 0; i < iterations; i++)
                {
                    foreach (var v in ms)
                    {
                        s1.Equals(v);
                    }
                }
            }


            using (new OperationTimer("compare strings internation"))
            {
                s1 = string.Intern(s1);
                for (int index = 0; index < ms.Length; index++)
                {
                    ms[index] = string.Intern(ms[index]);
                }

                for (int i = 0; i < iterations; i++)
                {
                    foreach (var v in ms)
                    {
                        ReferenceEquals(s1, v);
                    }
                }
            }

            using (new OperationTimer("compare strings internation interned"))
            {
                for (int i = 0; i < iterations; i++)
                {
                    foreach (var v in ms)
                    {
                        ReferenceEquals(s1, v);
                    }
                }
            }



            /*
            using (new OperationTimer("generic List value type"))
            {
                var iterations = 100000;
                List<int> d= new List<int>(iterations);
                for (int i = 0; i < iterations; i++)
                {
                    d.Add(i);
                }
            }
            using (new OperationTimer("generic List ref type"))
            {
                var iterations = 100000;
                List<string> d = new List<string>(iterations);
                for (int i = 0; i < iterations; i++)
                {
                    d.Add(i.ToString());
                }
            }

            using (new OperationTimer("not generic List value type"))
            {
                var iterations = 100000;
                ArrayList al =   new ArrayList();

                for (int i = 0; i < iterations; i++)
                {
                    al.Add(i);
                }
            }
            using (new OperationTimer("not generic List ref type"))
            {
                var iterations = 100000;
                ArrayList al = new ArrayList();

                for (int i = 0; i < iterations; i++)
                {
                    al.Add(i.ToString());
                }
            }
            */

            /*
            var iterations = 100;

            BMTools.BmDebug.ClassUsing = "test1";
 
            BMTools.BmDebug.Enabled = false;
            BMTools.BmDebug.DebugOutput = BMTools.BmDebug.DebugOutputModes.File;
            for (var i = 0; i < iterations; i++)//прогрев
            {
                BMTools.BmDebug.Info("test");
            }
            using (new OperationTimer("debug Disabled call reference file"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Info("test");
                }
            }
            using (new OperationTimer("debug Disabled call value file"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Info(105);
                }
            }

            BMTools.BmDebug.DebugOutput = BMTools.BmDebug.DebugOutputModes.LogWindow;
            for (var i = 0; i < iterations; i++)//прогрев
            {
                BMTools.BmDebug.Info("test");
            }
            using (new OperationTimer("debug Disabled call reference log window"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Info("test");
                }
            }
            using (new OperationTimer("debug Disabled call value log window"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Info(105);
                }
            }

            BMTools.BmDebug.Enabled = true;
            BMTools.BmDebug.DebugOutput = BMTools.BmDebug.DebugOutputModes.File;
            for (var i = 0; i < iterations; i++)//прогрев
            {
                BMTools.BmDebug.Info("test");
            }
            using (new OperationTimer("debug Enabled call reference file"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Info("test");
                }
            }
            using (new OperationTimer("debug Enabled call value file"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Info(105);
                }
            }

            BMTools.BmDebug.DebugOutput = BMTools.BmDebug.DebugOutputModes.LogWindow;
            for (var i = 0; i < iterations; i++)//прогрев
            {
                BMTools.BmDebug.Info("test");
            }
            using (new OperationTimer("debug Enabled call reference log window"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Info("test");
                }
            }
            using (new OperationTimer("debug Enabled call value log window"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Info(105);
                }
            }
            */
        }
    }
}
