﻿using System;
using System.Linq;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;
using Xwt;

namespace MonoDevelop.ConnectedServices.Gui.ServicesTab
{
	/// <summary>
	/// ViewContent host for the services gallery and service details widgets
	/// </summary>
	class ConnectedServicesViewContent : AbstractXwtViewContent
	{
		ConnectedServicesWidget widget;
		ScrollView scrollContainer;

		public ConnectedServicesViewContent (DotNetProject project)
		{
			this.Project = project;
			this.ContentName = string.Format ("{0} - {1}", GettextCatalog.GetString (ConnectedServices.SolutionTreeNodeName), project.Name);

			widget = new ConnectedServicesWidget ();
			widget.GalleryShown += (sender, e) => {
				this.ContentName = string.Format ("{0} - {1}", GettextCatalog.GetString (ConnectedServices.SolutionTreeNodeName), project.Name);
			};
			widget.ServiceShown += (sender, e) => {
				this.ContentName = string.Format ("{0} - {1}", e.Service.DisplayName, project.Name);
			};
			scrollContainer = new ScrollView (widget);
		}

		public override Widget Widget {
			get {
				return scrollContainer;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this ViewContent represents a file or not.
		/// </summary>
		public override bool IsFile {
			get {
				return false;
			}
		}

		/// <summary>
		/// Updates the content of the view for the project. If a service id is given, opens the details view for that service
		/// </summary>
		public void UpdateContent(string serviceId)
		{
			var binding = ((DotNetProject)this.Project).GetConnectedServicesBinding ();
			if (string.IsNullOrEmpty (serviceId)) {
				var services = binding.SupportedServices;
				this.widget.ShowGallery (services, Project);
			} else {
				var service = binding.SupportedServices.FirstOrDefault (x => x.Id == serviceId);
				this.widget.ShowServiceDetails (service);
			}
		}

		public override object GetDocumentObject ()
		{
			var node = ((DotNetProject)this.Project).GetConnectedServicesBinding ()?.ServicesNode;
			if (node != null) {
				if (widget.ShowingService != null)
					return node.GetServiceNode (widget.ShowingService);
			}
			return node;
		}
	}
}
