using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Xml.Linq;

namespace MC.MvcQuickNav
{
    internal class XmlSiteMapProvider : ISiteMapProvider
    {
        private readonly UrlHelper _urlHelper;
        private readonly string _xmlText;

        public XmlSiteMapProvider(UrlHelper urlHelper, string siteMapPath)
        {
            _urlHelper = urlHelper;
            // read this from disk once and store it for later
            _xmlText = File.ReadAllText(siteMapPath);
        }

        public NavigationNode GetSiteMap()
        {
            var xdoc = XDocument.Parse(_xmlText);
            var nodes = FromSiteMapElement(xdoc.Root);
            return new NavigationNode(null, nodes);
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
