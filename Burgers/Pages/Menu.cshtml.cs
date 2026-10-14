using Microsoft.AspNetCore.Mvc;
using Burgers.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Burgers.Models;

namespace Burgers.Pages
{
    public class MenuModel : PageModel
    {
        private readonly IMenuService _menuService;

        public MenuModel(IMenuService menuService)
        {
            _menuService = menuService;
        }
        public List<MenuItem> MenuItems { get; set; } = new();
        public void OnGet()
        {
            MenuItems = _menuService.GetMenuItems();
        }
    }
}
