using System;
using System.Collections.Generic;
using System.Linq;

namespace MC.MvcQuickNav
{
    public static class TreeExtensions
    {
        /// <summary>
        /// Removes all nodes below a certain depth.
        /// </summary>
        public static void Prune<T>(this ITreeNode<T> node, int maxDepth)
        {
            if (maxDepth > 1)
            {
                foreach (var child in node.Children)
                    child.Prune(maxDepth - 1);
            }
            else
            {
                node.RemoveChildren();
            }
        }

        /// <summary>
        /// Walks through the tree, returning each node in turn.
        /// </summary>
        public static IEnumerable<ITreeNode<T>> Walk<T>(this ITreeNode<T> node, int maxDepth = Int32.MaxValue)
        {
            yield return node;
            if (maxDepth > 1)
            {
                foreach (var child in 
                    node.Children.Select(child => child.Walk(maxDepth - 1)).SelectMany(nodes => nodes))
                {
                    yield return child;
                }
            }
        }

        /// <summary>
        /// Walks through a series of trees, returning each node in turn.
        /// </summary>
        public static IEnumerable<ITreeNode<T>> WalkMany<T>(this IEnumerable<ITreeNode<T>> nodes, int maxDepth = Int32.MaxValue)
        {
            return nodes.SelectMany(node => node.Walk(maxDepth));
        }

        /// <summary>
        /// Searches the navigation tree for the first node that satisfies the predicate, returning the list of nodes visited.
        /// </summary>
        public static IEnumerable<ITreeNode<T>> FindPath<T>(this ITreeNode<T> node, Predicate<ITreeNode<T>> predicate) 
        {
            // if this node satisfies the predicate, then we've finished
            if (predicate(node))
            {
                return new List<ITreeNode<T>> { node };
            }

            // check each child node to try to find the active
            foreach (var child in node.Children)
            {
                var pathToActive = FindPath(child, predicate).ToList();
                if (pathToActive.Any())
                {
                    // this child contains the active node either directly or indirectly, 
                    // so add the parent to the beginning and return
                    pathToActive.Insert(0, node);
                    return pathToActive;
                }
            }

            // the active node was not found on this path
            return new List<ITreeNode<T>>();
        }
    }
}
