MC.MvcQuickNav
==============

A series of MVC HTML helpers to generate navigation menus, breadcrumb trails and child navigation controls,
supporting an arbitrary level of depth.

All navigation is driven from a single tree of navigation nodes.  The user's current URL is compared with each node to 
determine which is the best match for the their location in the site.  This information is then used to determine which
sub-tree(s) to render for each control.

All helpers render their markup semantically as a series of nested unordered list tags, with classes added to the 
active node and those with children to enable the UI to distinguish these to the user.

The HTML rendered by the NavigationMenu() helper is compatible with the CSS provided by
http://cssmenumaker.com/, so this can be easily dropped in and tweaked, else you can design the CSS from scratch.

An XML provider is included, which parses the navigation tree from an XML file.  It uses the same file format as
ASP.NET WebForms web.sitemap, so you can just drop this file into the root of your website.

See the example web project for a full exmaple.
