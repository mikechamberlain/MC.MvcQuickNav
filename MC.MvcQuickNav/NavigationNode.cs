using System.Collections.Generic;
using System.Linq;

namespace MC.MvcQuickNav
{
    public class NavigationNode : ITreeNode<NavigationItem>
    {
        private List<ITreeNode<NavigationItem>> _children;

        public NavigationItem Value { get; set; }
        public IEnumerable<ITreeNode<NavigationItem>> Children
        {
            get { return _children; }
            set { _children = value.ToList(); }
        }

        public NavigationNode()
            : this(null)
        {}

        public NavigationNode(NavigationItem value)
            : this(value, new List<NavigationNode>())
        {}

        public NavigationNode(NavigationItem value, IEnumerable<ITreeNode<NavigationItem>> children)
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