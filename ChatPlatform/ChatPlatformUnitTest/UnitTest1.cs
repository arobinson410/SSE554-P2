using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChatPlatformUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private static event EventHandler TestEventHandler;
        [TestMethod]
        public void CheckServerStartup()
        {
            Assert.IsTrue(ChatPlatform.ServerHandler.Start(13000, TestEventHandler), "Server Unable to Start");
            ChatPlatform.ServerHandler.Stop();
        }

        [TestMethod]
        public void ConnectionTest()
        {
            TestEventHandler += getMessage;

            ChatPlatform.ServerHandler.Start(13000, TestEventHandler);
            ChatPlatform.ServerHandler.BeginAcceptConnections();

            ChatClient.Client.SetupClient("127.0.0.1", 13000);
            ChatClient.Client.SendMessage("Test");

            TestEventHandler -= getMessage;
        }

        private void getMessage(object sender, EventArgs e)
        {
            Assert.IsTrue(((string)sender).Equals("Test"));
        }
    }
}
