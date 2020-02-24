using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChatPlatformUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CheckServerStartup()
        {
            Assert.IsTrue(ChatPlatform.ServerHandler.Start(13000), "Server Unable to Start");
            ChatPlatform.ServerHandler.Stop();
        }

        [TestMethod]
        public void ConnectionTest()
        {
            List<ChatPlatform.MessageRecievedEventArgs> list = new List<ChatPlatform.MessageRecievedEventArgs>();

            ChatPlatform.ServerHandler.ChatEventHandler += delegate(object sender, EventArgs eventArgs)
            {
                ChatPlatform.MessageRecievedEventArgs m = eventArgs as ChatPlatform.MessageRecievedEventArgs;
                list.Add(m);
            };

            ChatPlatform.ServerHandler.Start(13000);
            ChatPlatform.ServerHandler.BeginAcceptConnections();
            Thread.Sleep(1000);

            ChatClient.ClientHandler c = new ChatClient.ClientHandler("127.0.0.1", 13000, "1");

            Thread.Sleep(1000);
            c.SendMessage("Test");
            Thread.Sleep(1000);

            Debug.WriteLine("Messages Recieved");
            foreach (ChatPlatform.MessageRecievedEventArgs m in list)
                Debug.WriteLine("{0}: {1} //{2}", m.sender, m.message, m.t);

            Assert.IsTrue(list[1].message.Equals("Test"));

            c.StopClient();
            ChatPlatform.ServerHandler.Stop();
        }

        [TestMethod]
        public void TestLogin()
        {
            ChatPlatform.ServerHandler.Start(13000);
            ChatPlatform.ServerHandler.BeginAcceptConnections();
            Thread.Sleep(1000);

            ChatClient.ClientHandler c = new ChatClient.ClientHandler("127.0.0.1", 13000, "TESTUSER");
            Thread.Sleep(1000);

            Assert.IsTrue(ChatPlatform.ServerHandler.ClientList[0].Username.Equals("TESTUSER"));

            c.StopClient();
            ChatPlatform.ServerHandler.Stop();
        }

        [TestMethod]
        public void TestTalkback()
        {
            List<ChatClient.MessageRecievedEventArgs> list = new List<ChatClient.MessageRecievedEventArgs>();

            ChatPlatform.ServerHandler.Start(13000);
            ChatPlatform.ServerHandler.BeginAcceptConnections();
            Thread.Sleep(1000);

            ChatClient.ClientHandler c1 = new ChatClient.ClientHandler("127.0.0.1", 13000, "TESTUSER1");
            ChatClient.ClientHandler c2 = new ChatClient.ClientHandler("127.0.0.1", 13000, "TESTUSER2");
            Thread.Sleep(1000);

            c2.ChatRecievedEventHandler += delegate (object sender, EventArgs eventArgs)
            {
                ChatClient.MessageRecievedEventArgs m = eventArgs as ChatClient.MessageRecievedEventArgs;
                list.Add(m);
            };

            c1.SendMessage("TESTMESSAGE");
            Thread.Sleep(1000);

            foreach(ChatClient.MessageRecievedEventArgs m in list)
                Debug.WriteLine("{0}", m.message);

            Assert.IsTrue("TESTUSER1: TESTMESSAGE".Equals(list[0].message));

            c1.StopClient();
            c2.StopClient();
            ChatPlatform.ServerHandler.Stop();
        }

        [TestMethod]
        public void TestClientNumber()
        {
            List<ChatClient.ClientHandler> list = new List<ChatClient.ClientHandler>();

            ChatPlatform.ServerHandler.Start(13000);
            ChatPlatform.ServerHandler.BeginAcceptConnections();
            Thread.Sleep(1000);

            try
            {
                while (list.Count < 1000)
                {
                    ChatClient.ClientHandler c1 = new ChatClient.ClientHandler("127.0.0.1", 13000, "TESTUSER1");
                    list.Add(c1);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }


        }
    }
}
