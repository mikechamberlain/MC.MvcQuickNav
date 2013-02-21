using System;
using System.Collections.Generic;
using System.Linq;

namespace MC.MvcQuickNav
{
    public class NavigationManager
    {
        private readonly Uri _currentUrl;
        private readonly INavigationTreeProvider _navigationTreeProvider;

        public NavigationManager(INavigationTreeProvider navigationTreeProvider, Uri currentUrl)
        {
            _currentUrl = currentUrl;
            _navigationTreeProvider = navigationTreeProvider;
        }

        /// <summary>
        /// Gets the entire navigation tree.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<NavigationNode> GetNodes(int activeDepth = Int32.MaxValue)
        {
            var nodes = _navigationTreeProvider.GetNavigationNodes().ToList();
            SetNodeActivity(nodes, activeDepth);
            return nodes;
        }

        /// <summary>
        /// Returns the root of the user's current navigation node.
        /// </summary>
        public ITreeNode<NavigationItem> GetActiveBranch(int activeDepth = Int32.MaxValue)
        {
            var nodes = GetNodes(1);
            var active = nodes.SingleOrDefault(n => n.Value.IsActive);
            if (active == null) 
                return null;
            SetNodeActivity(active, activeDepth);
            return active;
        }

        /// <summary>
        /// Returns the user's current active node
        /// </summary>
        public ITreeNode<NavigationItem> GetActiveNode(int activeDepth = Int32.MaxValue)
        {
            return GetNodes(activeDepth).WalkMany(activeDepth).SingleOrDefault(n => n.Value.IsActive);
        }

        /// <summary>
        /// Sets the node whose URL is the most specific match to the current URL to be the active, and all others inactive.
        /// </summary>
        private void SetNodeActivity(IEnumerable<ITreeNode<NavigationItem>> nodes, int maxDepth = Int32.MaxValue)
        {
            var nodeList = nodes.WalkMany(maxDepth).ToList();

            // get the nodes in descending order of common url prefix length,
            // ignoring nodes with no common prefix
            var activeNode = (from node in nodeList
                              where node.Value.Url != null
                              let length = GetCommonPrefixLength(node.Value.Url, _currentUrl.PathAndQuery)
                              where length > 0
                              orderby length descending
                              select node).FirstOrDefault();

            foreach (var node in nodeList)
            {
                node.Value.IsActive = (node == activeNode);
            }
        }

        private void SetNodeActivity(ITreeNode<NavigationItem> node, int maxDepth = Int32.MaxValue)
        {
            SetNodeActivity(new[] {node}, maxDepth);
        }

        private int GetCommonPrefixLength(string s1, string s2)
        {
            var i = 0;
            var maxLength = Math.Min(s1.Length, s2.Length);
            while (i < maxLength && AreEqual(s1[i], s2[i]))
            {
                i++;
            }
            return i;
        }

        private bool AreEqual(char c1, char c2)
        {
            return String.Compare(c1.ToString(), c2.ToString(), StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}
