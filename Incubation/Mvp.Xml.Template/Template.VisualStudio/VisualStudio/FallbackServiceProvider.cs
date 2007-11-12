using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Xml.Template.VisualStudio
{
	public class FallbackServiceProvider : IServiceProvider
	{
		IServiceProvider childProvider;
		IServiceProvider parentProvider;

		public FallbackServiceProvider(IServiceProvider childProvider, IServiceProvider parentProvider)
		{
			this.childProvider = childProvider;
			this.parentProvider = parentProvider;
		}
		
		public object GetService(Type serviceType)
		{
			object instance = childProvider.GetService(serviceType);
			if (instance == null)
			{
				instance = parentProvider.GetService(serviceType);
			}

			return instance;
		}
	}
}
