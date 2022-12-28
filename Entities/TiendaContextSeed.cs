using CsvHelper;
using Entities.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class TiendaContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
			try
			{
				var ruta = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				if (!context.Marcas.Any())
				{
                    // using(var reedMarcas = new StreamReader(ruta + @"/Csvs/marcas.csv")) No se usa porque da error
                    using (var reedMarcas = new StreamReader(@"../Entities/Csvs/marcas.csv"))
					{
						using(var csvMarcas = new CsvReader(reedMarcas, CultureInfo.InvariantCulture))
						{
							var marcas = csvMarcas.GetRecords<Marca>();
							context.Marcas.AddRange(marcas);
							await context.SaveChangesAsync();
						}
					}
				}
                if (!context.Categorias.Any())
                {
                    using (var reedCategoria = new StreamReader(@"../Entities/Csvs/categorias.csv"))
                    {
                        using (var csvCategorias = new CsvReader(reedCategoria, CultureInfo.InvariantCulture))
                        {
                            var categorias = csvCategorias.GetRecords<Categoria>();
                            context.Categorias.AddRange(categorias);
                            await context.SaveChangesAsync();
                        }
                    }
                }

                if (!context.Productos.Any())
                {
                    using (var reedProduct = new StreamReader(@"../Entities/Csvs/productos.csv"))
                    {
                        using (var csvProductos = new CsvReader(reedProduct, CultureInfo.InvariantCulture))
                        {
                            var listadoProductosCsv = csvProductos.GetRecords<Producto>();

                            List<Producto> productos = new List<Producto>();
                            foreach (var producto in listadoProductosCsv)
                            {
                                productos.Add(new Producto
                                {
                                    Id = producto.Id,
                                    Nombre = producto.Nombre,
                                    Precio = producto.Precio,
                                    FechaCrecion = producto.FechaCrecion,
                                    CategoriaId = producto.CategoriaId,
                                    MarcaId = producto.MarcaId
                                });
                            }
                            context.Productos.AddRange(productos);
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<TiendaContextSeed>();
				logger.LogError(ex.Message);
				throw;
			}
        }
    }
}
