using System.Collections.Generic;

namespace MC.MvcQuickNav
{
    public interface ITreeNode<T>
    {
        T Value { get; set; }
        IEnumerable<ITreeNode<T>> Children { get; set; }
        void AddChild(T value);
        void RemoveChildren();
    }
}
