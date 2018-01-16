using System;
using System.Text;
using SolaceSystems.Solclient.Messaging;

namespace SolaceCaller
{
    public class TopicPublisher : SolaceBase
    {
        public TopicPublisher(string vpnName, string userName, string password) :
            base(vpnName, userName, password)
        { }

        public void Run(IContext context, string host)
        {
            // validate parameters
            HandleValidations(context, host);

            // Create session properties
            SessionProperties sessionProps = GetSessionProperties(host);

            // Connect to the Solace messaging router
            // TODO :: Adding loggin message as  - string.Format("Connecting as {0}@{1} on {2}...", _userName, _vpnName, host);
            using (ISession session = context.CreateSession(sessionProps, null, null))
            {
                ReturnCode returnCode = session.Connect();
                if (returnCode == ReturnCode.SOLCLIENT_OK)
                {
                    // TODO :: Adding loggin message as  - string.Format("Session successfully connected.");
                    PublishMessage(session);
                }
                else
                {
                    // TODO :: Adding loggin message as  - string.Format("Error connecting, return code: {0}", returnCode);
                }
            }
        }

        private void PublishMessage(ISession session)
        {
            // Create the message
            using (IMessage message = ContextFactory.Instance.CreateMessage())
            {
                message.Destination = ContextFactory.Instance.CreateTopic("tutorial/topic");
                // Create the message content as a binary attachment
                message.BinaryAttachment = Encoding.ASCII.GetBytes("Sample Message");

                // Publish the message to the topic on the Solace messaging router
                // TODO :: Adding loggin message as  - string.Format("Publishing message...");
                ReturnCode returnCode = session.Send(message);
                if (returnCode == ReturnCode.SOLCLIENT_OK)
                {
                    // TODO :: Adding loggin message as  - string.Format("Done.");
                }
                else
                {
                    // TODO :: Adding loggin message as  - string.Format("Publishing failed, return code: {0}", returnCode);
                }
            }
        }
    }
}
