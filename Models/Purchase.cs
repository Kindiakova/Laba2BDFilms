using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laba2FilmsBD.Models
{
    public partial class Purchase
    {
        [Required]
        [Display(Name = "Фільм")]
        public int FilmId { get; set; }
        [Required]
        [Display(Name = "Покупець")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Поле не має бути порожнім")]
        [Display(Name = "Ціна")]
        [Range(0, 100000009, ErrorMessage = "Вартість має бути >= 0")]
        public int? Cost { get; set; }

        [Display(Name = "Дата і час платежу")]
        [Required(ErrorMessage = "Поле не має бути порожнім")]
        public DateTime? PaymentDay { get; set; }

        [Display(Name = "Покупець")]
        public virtual Customer Customer { get; set; } = null!;

        [Display(Name = "Фільм")]
        public virtual Film Film { get; set; } = null!;
    }
}
