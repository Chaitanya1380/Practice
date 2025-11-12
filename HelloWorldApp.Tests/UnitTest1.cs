using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelloWorldApp;

namespace HelloWorldApp.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Add_TwoNumbers_ReturnsSum()
        {
            // Arrange
            int a = 5;
            int b = 3;

            // Act
            int result = Program.Add(a, b);

            // Assert
            Assert.AreEqual(8, result);
        }
    }
}