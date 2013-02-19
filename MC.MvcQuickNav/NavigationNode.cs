using System.Collections.Generic;
using System.Linq;

namespace MC.MvcQuickNav
{
    public class NavigationNode : ITreeNode<NavigationItem>
    {
        private readonly List<NavigationNode> _children;

        public NavigationItem Value { get; private set; }
        public IEnumerable<ITreeNode<NavigationItem>> Children { get { return _children; } }

        public NavigationNode()
            : this(null)
        {}

        public NavigationNode(NavigationItem value)
            : this(value, new List<NavigationNode>())
        {}

        public NavigationNode(NavigationItem value, IEnumerable<NavigationNode> children)
        {
            Value = value;
            _children = children.ToList();
        }

        public void AddChild(NavigationItem value)
        {
            _children.Add(new NavigationNode(value));
        }

        public void RemoveChildren()
        {
            _children.Clear();
        }

        public override string ToString()
        {
            return Value.Url;
        }
    }
}