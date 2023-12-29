using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Consultorio_Seguros.Models.BD
{
    public class Asegurados
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public int Edad { get; set; }
        public string Correo { get; set; }
        public string Estado { get; set; }
        public ICollection<Seguros> Seguros { get; set; }
    }
}
