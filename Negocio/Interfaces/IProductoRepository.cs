using Entities.Models;

namespace Negocio.Interfaces
{
    public interface IProductoRepository : IGenericRepository<Producto>
    {
        Task<IEnumerable<Producto>> GetProductosMasCaros(int cantidad);
    }
}
