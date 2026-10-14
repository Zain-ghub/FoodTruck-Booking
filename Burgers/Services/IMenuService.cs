using Burgers.Models;
namespace Burgers.Services
{
    public interface IMenuService
    {
        List<MenuItem> GetMenuItems();
        List<MenuItem> GetAllMenuItems();
        MenuItem? GetMenuItemById(int id);
        void AddMenuItem(MenuItem menuItem);
        bool UpdateMenuItem(MenuItem menuItem);
        bool DeleteMenuItem(int id);
    }
}
