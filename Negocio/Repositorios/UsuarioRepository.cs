using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Negocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Repositorios
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<Usuario> GetByUserNameAsync(string userName)
        {
            return await _context.Usuarios
                                .Include(u => u.Roles)
                                .FirstOrDefaultAsync(u => u.Username.ToLower() == userName.ToLower()); 
        }
    }
}
