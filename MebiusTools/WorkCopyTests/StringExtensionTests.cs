using NUnit.Framework;
using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkCopy;

namespace ExtensionMethods.Tests
{
    [TestFixture()]
    public class StringExtensionTests
    {
        [Test()]
        public void ToUnixPathTest()
        {
            var str = @"s:\np\2016.04\homec\vve\";
            var result = @"./2016.04/homec/vve/";

            Assert.IsTrue(str.ToUnixPath() == result, $"\tstr=|{str}| \n\tresult=|{result}|\n\tstrToUnix=|{str.ToUnixPath()}|");
        }
    }
}