using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Laba2FilmsBD.Models
{
    [NotMapped]
    public class AdditionalClass
    {
        public int ActorId { get; set; }
        public string ActorFirstName { get; set; } = null!;

        public string ActorLastName { get; set; } = null!;

        public int? CustomerId { get; set; }

        public string? CustomerFirstName { get; set; }

        public string? CustomerLastName { get; set; }

        public string? CustomerEmail { get; set; }

    }
}
