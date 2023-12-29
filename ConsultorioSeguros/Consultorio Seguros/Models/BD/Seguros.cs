using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Consultorio_Seguros.Models.BD
{
    public class Seguros
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public decimal SumaAsegurada { get; set; }
        public decimal Prima { get; set; }
        public string Ramo { get; set; }
        public string Estado { get; set; }
        public ICollection<Asegurados> Asegurados { get; set; }
    }
}
