using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.MvcQuickNav
{
    public interface ISiteMapProvider
    {
        IEnumerable<NavigationNode> GetSiteMap();
    }
}
