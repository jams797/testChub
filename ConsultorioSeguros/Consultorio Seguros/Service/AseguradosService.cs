using Azure;
using Consultorio_Seguros.Helper;
using Consultorio_Seguros.Models.Application;
using Consultorio_Seguros.Models.BD;
using Consultorio_Seguros.Models.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using System.Data;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Consultorio_Seguros.Service
{
    public class AseguradosService
    {
        private readonly DataBaseContext _context;
        private readonly string _connectionString;
        public AseguradosService(DataBaseContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Obtener Todos los asegurados.
        /// </summary>
        public async Task<ResponseJson> GetAsegurados()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("GetAsegurados", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows)
                            {
                                return new ResponseJson { Message = "Asegurado no encontrado", error = true };
                            }

                            var asegurados = new List<Asegurados>();

                            while (await reader.ReadAsync())
                            {
                                var asegurado = new Asegurados
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Cedula = reader.GetString(reader.GetOrdinal("Cedula")),
                                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                    Correo = reader.GetString(reader.GetOrdinal("Correo")),
                                    Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                                    Edad = reader.GetInt32(reader.GetOrdinal("Edad")),
                                    Estado = reader.GetString(reader.GetOrdinal("Estado"))
                                };

                                asegurados.Add(asegurado);
                            }

                            return new ResponseJson { Message = "Asegurados obtenidos exitosamente", Data = asegurados, error = false };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseJson { Message = ex.Message, error = true };
            }
        }

        /// <summary>
        /// Busqueda por id de asegurados
        /// </summary>
        public async Task<ResponseJson> GetAsegurado(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("GetAsegurado", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", id));

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows)
                            {
                                return new ResponseJson { Message = "Asegurado no encontrado", error = true };
                            }

                            var asegurados = new List<Asegurados>();

                            while (await reader.ReadAsync())
                            {
                                var asegurado = new Asegurados
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Cedula = reader.GetString(reader.GetOrdinal("Cedula")),
                                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                    Correo = reader.GetString(reader.GetOrdinal("Correo")),
                                    Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                                    Edad = reader.GetInt32(reader.GetOrdinal("Edad")),
                                    Estado = reader.GetString(reader.GetOrdinal("Estado"))
                                };

                                asegurados.Add(asegurado);
                            }

                            return new ResponseJson { Message = "Asegurado obtenido exitosamente", Data = asegurados, error = false };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseJson { Message = ex.Message, error = true };
            }
        }

        /// <summary>
        /// Agregar nuevo asegurado
        /// </summary>
        public async Task<ResponseJson> AddAsegurado(AseguradosModel asegurados)
        {
            ResponseJson response = new ResponseJson();
            ApplicationMethods metA = new ApplicationMethods();
            ValidationAsegurado validationAsegurado = new ValidationAsegurado();

            try
            {
                if (asegurados.Cedula == null || asegurados.Edad == 0 || asegurados.Nombre == null || asegurados.Telefono == null) return metA.ReturnJsonError("fieldNull");
                if (asegurados.Edad < 18 || asegurados.Edad > 100) return metA.ReturnJsonError("ageInvalid");
                if (!validationAsegurado.ValidarCedula(asegurados.Cedula)) return metA.ReturnJsonError("cedulaInvalid");
                if(!validationAsegurado.IsValidEmail(asegurados.Correo)) return metA.ReturnJsonError("emailErrorInvalid");
                if (!validationAsegurado.IsDigitOnly(asegurados.Telefono) || !Regex.IsMatch(asegurados.Telefono, @"^[\d]{10}$")) return metA.ReturnJsonError("phoneInvalid");
                if (asegurados.Cedula.Trim() == "" || asegurados.Nombre.Trim() == "" || asegurados.Telefono == "" || asegurados.Edad.ToString().Trim() == "") return metA.ReturnJsonError("fieldEmpty");
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("AddAsegurado", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Cedula", asegurados.Cedula);
                        command.Parameters.AddWithValue("@Nombre", asegurados.Nombre);
                        command.Parameters.AddWithValue("@Telefono", asegurados.Telefono);
                        command.Parameters.AddWithValue("@Edad", asegurados.Edad);
                        command.Parameters.AddWithValue("@Correo", asegurados.Correo);
                        command.Parameters.AddWithValue("@Estado", "A");

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                response.Message = reader["Message"].ToString();
                                response.error = (int)reader["Error"] == 1;
                            }
                        }
                    }
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
        /// Actualizar registro de asegurado
        /// </summary>
        public async Task<ResponseJson> UpdateAsegurado(int id, AseguradosModel asegurados)
        {
            ResponseJson response = new ResponseJson();
            ApplicationMethods metA = new ApplicationMethods();
            ValidationAsegurado validationAsegurado = new ValidationAsegurado();

            try
            {
                if (asegurados.Cedula == null || asegurados.Edad == 0 || asegurados.Nombre == null || asegurados.Telefono == null) return metA.ReturnJsonError("fieldNull");
                if (asegurados.Edad < 18 || asegurados.Edad > 100) return metA.ReturnJsonError("ageInvalid");
                if (!validationAsegurado.ValidarCedula(asegurados.Cedula)) return metA.ReturnJsonError("cedulaInvalid");
                if (!validationAsegurado.IsValidEmail(asegurados.Correo)) return metA.ReturnJsonError("emailErrorInvalid");
                if (!validationAsegurado.IsDigitOnly(asegurados.Telefono) || !Regex.IsMatch(asegurados.Telefono, @"^[\d]{10}$")) return metA.ReturnJsonError("phoneInvalid");
                if (asegurados.Cedula.Trim() == "" || asegurados.Nombre.Trim() == "" || asegurados.Telefono == "" || asegurados.Edad.ToString().Trim() == "") return metA.ReturnJsonError("fieldEmpty");
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("UpdateAsegurado", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Cedula", asegurados.Cedula);
                        command.Parameters.AddWithValue("@Nombre", asegurados.Nombre);
                        command.Parameters.AddWithValue("@Telefono", asegurados.Telefono);
                        command.Parameters.AddWithValue("@Edad", asegurados.Edad);
                        command.Parameters.AddWithValue("@Correo", asegurados.Correo);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                response.Message = reader["Message"].ToString();
                                response.error = (int)reader["Error"] == 1;
                            }
                        }
                    }
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
        /// Eliminar Asegurado
        /// </summary>
        public async Task<ResponseJson> DeleteAsegurado(int id)
        {
            ResponseJson response = new ResponseJson();
            ApplicationMethods metA = new ApplicationMethods();
            ValidationAsegurado validationAsegurado = new ValidationAsegurado();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("DeleteAsegurado", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Estado", "I");

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                response.Message = reader["Message"].ToString();
                                response.error = (int)reader["Error"] == 1;
                            }
                        }
                    }
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
        /// Obtener archio subido y procesarlo
        /// </summary>
        public async Task<ResponseJson> ProcessFileAsync(IFormFile file)
        {
            if (file.Length > 10485760)
            {
                return new ResponseJson { error = true, Message = "El archivo excede el límite de 10MB" };
            }

            string fileExtension = Path.GetExtension(file.FileName)?.ToLower();

            if (fileExtension == ".txt")
            {
                var asegurados = await ReadFileAsync(file);
                return await AddAseguradosToDatabaseAsync(asegurados);
            }
            else if (fileExtension == ".xlsx")
            {
                var asegurados = await ReadExcelFileAsync(file);
                return await AddAseguradosToDatabaseAsync(asegurados);
            }
            else
            {
                return new ResponseJson { error = true, Message = "Formato de archivo no compatible." };
            }
        }

        /// <summary>
        /// Lee archivo en formato txt
        /// </summary>
        private async Task<List<AseguradosModel>> ReadFileAsync(IFormFile file)
        {
            List<AseguradosModel> asegurados = new List<AseguradosModel>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                try
                {
                    bool isFirstLine = true;
                    while (reader.Peek() >= 0)
                    {
                        var line = await reader.ReadLineAsync();
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }
                        var values = line.Split(',');
                        if (int.TryParse(values[3], out int edad))
                        {
                            asegurados.Add(new AseguradosModel
                            {
                                Cedula = values[0],
                                Nombre = values[1],
                                Telefono = values[2],
                                Edad = edad,
                                Correo = values[4]
                            });
                        }
                    }
                }
                catch(Exception ex)
                {
                    return null;
                }
                
            }
            return asegurados;
        }

        /// <summary>
        /// Lee archivo en formato excel
        /// </summary>
        private async Task<List<AseguradosModel>> ReadExcelFileAsync(IFormFile file)
        {
            List<AseguradosModel> asegurados = new List<AseguradosModel>();

            using (var package = new ExcelPackage(file.OpenReadStream()))
            {
                var worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) 
                {
                    asegurados.Add(new AseguradosModel
                    {
                        Cedula = worksheet.Cells[row, 1].Value?.ToString(),
                        Nombre = worksheet.Cells[row, 2].Value?.ToString(),
                        Telefono = worksheet.Cells[row, 3].Value?.ToString(),
                        Edad = Convert.ToInt32(worksheet.Cells[row, 4].Value),
                        Correo = worksheet.Cells[row, 5].Value?.ToString(),
                    });
                }
            }

            return asegurados;
        }

        /// <summary>
        /// Agrega la lista de asegurados obtenidas por los archivos
        /// </summary>
        private async Task<ResponseJson> AddAseguradosToDatabaseAsync(List<AseguradosModel> asegurados)
        {
            ResponseJson response = new ResponseJson();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    foreach (var asegurado in asegurados)
                    {
                        using (SqlCommand command = new SqlCommand("AddAsegurado", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Cedula", asegurado.Cedula);
                            command.Parameters.AddWithValue("@Nombre", asegurado.Nombre);
                            command.Parameters.AddWithValue("@Telefono", asegurado.Telefono);
                            command.Parameters.AddWithValue("@Edad", asegurado.Edad);
                            command.Parameters.AddWithValue("@Correo", asegurado.Correo);
                            command.Parameters.AddWithValue("@Estado", "A");

                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                if (reader.Read())
                                {
                                    response.Message = reader["Message"].ToString();
                                    response.error = (int)reader["Error"] == 1;
                                }
                            }
                        }

                        
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseJson { Message = ex.Message, error = true };
            }
            return response;
        }

        /// <summary>
        /// Asignar uno o mas seguro a un asegurado
        /// </summary>
        public async Task<ResponseJson> AssignSegurosToAsegurado(int aseguradoId, List<int> segurosIds)
        {
            ResponseJson response = new ResponseJson();
            ApplicationMethods metA = new ApplicationMethods();
            try
            {
                if (aseguradoId.ToString().Length == 0 || segurosIds == null) return metA.ReturnJsonError("fieldNull");
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    foreach (var seguroId in segurosIds)
                    {
                        using (SqlCommand command = new SqlCommand("AssignSegurosToAsegurado", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@IdAsegurado", aseguradoId);
                            command.Parameters.AddWithValue("@IdSeguro", seguroId);


                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                if (reader.Read())
                                {
                                    response.Message = reader["Message"].ToString();
                                    response.error = (int)reader["Error"] == 1;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseJson { Message = ex.Message, error = true };
            }
            return response;
        }

        /// <summary>
        /// Eliminar uno o mas seguro a un asegurado
        /// </summary>
        public async Task<ResponseJson> DeleteSegurosToAsegurado(int aseguradoId, List<int> segurosIds)
        {
            ResponseJson response = new ResponseJson();
            ApplicationMethods metA = new ApplicationMethods();
            try
            {
                if (aseguradoId.ToString().Length == 0 || segurosIds == null) return metA.ReturnJsonError("fieldNull");
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    foreach (var seguroId in segurosIds)
                    {
                        using (SqlCommand command = new SqlCommand("DeleteSegurosToAsegurado", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@IdAsegurado", aseguradoId);
                            command.Parameters.AddWithValue("@IdSeguro", seguroId);
                            command.Parameters.AddWithValue("@Estado", "I");


                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                if (reader.Read())
                                {
                                    response.Message = reader["Message"].ToString();
                                    response.error = (int)reader["Error"] == 1;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseJson { Message = ex.Message, error = true };
            }
            return response;
        }

        /// <summary>
        /// Obtener los seguros activos de un asegurado por medio de la cedula
        /// </summary>
        public async Task<ResponseJson> GetSegurosByCedula(string cedula)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("GetSegurosByCedula", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@cedula", cedula));

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows)
                            {
                                return new ResponseJson { Message = "Asegurado no encontrado", error = true };
                            }

                            var seguros = new List<Seguros>();

                            while (await reader.ReadAsync())
                            {
                                var seguro = new Seguros
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                    Codigo = reader.GetString(reader.GetOrdinal("Codigo")),
                                    SumaAsegurada = reader.GetDecimal(reader.GetOrdinal("SumaAsegurada")),
                                    Prima = reader.GetDecimal(reader.GetOrdinal("Prima")),
                                    Ramo = reader.GetString(reader.GetOrdinal("Ramo"))
                                };

                                seguros.Add(seguro);
                            }

                            return new ResponseJson { Message = "Seguros obtenidos exitosamente", Data = seguros, error = false };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseJson { Message = ex.Message, error = true };
            }
        }

        /// <summary>
        /// Obtener los seguros activos de un asegurado por medio del id
        /// </summary>
        public async Task<ResponseJson> GetSegurosById(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("GetSegurosById", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", id));

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows)
                            {
                                return new ResponseJson { Message = "Asegurado no encontrado", error = true };
                            }

                            var seguros = new List<Seguros>();

                            while (await reader.ReadAsync())
                            {
                                var seguro = new Seguros
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                    Codigo = reader.GetString(reader.GetOrdinal("Codigo")),
                                    SumaAsegurada = reader.GetDecimal(reader.GetOrdinal("SumaAsegurada")),
                                    Prima = reader.GetDecimal(reader.GetOrdinal("Prima")),
                                    Ramo = reader.GetString(reader.GetOrdinal("Ramo"))
                                };

                                seguros.Add(seguro);
                            }

                            return new ResponseJson { Message = "Seguros obtenidos exitosamente", Data = seguros, error = false };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseJson { Message = ex.Message, error = true };
            }
        }
    }
}

