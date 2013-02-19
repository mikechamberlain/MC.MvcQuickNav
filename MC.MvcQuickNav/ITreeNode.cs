using System.Collections.Generic;

namespace MC.MvcQuickNav
{
    public interface ITreeNode<T>
    {
        T Value { get; }
        IEnumerable<ITreeNode<T>> Children { get; }
        void AddChild(T value);
        void RemoveChildren();
    }
}
