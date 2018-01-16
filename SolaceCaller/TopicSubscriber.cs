using System;
using System.Threading;
using SolaceSystems.Solclient.Messaging;

namespace SolaceCaller
{
    public class TopicSubscriber : SolaceBase, IDisposable
    {
        public TopicSubscriber(string vpnName, string userName, string password) :
            base(vpnName, userName, password)
        { }

        private ISession Session = null;
        private EventWaitHandle WaitEventWaitHandle = new AutoResetEvent(false);

        public void Run(IContext context, string host)
        {
            // Validate parameters
            HandleValidations(context, host);

            // Create session properties
            SessionProperties sessionProps = GetSessionProperties(host);

            // Connect to the Solace messaging router
            // TODO :: Adding loggin message as  - string.Format("Connecting as {0}@{1} on {2}...", _userName, _vpnName, host);

            // NOTICE HandleMessage as the message event handler
            Session = context.CreateSession(sessionProps, HandleMessage, null);

            ReturnCode returnCode = Session.Connect();
            if (returnCode == ReturnCode.SOLCLIENT_OK)
            {
                // TODO :: Adding loggin message as  - string.Format("Session successfully connected.");

                // This is the topic on Solace messaging router where a message is published
                // Must subscribe to it to receive messages
                Session.Subscribe(ContextFactory.Instance.CreateTopic("tutorial/topic"), true);

                // TODO :: Adding loggin message as  - string.Format("Waiting for a message to be published...");
                WaitEventWaitHandle.WaitOne();
            }
            else
            {
                // TODO :: Adding loggin message as  - string.Format("Error connecting, return code: {0}", returnCode);
            }
        }

        /// <summary>
        /// This event handler is invoked by Solace Systems Messaging API when a message arrives
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        private void HandleMessage(object source, MessageEventArgs args)
        {
            // TODO :: Adding loggin message as  - string.Format("Received published message.");
            // Received a message
            using (IMessage message = args.Message)
            {
                // Expecting the message content as a binary attachment
                // TODO :: Adding loggin message as  - string.Format("Message content: {0}", Encoding.ASCII.GetString(message.BinaryAttachment));
                // finish the program
                WaitEventWaitHandle.Set();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Session != null)
                    {
                        Session.Dispose();
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
