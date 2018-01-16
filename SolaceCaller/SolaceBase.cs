using System;
using SolaceSystems.Solclient.Messaging;

namespace SolaceCaller
{
    public class SolaceBase
    {
        protected readonly string _vpnName;
        protected readonly string _userName;
        protected readonly string _password;
        protected const int _defaultReconnectRetries = 3;

        public SolaceBase(string vpnName, string userName, string password)
        {
            _vpnName = vpnName;
            _password = password;
            _userName = userName;
        }

        protected void HandleValidations(IContext context, string host)
        {
            if (context == null)
            {
                throw new ArgumentException("Solace Systems API context Router must be not null.", "context");
            }
            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentException("Solace Messaging Router host name must be non-empty.", "host");
            }
            if (string.IsNullOrWhiteSpace(_vpnName))
            {
                throw new InvalidOperationException("VPN name must be non-empty.");
            }
            if (string.IsNullOrWhiteSpace(_userName))
            {
                throw new InvalidOperationException("Client username must be non-empty.");
            }
        }

        protected SessionProperties GetSessionProperties(string host)
        {
            return new SessionProperties()
            {
                Host = host,
                VPNName = _vpnName,
                UserName = _userName,
                Password = _password,
                ReconnectRetries = _defaultReconnectRetries
            };
        }
    }
}
