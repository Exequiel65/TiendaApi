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
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Producto>> GetProductosMasCaros(int cantidad) =>
                        await _context.Productos
                            .OrderByDescending(p => p.Precio)
                            .Take(cantidad)
                            .ToListAsync();
        public override async Task<Producto> GetByIdAsync(int id)
        {
            return await _context.Productos
                            .Include(p => p.Marca)
                            .Include(p => p.Categoria)
                            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _context.Productos
                            .Include(p => p.Marca)
                            .Include(p => p.Categoria)
                            .ToListAsync();
        }

        public override async Task<(int totalRegistros, IEnumerable<Producto> registros)> GetAllAsync(int pageIndex, int pageSize)
        {
            var totalRegistros = await _context.Productos.CountAsync();
            var registros = await _context.Productos
                                .Include(u => u.Marca)
                                .Include(U => U.Categoria)
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
            return (totalRegistros, registros);
        }
    }

}
