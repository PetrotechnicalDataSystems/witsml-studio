﻿//----------------------------------------------------------------------- 
// PDS WITSMLstudio Desktop, 2018.1
//
// Copyright 2018 PDS Americas LLC
// 
// Licensed under the PDS Open Source WITSML Product License Agreement (the
// "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.pds.group/WITSMLstudio/OpenSource/ProductLicenseAgreement
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System.Net;
using System.Runtime.Serialization;
using System.Security;
using Caliburn.Micro;
using Energistics.Common;

namespace PDS.WITSMLstudio.Desktop.Core.Connections
{
    /// <summary>
    /// Connection details for a connection
    /// </summary>
    [DataContract]
    public class Connection : PropertyChangedBase
    {
        /// <summary>
        /// Initializes the <see cref="Connection"/> class.
        /// </summary>
        static Connection()
        {
            AutoMapper.Mapper.Initialize(x => x.CreateMap<Connection, Connection>());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        public Connection()
        {
            SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            IsAuthenticationBasic = true;
            ProxyPort = 80;
            RedirectPort = 9005;
            SubProtocol = EtpSettings.EtpSubProtocolName;
            EtpEncoding = "binary";
        }

        private AuthenticationTypes _authenticationType;

        /// <summary>
        /// Gets or sets the authentication type.
        /// </summary>
        /// <value>The authentication type.</value>
        [DataMember]
        public AuthenticationTypes AuthenticationType
        {
            get { return _authenticationType; }
            set
            {
                if (_authenticationType != value)
                {
                    _authenticationType = value;
                    NotifyOfPropertyChange(() => AuthenticationType);
                }
            }
        }

        private SecurityProtocolType _securityProtocol;

        /// <summary>
        /// Gets or sets the security protocol.
        /// </summary>
        /// <value>The security protocol.</value>
        [DataMember]
        public SecurityProtocolType SecurityProtocol
        {
            get { return _securityProtocol; }
            set
            {
                if (_securityProtocol != value)
                {
                    _securityProtocol = value;
                    NotifyOfPropertyChange(() => SecurityProtocol);
                }
            }
        }

        private string _name;

        /// <summary>
        /// Gets or sets the name of the connection
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.Equals(_name, value))
                {
                    _name = value;
                    NotifyOfPropertyChange(() => Name);
                }
            }
        }

        private string _uri;

        /// <summary>
        /// Gets or sets the uri to access the connection
        /// </summary>
        [DataMember]
        public string Uri
        {
            get { return _uri; }
            set
            {
                if (!string.Equals(_uri, value))
                {
                    _uri = value;
                    NotifyOfPropertyChange(() => Uri);
                }
            }
        }

        private string _clientId;

        /// <summary>
        /// Gets or sets the client ID
        /// </summary>
        [DataMember]
        public string ClientId
        {
            get { return _clientId; }
            set
            {
                if (!string.Equals(_clientId, value))
                {
                    _clientId = value;
                    NotifyOfPropertyChange(() => ClientId);
                }
            }
        }

        private int _redirectPort;

        /// <summary>
        /// Gets or sets the redirect port.
        /// </summary>
        /// <value>The redirect port.</value>
        [DataMember]
        public int RedirectPort
        {
            get { return _redirectPort; }
            set
            {
                if (_redirectPort != value)
                {
                    _redirectPort = value;
                    NotifyOfPropertyChange(() => RedirectPort);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the proxy host name is not a URI.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the proxy host name is not a URI; otherwise, <c>false</c>.
        /// </value>
        public bool IsProxyHostName => !ProxyHost?.Contains("://") ?? false;

        private string _proxyHost;

        /// <summary>
        /// Gets or sets the proxy host name.
        /// </summary>
        [DataMember]
        public string ProxyHost
        {
            get { return _proxyHost; }
            set
            {
                if (!string.Equals(_proxyHost, value))
                {
                    _proxyHost = value;
                    NotifyOfPropertyChange(() => ProxyHost);
                    NotifyOfPropertyChange(() => IsProxyHostName);
                }
            }
        }

        private int _proxyPort;

        /// <summary>
        /// Gets or sets the proxy port.
        /// </summary>
        /// <value>The proxy port.</value>
        [DataMember]
        public int ProxyPort
        {
            get { return _proxyPort; }
            set
            {
                if (_proxyPort != value)
                {
                    _proxyPort = value;
                    NotifyOfPropertyChange(() => ProxyPort);
                }
            }
        }

        private string _proxyUsername;

        /// <summary>
        /// Gets or sets the username to authenticate with the proxy server.
        /// </summary>
        [DataMember]
        public string ProxyUsername
        {
            get { return _proxyUsername; }
            set
            {
                if (!string.Equals(_proxyUsername, value))
                {
                    _proxyUsername = value;
                    NotifyOfPropertyChange(() => ProxyUsername);
                }
            }
        }

        /// <summary>
        /// Gets or sets the proxy password.
        /// </summary>
        [DataMember]
        public string ProxyPassword { get; set; }

        /// <summary>
        /// Gets or sets the SecureString password to authenticate with the proxy server.
        /// </summary>
        public SecureString SecureProxyPassword { get; set; }

        private bool _proxyUseDefaultCredentials;

        /// <summary>
        /// Gets or sets a value indicating whether to use default credentials for the web proxy.
        /// </summary>
        /// <value>
        /// <c>true</c> if use default credentials; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool ProxyUseDefaultCredentials
        {
            get { return _proxyUseDefaultCredentials; }
            set
            {
                if (_proxyUseDefaultCredentials != value)
                {
                    _proxyUseDefaultCredentials = value;
                    NotifyOfPropertyChange(() => ProxyUseDefaultCredentials);
                }
            }
        }

        private string _username;

        /// <summary>
        /// Gets or sets the username to authenticate the connection
        /// </summary>
        [DataMember]
        public string Username
        {
            get { return _username; }
            set
            {
                if (!string.Equals(_username, value))
                {
                    _username = value;
                    NotifyOfPropertyChange(() => Username);
                }
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the SecureString password to authenticate the connection.
        /// </summary>
        public SecureString SecurePassword { get; set; }

        private string _jsonWebToken;

        /// <summary>
        /// Gets or sets the JSON Web Token to authenticate the connection.
        /// </summary>
        [DataMember]
        public string JsonWebToken
        {
            get { return _jsonWebToken; }
            set
            {
                if (!string.Equals(_jsonWebToken, value))
                {
                    _jsonWebToken = value;
                    NotifyOfPropertyChange(() => JsonWebToken);
                }
            }
        }

        private string _subProtocol;

        /// <summary>
        /// Gets or sets the sub protocol.
        /// </summary>
        [DataMember]
        public string SubProtocol
        {
            get { return _subProtocol; }
            set
            {
                if (!string.Equals(_subProtocol, value))
                {
                    _subProtocol = value;
                    NotifyOfPropertyChange(() => SubProtocol);
                }
            }
        }

        private string _etpEncoding;

        /// <summary>
        /// Gets or sets the ETP encoding.
        /// </summary>
        [DataMember]
        public string EtpEncoding
        {
            get { return _etpEncoding; }
            set
            {
                if (!string.Equals(_etpEncoding, value))
                {
                    _etpEncoding = value;
                    NotifyOfPropertyChange(() => EtpEncoding);
                }
            }
        }

        private bool _acceptInvalidCertificates;

        /// <summary>
        /// Gets or sets a value indicating whether to accept invalid certificates.
        /// </summary>
        /// <value>
        /// <c>true</c> if accept invalid certificates; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool AcceptInvalidCertificates
        {
            get { return _acceptInvalidCertificates; }
            set
            {
                if (_acceptInvalidCertificates != value)
                {
                    _acceptInvalidCertificates = value;
                    NotifyOfPropertyChange(() => AcceptInvalidCertificates);
                }
            }
        }

        private bool _preAuthenticate;

        /// <summary>
        /// Gets or sets a value indicating whether to pre-authenticate.
        /// </summary>
        /// <value>
        /// <c>true</c> if pre-authenticate; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool PreAuthenticate
        {
            get { return _preAuthenticate; }
            set
            {
                if (_preAuthenticate != value)
                {
                    _preAuthenticate = value;
                    NotifyOfPropertyChange(() => PreAuthenticate);
                }
            }
        }

        private bool _isAuthenticationBasic;

        /// <summary>
        /// Gets or sets a value indicating whether to connect using Basic authentication.
        /// </summary>
        /// <value>
        /// <c>true</c> if using Basic authentication; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsAuthenticationBasic
        {
            get { return _isAuthenticationBasic; }
            set
            {
                if (_isAuthenticationBasic != value)
                {
                    _isAuthenticationBasic = value;
                    IsAuthenticationBearer = !value;
                    NotifyOfPropertyChange(() => IsAuthenticationBasic);
                }
            }
        }

        private bool _isAuthenticationBearer;

        /// <summary>
        /// Gets or sets a value indicating whether to connect using Bearer authentication.
        /// </summary>
        /// <value>
        /// <c>true</c> if using Bearer authentication; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsAuthenticationBearer
        {
            get { return _isAuthenticationBearer; }
            set
            {
                if (_isAuthenticationBearer != value)
                {
                    _isAuthenticationBearer = value;
                    IsAuthenticationBasic = !value;
                    NotifyOfPropertyChange(() => IsAuthenticationBearer);
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"Uri: {Uri}; Username: {Username}; AuthenticationType: {AuthenticationType}; SecurityProtocol: {SecurityProtocol};" +
                   $" ProxyHost: {ProxyHost}; ProxyPort: {ProxyPort}; ProxyUsername: {ProxyUsername}; ProxyUseDefaultCredentials: {ProxyUseDefaultCredentials};" +
                   (!string.IsNullOrWhiteSpace(SubProtocol) ? $" SubProtocol: {SubProtocol};" : string.Empty) +
                   (!string.IsNullOrWhiteSpace(EtpEncoding) ? $" EtpEncoding: {EtpEncoding};" : string.Empty);
        }
    }
}
