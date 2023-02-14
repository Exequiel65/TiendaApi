using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaApi.Dtos;
using TiendaApi.Services;

namespace TiendaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsuariosController(IUserService userService) {
            _userService = userService;

        }
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterDto model)
        {
            var result = await _userService.RegisterAsync(model);
            return Ok(result);
        }
    }
}
