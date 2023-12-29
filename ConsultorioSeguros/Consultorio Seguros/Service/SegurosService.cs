using Azure;
using Consultorio_Seguros.Helper;
using Consultorio_Seguros.Models.Application;
using Consultorio_Seguros.Models.BD;
using Consultorio_Seguros.Models.Model;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using static System.Net.WebRequestMethods;

namespace Consultorio_Seguros.Service
{
    public class SegurosService
    {
        private readonly DataBaseContext _context;
        private readonly string _connectionString;
        public SegurosService(DataBaseContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Obtener todos los seguros existentes
        /// </summary>
        public async Task<ResponseJson> GetSeguros()
        {
            ResponseJson response = new ResponseJson();
            try
            {
                var seguros = await _context.Seguros.Where(s => s.Estado == "A").ToListAsync();
                response.Data = (IEnumerable<object>)seguros;
                response.Message = "Consulta exitosa";
                response.error = false;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.error = true;
            }
            return response;
        }

        /// <summary>
        /// Obtener seguro por id
        /// </summary>
        public async Task<ResponseJson> GetSeguro(int id)
        {
            ResponseJson response = new ResponseJson();
            try
            {
                var seguro = await _context.Seguros.Where(s => s.Estado == "A" && s.Id == id).ToListAsync();
                if(seguro == null) return new ResponseJson { Message = "No se encontró el registro", error = true };
                response.Data = (IEnumerable<object>) seguro;
                response.Message = "Consulta exitosa";
                response.error = false;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.error = true;
            }
            return response;
        }


        /// <summary>
        /// Agregar seguro
        /// </summary>
        public async Task<ResponseJson> AddSeguro(SegurosModel seguro)
        {
            ResponseJson response = new ResponseJson();
            ApplicationMethods metA = new ApplicationMethods();
            try
            {
                var seguroFindForId = await _context.Seguros
                    .Include(s => s.Asegurados)
                    .FirstOrDefaultAsync(s => s.Codigo == seguro.Codigo && s.Ramo == seguro.Ramo);

                if (seguroFindForId != null) return new ResponseJson { Message = "Codigo y ramo existentes", error = true };

                if (seguro.SumaAsegurada == 0 || seguro.Nombre == null || seguro.Prima == 0 || seguro.Codigo == null) return metA.ReturnJsonError("fieldNull");
                if (seguro.SumaAsegurada < 0 || seguro.Prima < 0) return metA.ReturnJsonError("fieldNegative");
                if (seguro.SumaAsegurada > 100000 || seguro.Prima > 100000) return metA.ReturnJsonError("fieldMax");
                if (seguro.Nombre.Trim() == "" || seguro.Codigo.Trim() == "") return metA.ReturnJsonError("fieldEmpty");

                Seguros seguros = new Seguros();
                seguros.Prima = seguro.Prima;
                seguros.SumaAsegurada = seguro.SumaAsegurada;
                seguros.Codigo = seguro.Codigo;
                seguros.Nombre = seguro.Nombre;
                seguros.Ramo = seguro.Ramo;
                seguros.Estado = "A";
                _context.Seguros.Add(seguros);
                await _context.SaveChangesAsync();
                response.Data = null;
                response.Message = "Registro exitoso";
                response.error = false;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.error = true;
            }
            return response;
        }

        /// <summary>
        /// Actualizar seguro
        /// </summary>
        public async Task<ResponseJson> UpdateSeguro(int id, SegurosModel seguro)
        {
            ResponseJson response = new ResponseJson();
            ApplicationMethods metA = new ApplicationMethods();
            try
            {
                var seguroExistente = await _context.Seguros.FindAsync(id);

                if (seguroExistente != null)
                {
                    var seguroFindForId = await _context.Seguros
                    .Include(s => s.Asegurados)
                    .FirstOrDefaultAsync(s => s.Codigo == seguro.Codigo && s.Ramo == seguro.Ramo && s.Id != id);

                    if (seguroFindForId != null) return new ResponseJson { Message = "Codigo y ramo existentes", error = true };

                    if (seguro.SumaAsegurada == 0 || seguro.Nombre == null || seguro.Prima == 0 || seguro.Codigo == null) return metA.ReturnJsonError("fieldNull");
                    if (seguro.SumaAsegurada < 0 || seguro.Prima < 0) return metA.ReturnJsonError("fieldNegative");
                    if (seguro.SumaAsegurada > 100000 || seguro.Prima > 100000) return metA.ReturnJsonError("fieldMax");
                    if (seguro.Nombre.Trim() == "" || seguro.Codigo.Trim() == "") return metA.ReturnJsonError("fieldEmpty");

                    seguroExistente.Nombre = seguro.Nombre;
                    seguroExistente.Codigo = seguro.Codigo;
                    seguroExistente.SumaAsegurada = seguro.SumaAsegurada;
                    seguroExistente.Prima = seguro.Prima;
                    seguroExistente.Ramo = seguro.Ramo;


                    await _context.SaveChangesAsync();
                    response.Data = null;
                    response.Message = "Actualizacion exitosa";
                    response.error = false;
                }
                else
                {
                    return metA.ReturnJsonError("fieldNull");
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.error = true;
            }
            return response;
        }

        /// <summary>
        /// Eliminar seguro
        /// </summary>
        public async Task<ResponseJson> DeleteSeguro(int id)
        {
            ResponseJson response = new ResponseJson();
            ApplicationMethods metA = new ApplicationMethods();
            try
            {
                var seguro = _context.Seguros.Find(id);
                if (seguro == null) return new ResponseJson { Message = "No se encontró el registro", error = true };
                if (id == 0) return metA.ReturnJsonError("fieldNull");

                seguro.Estado = "I";

                //_context.Seguros.Remove(seguro);
                await _context.SaveChangesAsync();
                response.Data = null;
                response.Message = "Eliminación exitosa";
                response.error = false;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.error = true;
            }
            return response;
        }

        /// <summary>
        /// Obtener seguro por medio del codigo del seguro
        /// </summary>
        public async Task<ResponseJson> GetAseguradosByCodigoSeguro(string codigo)
        {
            try
            {
                var seguro = await _context.Seguros
                    .Include(s => s.Asegurados)
                    .FirstOrDefaultAsync(s => s.Codigo == codigo);

                if (seguro == null)
                {
                    return new ResponseJson { Message = "Seguro no encontrado", error = true };
                }

                var asegurados = seguro.Asegurados;

                return new ResponseJson { Message = "Asegurados obtenidos exitosamente", Data = asegurados, error = false };
            }
            catch (Exception ex)
            {
                return new ResponseJson { Message = ex.Message, error = true };
            }
        }

    }
}
