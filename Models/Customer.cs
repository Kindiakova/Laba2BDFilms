using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Laba2FilmsBD.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Purchases = new HashSet<Purchase>();
        }

        [Required]        
        public int Id { get; set; }

        [Display(Name = "Ім'я")]
        public string? FirstName { get; set; }

        [Display(Name = "Прізвище")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Поле не має бути порожнім")]
        [Remote("NameNotExists", "Customers", AdditionalFields = "Email", ErrorMessage = "Покупець з цією адресою вже існує")]

        public string? Email { get; set; }

        [NotMapped]
        [Display(Name = "Загальна сума витрат")]
        public int? TotalAmount { get; set; }

        [Display(Name = "Останній візит на сайт")]
        public DateTime? LastActiveDate { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
