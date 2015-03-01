namespace DonutRing.UnitTest
{
    using DonutRing.Shared;
    using DonutRing.Shared.Manager;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class TestColorManager
    {
        [Test]
        public async Task TestGetRandomColor()
        {
            var colorTuple = await ColorManager.GetColorTupleAsync();
            Assert.IsNotNull(colorTuple);
            Assert.AreNotEqual(Settings.DEFAULT_COLOR_NAME, colorTuple.Item1);
            Assert.AreNotEqual(Settings.DEFAULT_COLOR_HEX, colorTuple.Item2);
        }
    }
}
