using Garden_Centre_MVC.Models;

namespace Garden_Centre_MVC.Attributes.Assets
{
    /// <summary>
    /// this class will store the users details as they login untill
    /// there sessions is destroyed.
    /// this is static so it can be accessed anywhere throughout the application
    /// and there is only ever one instance.
    /// </summary>
    public static class CurrentUser
    {
        public static EmployeeLogin EmployeeLogin { get; set; }
    }
}