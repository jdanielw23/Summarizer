using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Summarizer.Model.Daniels_Implementation;
using Summarizer.Model.Utils;
using System.Collections.Generic;
using System.Linq;

namespace SummarizerTest.ModelTest
{
    [TestClass]
    public class DiagrammerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Diagrammer diagrammer = new Diagrammer(@"..\..\..\Summarizer\Documents\The Bible txt - Original\Bible_KJV.txt");
        }
    }
}
