using System.Web.Mvc;
using SolaceSystems.Solclient.Messaging;
using SolaceCaller;
using System;

namespace SolaceSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _vpnName;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _host;

        public HomeController()
        {
            _vpnName = "";
            _userName = "";
            _password = "";
            _host = "";
        }

        // GET: Publish
        public ActionResult Publish()
        {

            var cfp = new ContextFactoryProperties()
            {
                SolClientLogLevel = SolLogLevel.Warning
            };

            cfp.LogToConsoleError();

            ContextFactory.Instance.Init(cfp);

            try
            {
                // Context must be created first
                using (IContext context = ContextFactory.Instance.CreateContext(new ContextProperties(), null))
                {
                    // Create the application
                    TopicPublisher topicPublisher = new TopicPublisher(_vpnName, _userName, _password);

                    // Run the application within the context and against the host
                    topicPublisher.Run(context, _host);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown: {0}", ex.Message);
            }
            finally
            {
                // Dispose Solace Systems Messaging API
                ContextFactory.Instance.Cleanup();
            }

            return View();
        }

        // GET: Publish
        public ActionResult Subscribes()
        {

            var cfp = new ContextFactoryProperties()
            {
                SolClientLogLevel = SolLogLevel.Warning
            };

            cfp.LogToConsoleError();

            ContextFactory.Instance.Init(cfp);

            try
            {
                // Context must be created first
                using (IContext context = ContextFactory.Instance.CreateContext(new ContextProperties(), null))
                {
                    // Create the application
                    TopicSubscriber topicSubscriber = new TopicSubscriber(_vpnName, _userName, _password);

                    // Run the application within the context and against the host
                    topicSubscriber.Run(context, _host);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown: {0}", ex.Message);
            }
            finally
            {
                // Dispose Solace Systems Messaging API
                ContextFactory.Instance.Cleanup();
            }

            return View();
        }
    }
}