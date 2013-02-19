using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using TreeNodes = System.Collections.Generic.IEnumerable<MC.MvcQuickNav.ITreeNode<MC.MvcQuickNav.NavigationItem>>;

namespace MC.MvcQuickNav
{
    public static class NavigationHelper
    {
        internal const string DefaultSiteMapUrl = "~/Web.sitemap";
        private const int DefaultMaxNavigationMenuDepth = 3;
        private const int DefaultMaxChildNavigationDepth = 3;
        private const string ActiveNodeCssClassName = "active";

        /// <summary>
        /// Renders the navigation tree for the entire site.
        /// </summary>
        public static MvcHtmlString FullNavigation(this HtmlHelper helper, int maxDepth = DefaultMaxNavigationMenuDepth)
        {
            var manager = helper.GetNavigationTreeManager();
            var nodes = manager.GetNodes(1); // main menu shows active node at the top level
            foreach(var node in nodes)
                node.Prune(maxDepth);
            return nodes.BuildHtml("menu");
        }

        /// <summary>
        /// Renders the child navigation tree for the user's current location in the site.
        /// </summary>
        public static MvcHtmlString ChildNavigation(this HtmlHelper helper, int maxDepth = DefaultMaxChildNavigationDepth)
        {
            var manager = helper.GetNavigationTreeManager();
            var activeBranch = manager.GetActiveBranch(maxDepth);
            if (activeBranch == null || !activeBranch.Children.Any())
                return MvcHtmlString.Empty;

            activeBranch.Walk().Single(n => n.Value.IsActive).Value.Url = ""; // don't want the active node to be a link
            activeBranch.Prune(maxDepth);
            return activeBranch.BuildHtml("children");
        }

        /// <summary>
        /// Renders the breadcrumb trail for the user's current position in the site.
        /// </summary>
        public static MvcHtmlString Breadcrumbs(this HtmlHelper helper)
        {
            var manager = GetNavigationTreeManager(helper);
            var activeNode = manager.GetActiveBranch();
            if (activeNode == null)
                return MvcHtmlString.Empty;

            var activePath = activeNode.FindPath(n => n.Value.IsActive).ToList();
            activePath.Last().Value.Url = "";
            foreach(var node in activePath)
                node.Prune(1);
            return activePath.BuildHtml("breadcrumbs");
        }

        /// <summary>
        /// Renders the active node's title.
        /// </summary>
        public static MvcHtmlString NavigationTitle(this HtmlHelper helper)
        {
            var manager = helper.GetNavigationTreeManager();
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

        private static TagBuilder BuildTags(this TreeNodes nodes)
        {
            var ulTag = new TagBuilder("ul");

            foreach(var node in nodes)
            {
                var liTag = new TagBuilder("li");
                if (node.Value.IsActive)
                {
                    liTag.AddCssClass(ActiveNodeCssClassName);
                }

                var spanTag = new TagBuilder("span");
                spanTag.SetInnerText(node.Value.Title);

                if (String.IsNullOrWhiteSpace(node.Value.Url))
                {
                    liTag.InnerHtml += spanTag;
                }
                else
                {
                    var aTag = new TagBuilder("a");
                    aTag.MergeAttribute("href", node.Value.Url);
                    if (node.Value.OpenInNewWindow)
                    {
                        aTag.MergeAttribute("target", "_blank");
                    }
                    if (!String.IsNullOrWhiteSpace(node.Value.Title))
                    {
                        aTag.MergeAttribute("title", node.Value.Description);
                    }
                    aTag.InnerHtml += spanTag;
                    liTag.InnerHtml += aTag;
                }

                if (node.Children.Any())
                {
                    liTag.InnerHtml += node.Children.BuildTags();
                }

                ulTag.InnerHtml += liTag;
            }
            return ulTag;
        }

        public static NavigationTreeManager GetNavigationTreeManager(this HtmlHelper helper)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var siteMapProvider = new XmlSiteMapProvider(urlHelper, HostingEnvironment.MapPath(DefaultSiteMapUrl));
            return new NavigationTreeManager(siteMapProvider, helper.ViewContext.RequestContext.HttpContext.Request.Url);
        }
    }
}