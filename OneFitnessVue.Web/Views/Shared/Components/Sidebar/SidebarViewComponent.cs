using Microsoft.AspNetCore.Mvc;

namespace FitnessTimeGym.Web.Views.Shared.Components.Sidebar
{
    public class SidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}