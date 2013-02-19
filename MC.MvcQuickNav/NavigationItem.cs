namespace MC.MvcQuickNav
{
    public class NavigationItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public bool OpenInNewWindow { get; set; }
        public bool IsActive { get; set; }
    }
}
