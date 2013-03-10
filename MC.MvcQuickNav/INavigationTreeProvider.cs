using System.Collections.Generic;

namespace MC.MvcQuickNav
{
    /// <summary>
    /// Encapsulates a class that can act as the source of a navigation tree.
    /// </summary>
    public interface INavigationTreeProvider
    {
        IEnumerable<NavigationNode> GetNavigationNodes();
    }
}
