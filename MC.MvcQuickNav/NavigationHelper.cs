using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;
using TreeNodes = System.Collections.Generic.IEnumerable<MC.MvcQuickNav.ITreeNode<MC.MvcQuickNav.NavigationItem>>;

namespace MC.MvcQuickNav
{
    public static class NavigationHelper
    {
        private const int DefaultMaxNavigationMenuDepth = 3;
        private const int DefaultMaxChildNavigationDepth = 3;
        private const string ActiveNodeCssClassName = "active";
        private const string DefaultSiteMapUrl = "~/Web.sitemap";

        /// <summary>
        /// Renders the navigation menu for the site.
        /// </summary>
        public static MvcHtmlString NavigationMenu(this HtmlHelper helper, int maxDepth = DefaultMaxNavigationMenuDepth)
        {
            return FullNavigation(helper, maxDepth, "menu");
        }

        /// <summary>
        /// Renders the site map for the site.
        /// </summary>
        public static MvcHtmlString SiteMap(this HtmlHelper helper, int maxDepth = Int32.MaxValue)
        {
            return FullNavigation(helper, maxDepth, "sitemap");
        }

        private static MvcHtmlString FullNavigation(HtmlHelper helper, int maxDepth, string cssClass)
        {
            var manager = GetNavigationManager(helper.ViewContext.RequestContext);
            var nodes = manager.GetNodes(1).ToList(); // main menu shows active node at the top level
            nodes.PruneMany(maxDepth);
            return nodes.BuildHtml(cssClass);
        }

        /// <summary>
        /// Renders the navigation tree for the user's current section.
        /// </summary>
        public static MvcHtmlString InThisSection(this HtmlHelper helper, int maxDepth = DefaultMaxChildNavigationDepth)
        {
            var manager = GetNavigationManager(helper.ViewContext.RequestContext);
            var activeBranch = manager.GetActiveBranch(maxDepth);
            if (activeBranch == null || !activeBranch.Children.Any())
                return MvcHtmlString.Empty;

            activeBranch.Walk().Single(n => n.Value.IsActive).Value.Url = ""; // don't want the active node to be a link
            activeBranch.Prune(maxDepth);
            return activeBranch.BuildHtml("section");
        }

        /// <summary>
        /// Renders the breadcrumb trail for the user's current location in the site.
        /// </summary>
        public static MvcHtmlString Breadcrumbs(this HtmlHelper helper)
        {
            var manager = GetNavigationManager(helper.ViewContext.RequestContext);
            var activeNode = manager.GetActiveBranch();
            if (activeNode == null)
                return MvcHtmlString.Empty;

            var activePath = activeNode.FindPath(n => n.Value.IsActive).ToList();
            activePath.Last().Value.Url = "";
            activePath.PruneMany(1);
            return activePath.BuildHtml("breadcrumbs");
        }

        /// <summary>
        /// Renders the active node's title.
        /// </summary>
        public static MvcHtmlString NavigationTitle(this HtmlHelper helper)
        {
            var manager = GetNavigationManager(helper.ViewContext.RequestContext);
            var active = manager.GetActiveNode();
            return active == null 
                ? MvcHtmlString.Empty 
                : new MvcHtmlString(active.Value.Title);
        }

        /// <summary>
        /// Provides a mechanism to render a custom navtigation control.
        /// </summary>
        public static MvcHtmlString CustomNavigation(this HtmlHelper helper, TreeNodes nodes, string cssClassName)
        {
            return nodes.BuildHtml(cssClassName);
        }

        private static MvcHtmlString BuildHtml(this TreeNodes nodes, string cssClassName)
        {
            var treeNodes = nodes as List<ITreeNode<NavigationItem>> ?? nodes.ToList();
            if (nodes == null || !treeNodes.Any())
                return MvcHtmlString.Empty;

            var tags = treeNodes.BuildTags();
            var parent = new TagBuilder("nav");
            parent.AddCssClass(cssClassName);
            parent.InnerHtml += tags;
            return new MvcHtmlString(parent.ToString());
        }

        private static MvcHtmlString BuildHtml(this ITreeNode<NavigationItem> node, string cssClassName)
        {
            return BuildHtml(new[] {node}, cssClassName);
        }

        private static FluentTagBuilder BuildTags(this TreeNodes nodes)
        {
            var ulTag = new FluentTagBuilder("ul");

            foreach(var node in nodes)
            {
                var spanTag = new FluentTagBuilder("span")
                    .SetInnerText(node.Value.Title)
                    .If(!String.IsNullOrWhiteSpace(node.Value.Description)).MergeAttribute("title", node.Value.Description);

                var liTag = new FluentTagBuilder("li")
                    .If(node.Value.IsActive).AddCssClass(ActiveNodeCssClassName);
                
                if (String.IsNullOrWhiteSpace(node.Value.Url))
                {
                    liTag.AddInnerTag(spanTag);
                }
                else
                {
                    var aTag = new FluentTagBuilder("a")
                        .MergeAttribute("href", node.Value.Url)
                        .If(node.Value.OpenInNewWindow).MergeAttribute("target", "_blank")
                        .AddInnerTag(spanTag);
                    liTag.AddInnerTag(aTag);
                }

                liTag.If(node.Children.Any()).AddInnerTag(node.Children.BuildTags());
                ulTag.AddInnerTag(liTag);
            }
            return ulTag;
        }

        private static NavigationManager GetNavigationManager(RequestContext requestContext)
        {
            var provider = DependencyResolver.Current.GetService<INavigationTreeProvider>();
            if(provider == null)
            {
                provider = new XmlNavigationTreeProvider(requestContext, GetSiteMap());
            }
            return new NavigationManager(provider, requestContext.HttpContext.Request.Url);
        }

        private static XDocument ParseSiteMap()
        {
            return XDocument.Parse(File.ReadAllText(HostingEnvironment.MapPath(DefaultSiteMapUrl)));
        }

#if !DEBUG
        // In release mode, parse the site map to XML once only. 
        // The app must be restarted for changes to the site map file to take effect.
        private static readonly Lazy<XDocument> XmlSiteMap = new Lazy<XDocument>(ParseSiteMap, true);

        private static XDocument GetSiteMap()
        {
            return XmlSiteMap.Value;
        }
#endif

#if DEBUG
        // In debug mode, parse the site map on each call. Changes to the file are reflected immediately.
        private static XDocument GetSiteMap()
        {
            return ParseSiteMap();
        }
#endif
    }
}