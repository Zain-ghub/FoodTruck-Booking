using Burgers.Api.DTOs;
using Burgers.Core.Interfaces;
using Burgers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Burgers.Api.Controllers
{
    [Route("api/admin/menu")]
    [ApiController]
    [Authorize]
    public class AdminMenuController : ControllerBase
    {
        private readonly IMenuService _menuservice;
        public AdminMenuController(IMenuService menuService)
        {
            _menuservice = menuService;
        }

        [HttpGet]
        public ActionResult<List<MenuItemAdminDto>> GetAllMenuItems()
        {
            var items = _menuservice.GetAllMenuItems();

            var res = items.Select(x => new MenuItemAdminDto
            {
                Id = x.Id,
                Name = x.Name,
                IsAvailable = x.IsAvailable,
                Price = x.Price,
                Category = x.Category,
                Description = x.Description,
            }).ToList();

            return Ok(res);
        }
        [HttpGet("{id}")]
        public ActionResult<MenuItemAdminDto> GetMenuItemById(int id)
        {
            var item = _menuservice.GetMenuItemById(id);

            if (item == null) { return NotFound(); }

            var res = new MenuItemAdminDto
            {
                Category = item.Category,
                Id = item.Id,
                Name = item.Name,
                IsAvailable = item.IsAvailable,
                Price = item.Price,
                Description = item.Description
            };

            return Ok(res);
        }
        [HttpPost]
        public ActionResult<MenuItemAdminDto> PostMenuItem(MenuItemDto item)
        {
            var result = new MenuItem {
                Category = item.Category,
                Description = item.Description,
                Price = item.Price,
                Name = item.Name,
                IsAvailable = item.IsAvailable,
            };
            _menuservice.AddMenuItem(result);

            var dto = new MenuItemAdminDto
            {
                Category = result.Category,
                Description = result.Description,
                Price = result.Price,
                Name = result.Name,
                IsAvailable = result.IsAvailable,
                Id = result.Id,
            };

            return CreatedAtAction(nameof(GetMenuItemById), new { id = dto.Id }, dto);

        }
        [HttpDelete("{id}")]
        public ActionResult DeleteMenuItemById(int id)
        {
            var res = _menuservice.DeleteMenuItem(id);

            return res ? NoContent() : NotFound();
        }

        [HttpPut("{id}")]
        public ActionResult PutMenuItem(MenuItemDto item, int id) 
        {
            var ent = new MenuItem
            {
                Category = item.Category,
                Description = item.Description,
                Price = item.Price,
                Name = item.Name,
                IsAvailable = item.IsAvailable,
                Id = id
            };
           var res = _menuservice.UpdateMenuItem(ent);

            return res ? NoContent() : NotFound();
        }


    }
}
