using Entities.Models;
using Negocio.Interfaces;
using TiendaApi.Dtos;

namespace TiendaApi.Services
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterDto model);
    }
}
