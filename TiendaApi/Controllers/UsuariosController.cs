using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
