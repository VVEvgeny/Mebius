using System;
using System.Collections;
using System.Collections.Generic;

namespace PerformanceTest
{
    public sealed class OthersStringInternation : ITest
    {
        private readonly Random _random = new Random();
        private readonly string _template = "asdfghjkl";

        private string GenerateRandomWord()
        {
            var ret = string.Empty;
            for (var i = 0; i < _random.Next(1, 20); i++)
            {
                ret += _template[_random.Next(0, _template.Length - 1)].ToString();
            }
            return ret;
        }
        public void Test()
        {
            var s1 = GenerateRandomWord();
            string[] ms = new string[20];
            for (var i = 0; i < 20; i++)
            {
                if (i%5 == 0) ms[i] = s1;
                else ms[i] = GenerateRandomWord();
            }
           
            const int iterations = 100000000;
            using (new OperationTimer("compare strings"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    foreach (var v in ms)
                    {
                        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                        s1.Equals(v);
                    }
                }
            }


            using (new OperationTimer("compare strings internation"))
            {
                s1 = string.Intern(s1);
                for (var index = 0; index < ms.Length; index++)
                {
                    ms[index] = string.Intern(ms[index]);
                }

                for (var i = 0; i < iterations; i++)
                {
                    foreach (var v in ms)
                    {
                        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                        ReferenceEquals(s1, v);
                    }
                }
            }

            using (new OperationTimer("compare strings internation interned"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    foreach (var v in ms)
                    {
                        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                        ReferenceEquals(s1, v);
                    }
                }
            }
        }
    }
    public sealed class OthersGenerics : ITest
    {
        public void Test()
        {
            var iterations = 100000;

            using (new OperationTimer("generic List value type"))
            {
                var d = new List<int>(iterations);
                for (var i = 0; i < iterations; i++)
                {
                    d.Add(i);
                }
            }
            using (new OperationTimer("generic List ref type"))
            {
                var d = new List<string>(iterations);
                for (var i = 0; i < iterations; i++)
                {
                    d.Add(i.ToString());
                }
            }

            using (new OperationTimer("not generic List value type"))
            {
                var al = new ArrayList();
                for (var i = 0; i < iterations; i++)
                {
                    al.Add(i);
                }
            }
            using (new OperationTimer("not generic List ref type"))
            {
                var al = new ArrayList();
                for (var i = 0; i < iterations; i++)
                {
                    al.Add(i.ToString());
                }
            }
        }
    }
}
