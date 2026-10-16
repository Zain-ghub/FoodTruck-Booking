using Burgers.Models;
namespace Burgers.Core.Interfaces
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
