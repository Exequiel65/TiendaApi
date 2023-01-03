using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Producto : BaseEntity
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(50)]
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaCrecion { get; set; }
        public int MarcaId { get; set; }
        public Marca Marca { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}
