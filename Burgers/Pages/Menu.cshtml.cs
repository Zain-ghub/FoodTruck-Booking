using Microsoft.AspNetCore.Mvc;
using Burgers.Services;
using Burgers.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Burgers.Core.Interfaces;

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
