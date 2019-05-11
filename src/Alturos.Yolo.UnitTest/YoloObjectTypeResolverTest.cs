using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alturos.Yolo.UnitTest
{
    [TestClass]
    public class YoloObjectTypeResolverTest
    {
        [TestMethod]
        public void ResolveObjectId()
        {
            var names = new string[] { "Car", "Cat", "Bike" };

            var resolver = new YoloObjectTypeResolver(names);

            var name = resolver.Resolve(1);
            Assert.AreEqual(names[1], name);

            name = resolver.Resolve(2);
            Assert.AreEqual(names[2], name);

            name = resolver.Resolve(0);
            Assert.AreEqual(names[0], name);

            name = resolver.Resolve(4);
            Assert.AreEqual("unknown key", name);
        }
    }
}
