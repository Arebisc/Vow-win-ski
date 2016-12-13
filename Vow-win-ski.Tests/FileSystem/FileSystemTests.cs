using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Vow_win_ski.FileSystem;

namespace Vow_win_ski.Tests.FileSystem
{
    [TestFixture]
    public class FileSystemTests
    {
        [Test]
        [TestCase("fileName")]
        [TestCase("file Name")]
        public void Check_CreatingFileOnDisc(string fileName)
        {
            Disc.InitDisc();

            Disc.GetDisc.CreateFile(fileName, String.Empty);
            Assert.IsTrue(Disc.GetDisc.GetFileData(fileName) != null);
        }
    }
}
