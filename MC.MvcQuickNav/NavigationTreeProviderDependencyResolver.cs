using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MC.MvcQuickNav
{
    public class NavigationTreeProviderDependencyResolver : IDependencyResolver
    {
        private readonly Func<INavigationTreeProvider> _createProvider;

        public NavigationTreeProviderDependencyResolver(Func<INavigationTreeProvider> createProvider)
        {
            if (createProvider == null)
                throw new ArgumentNullException("createProvider");
            _createProvider = createProvider;
        }

        public object GetService(Type serviceType)
        {
            return serviceType == typeof(INavigationTreeProvider)
                ? _createProvider()
                : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }
    }
}
