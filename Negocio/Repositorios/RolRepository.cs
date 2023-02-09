using Entities;
using Entities.Models;
using Negocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Repositorios
{
    public class RolRepository : GenericRepository<Rol>, IRolRepository
    {
        public RolRepository (ApplicationDbContext context) : base(context)
        {

        }
    }
}
