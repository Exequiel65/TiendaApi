using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class UsuariosRoles
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int RoleId { get; set; }
        public Rol Rol { get; set; }
    }
}
