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

using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Energistics.Common;
using Energistics.Datatypes;
using Energistics.Protocol.Core;
using PDS.WITSMLstudio.Desktop.Core.Commands;
using PDS.WITSMLstudio.Desktop.Core.Connections;
using PDS.WITSMLstudio.Desktop.Core.Runtime;
using PDS.WITSMLstudio.Desktop.Core.ViewModels;
using PDS.WITSMLstudio.Framework;

namespace PDS.WITSMLstudio.Desktop.Plugins.EtpBrowser.ViewModels
{
    /// <summary>
    /// Manages the behavior of the tree view user interface elements.
    /// </summary>
    /// <seealso cref="Caliburn.Micro.Screen" />
    public sealed class HierarchyViewModel : Screen, ISessionAware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyViewModel"/> class.
        /// </summary>
        public HierarchyViewModel(IRuntimeService runtime)
        {
            Runtime = runtime;
            DisplayName = string.Format("{0:D} - {0}", Protocols.Discovery);
            GetBaseUriCommand = new DelegateCommand(x => GetBaseUri(), x => CanExecute);
        }

        /// <summary>
        /// Gets or Sets the Parent <see cref="T:Caliburn.Micro.IConductor" />
        /// </summary>
        public new MainViewModel Parent
        {
            get { return (MainViewModel)base.Parent; }
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>The model.</value>
        public Models.EtpSettings Model
        {
            get { return Parent.Model; }
        }

        /// <summary>
        /// Gets the runtime service.
        /// </summary>
        /// <value>The runtime service.</value>
        public IRuntimeService Runtime { get; private set; }

        /// <summary>
        /// Gets the GetBaseUri command.
        /// </summary>
        public ICommand GetBaseUriCommand { get; }

        private bool _canExecute;

        /// <summary>
        /// Gets or sets a value indicating whether the Discovery protocol messages can be executed.
        /// </summary>
        /// <value><c>true</c> if Discovery protocol messages can be executed; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool CanExecute
        {
            get { return _canExecute; }
            set
            {
                if (_canExecute != value)
                {
                    _canExecute = value;
                    NotifyOfPropertyChange(() => CanExecute);
                }
            }
        }

        /// <summary>
        /// Gets resources using the current Base URI
        /// </summary>
        public void GetBaseUri()
        {
            //Parent.OnConnectionChanged(true, false);
            Parent.Resources.Clear();
            Parent.GetResources(Model?.BaseUri);
        }

        /// <summary>
        /// Determines whether the GetObject message can be sent for the selected resource.
        /// </summary>
        /// <returns><c>true</c> if the selected resource's level is greater than 1; otherwise, <c>false</c>.</returns>
        public bool CanGetObject
        {
            get
            {
                var resource = Parent.SelectedResource;
                return CanExecute && !string.IsNullOrWhiteSpace(resource?.Resource?.Uri);
            }
        }

        /// <summary>
        /// Gets the selected resource's details using the Store protocol.
        /// </summary>
        public void GetObject()
        {
            Parent.GetObject();
        }

        /// <summary>
        /// Determines whether the DeleteObject message can be sent for the selected resource.
        /// </summary>
        /// <returns><c>true</c> if the selected resource's level is greater than 1; otherwise, <c>false</c>.</returns>
        public bool CanDeleteObject
        {
            get { return CanGetObject; }
        }

        /// <summary>
        /// Deletes the selected resource using the Store protocol.
        /// </summary>
        public void DeleteObject()
        {
            if (Runtime.ShowConfirm("Are you sure you want to delete the selected resource?", MessageBoxButton.YesNo))
            {
                Parent.DeleteObject();
            }
        }

        /// <summary>
        /// Refreshes the hierarchy.
        /// </summary>
        public void RefreshHierarchy()
        {
            //Parent.OnConnectionChanged(true, false);
            GetBaseUri();
        }

        /// <summary>
        /// Gets a value indicating whether this selected node can be refreshed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can be refreshed; otherwise, <c>false</c>.
        /// </value>
        public bool CanRefreshSelected
        {
            get
            {
                var resource = Parent.SelectedResource;

                if (CanExecute && !string.IsNullOrWhiteSpace(resource?.Resource?.Uri))
                {
                    return resource.Resource.HasChildren != 0 ||
                           ResourceTypes.Folder.ToString().EqualsIgnoreCase(resource.Resource.ResourceType);
                }

                return false;
            }
        }

        /// <summary>
        /// Refreshes the selected node.
        /// </summary>
        public void RefreshSelected()
        {
            var resource = Parent.Resources.FindSelected();
            // Return if there is nothing currently selected
            if (resource == null) return;

            resource.ClearAndLoadChildren();
            // Expand the node if it wasn't previously
            resource.IsExpanded = true;
        }

        /// <summary>
        /// Refreshes the context menu.
        /// </summary>
        public void RefreshContextMenu()
        {
            NotifyOfPropertyChange(() => CanGetObject);
            NotifyOfPropertyChange(() => CanDeleteObject);
            NotifyOfPropertyChange(() => CanCopyUriToStreaming);
            NotifyOfPropertyChange(() => CanRefreshSelected);
            NotifyOfPropertyChange(() => CanCopyUriToClipboard);
            NotifyOfPropertyChange(() => CanCopyUriToStore);
        }

        /// <summary>
        /// Gets a value indicating whether this instance can copy URI to clipboard.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can copy URI to clipboard; otherwise, <c>false</c>.
        /// </value>
        public bool CanCopyUriToClipboard => CanGetObject;

        /// <summary>
        /// Copies the URI to clipboard.
        /// </summary>
        public void CopyUriToClipboard()
        {
            var resource = Parent.SelectedResource;

            if (resource?.Resource == null)
                return;

            Clipboard.SetText(resource.Resource.Uri);
        }

        /// <summary>
        /// Gets a value indicating whether this instance can copy URI to store.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can copy URI to store; otherwise, <c>false</c>.
        /// </value>
        public bool CanCopyUriToStore => CanGetObject;

        /// <summary>
        /// Copies the URI to store.
        /// </summary>
        public void CopyUriToStore()
        {
            var resource = Parent.SelectedResource;
            var storeViewModel = Parent.Items.OfType<StoreViewModel>().FirstOrDefault();

            if (resource?.Resource == null || storeViewModel == null)
                return;

            storeViewModel.ClearInputSettings();
            Parent.ActivateItem(storeViewModel);
            storeViewModel.Model.Store.Uri = resource.Resource.Uri;
            NotifyOfPropertyChange(() => storeViewModel.Model.Store.Uri);
        }

        /// <summary>
        /// Determines whether the ChannelDescribe message can be sent for the selected resource.
        /// </summary>
        /// <value><c>true</c> if the channels can be described; otherwise, <c>false</c>.</value>
        public bool CanCopyUriToStreaming
        {
            get
            {
                var resource = Parent.SelectedResource;

                if (CanExecute && !string.IsNullOrWhiteSpace(resource?.Resource?.Uri))
                {
                    return resource.Resource.ChannelSubscribable;
                }

                return false;
            }
        }

        /// <summary>
        /// Copies the URI to streaming.
        /// </summary>
        public void CopyUriToStreaming()
        {
            var viewModel = Parent.Items.OfType<StreamingViewModel>().FirstOrDefault();
            var resource = Parent.SelectedResource;

            if (viewModel != null && resource != null)
            {
                Model.Streaming.Uri = resource.Resource.Uri;
                viewModel.AddUri();
                Parent.ActivateItem(viewModel);
            }
        }

        /// <summary>
        /// Determines whether the NotificationRequest message can be sent for the selected resource.
        /// </summary>
        /// <value><c>true</c> if the object is notifiable; otherwise, <c>false</c>.</value>
        public bool CanCopyUriToNotification
        {
            get
            {
                var resource = Parent.SelectedResource;

                if (CanExecute && !string.IsNullOrWhiteSpace(resource?.Resource?.Uri))
                {
                    return resource.Resource.ObjectNotifiable;
                }

                return false;
            }
        }

        /// <summary>
        /// Copies the URI to the Notification tab.
        /// </summary>
        public void CopyUriToNotification()
        {
            var viewModel = Parent.Items.OfType<StoreNotificationViewModel>().FirstOrDefault();
            var resource = Parent.SelectedResource;

            if (viewModel != null && resource != null)
            {
                Model.StoreNotification.Uri = resource.Resource.Uri;
                Parent.ActivateItem(viewModel);
            }
        }

        /// <summary>
        /// Called when the selected connection has changed.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public void OnConnectionChanged(Connection connection)
        {
        }

        /// <summary>
        /// Called when the <see cref="OpenSession" /> message is recieved.
        /// </summary>
        /// <param name="e">The <see cref="ProtocolEventArgs{OpenSession}" /> instance containing the event data.</param>
        public void OnSessionOpened(ProtocolEventArgs<OpenSession> e)
        {
            if (e.Message.SupportedProtocols.All(x => x.Protocol != (int)Protocols.Discovery))
                return;
            
            CanExecute = true;
            RefreshContextMenu();
        }

        /// <summary>
        /// Called when the <see cref="Energistics.EtpClient" /> web socket is closed.
        /// </summary>
        public void OnSocketClosed()
        {
            CanExecute = false;
            RefreshContextMenu();
        }
    }
}
