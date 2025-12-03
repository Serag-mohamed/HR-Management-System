using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRManagementSystem.Helpers
{
    public static class ActiveLinkHelper
    {
        public static string IsActive(ViewContext viewContext, string controller, string action = "Index")
        {
            var currentController = viewContext.RouteData.Values["controller"]?.ToString() ?? "";
            var currentAction = viewContext.RouteData.Values["action"]?.ToString() ?? "";

            return (currentController.Equals(controller, StringComparison.OrdinalIgnoreCase) &&
                    currentAction.Equals(action, StringComparison.OrdinalIgnoreCase))
                    ? "active"
                    : "";
        }
    }

}
