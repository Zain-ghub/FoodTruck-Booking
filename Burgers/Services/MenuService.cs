using Burgers.Data;
using Burgers.Models;
namespace Burgers.Services
{
    public class MenuService : IMenuService
    {
        private readonly BurgersContext _context;
        public MenuService(BurgersContext context)
        {
            _context = context;
        }
        public List<MenuItem> GetMenuItems()
        {
            return _context.MenuItems.Where(e => e.IsAvailable).OrderBy(e => e.Category).ThenBy(e => e.Name).ToList();
        }
        public List<MenuItem> GetAllMenuItems()
        {
            return _context.MenuItems.OrderBy(e => e.Category).ThenBy(e => e.Name).ToList();
        }
        public MenuItem? GetMenuItemById(int id)
        {
            return _context.MenuItems.Find(id);
        }
        public void AddMenuItem(MenuItem menuItem)
        {
            _context.MenuItems.Add(menuItem);
            _context.SaveChanges();
        }
        public bool UpdateMenuItem(MenuItem menuItem)
        {

            if (!_context.MenuItems.Any(e => e.Id == menuItem.Id))
            {
                return false;
            }
            _context.MenuItems.Update(menuItem);
            _context.SaveChanges();
            return true;
        }
        public bool DeleteMenuItem(int id)
        {
            var menuItem = _context.MenuItems.Find(id);
            if (menuItem != null)
            {
                _context.MenuItems.Remove(menuItem);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
