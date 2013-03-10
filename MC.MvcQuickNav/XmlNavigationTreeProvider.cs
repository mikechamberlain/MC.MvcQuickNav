using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;

namespace MC.MvcQuickNav
{
    /// <summary>
    /// Converts an XML sitemap to a navigation tree.
    /// </summary>
    public class XmlNavigationTreeProvider : INavigationTreeProvider
    {
        private readonly XDocument _sitemap;
        private readonly UrlHelper _urlHelper;

        public XmlNavigationTreeProvider(RequestContext requestContext, XDocument sitemap)
        {
            _sitemap = sitemap;
            _urlHelper = new UrlHelper(requestContext);
        }

        public IEnumerable<NavigationNode> GetNavigationNodes()
        {
            return FromSiteMapElement(_sitemap.Root);
        }

        private IEnumerable<NavigationNode> FromSiteMapElement(XElement xel)
        {
            return xel.Elements("siteMapNode").Select(FromNavigationNodeElement).ToList();
        }

        private NavigationNode FromNavigationNodeElement(XElement xel)
        {
            Func<string, string> safeAttribtueValue = name =>
            {
                var attribute = xel.Attribute(name);
                return (attribute == null ? "" : attribute.Value);
            };

            var url = safeAttribtueValue("url");
            bool isNewWindow;
            Boolean.TryParse(safeAttribtueValue("newWindow"), out isNewWindow);

            var item = new NavigationItem
            {
                Title = safeAttribtueValue("title"),
                Url = (url != "" ? _urlHelper.Content(url) : ""),
                Description = safeAttribtueValue("description"),
                OpenInNewWindow = isNewWindow
            };
            return new NavigationNode(item, FromSiteMapElement(xel));
        }
    }
}
