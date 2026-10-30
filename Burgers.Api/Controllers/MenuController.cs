using Burgers.Core.Interfaces;
using Burgers.Models;
using Microsoft.AspNetCore.Mvc;

namespace Burgers.Api.Controllers
{
    [ApiController]
    [Route("api/menu")]
    public class MenuController : ControllerBase
    {
       private readonly IMenuService _menuService;

        public MenuController(IMenuService service)
        {
            _menuService = service;
        }

        [HttpGet]
        public IActionResult GetMenuItems() 
        { 
             var result = _menuService.GetMenuItems();

            return Ok(result);
        }
    }
}
