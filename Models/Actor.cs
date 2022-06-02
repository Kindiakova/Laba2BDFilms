using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laba2FilmsBD.Models
{
    public partial class Actor
    {
        public Actor()
        {
            ActorsInFilms = new HashSet<ActorsInFilm>();
        }

        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не має бути порожнім")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; } = null!;

        
        [Required(ErrorMessage = "Поле не має бути порожнім")]
        [Display(Name = "Прізвище")]
        [Remote("NameNotExists", "Actors", AdditionalFields = "FirstName, LastName", ErrorMessage = "Людина з цим іменем вже існує")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Стать")]
        public string? Gender { get; set; }

        [Display(Name = "День народження")]
        public DateTime? BirthDay { get; set; }

       /* [Display(Name = "День народження")]
        public DateOnly? BirthDayDate { get; set; }*/

        public virtual ICollection<ActorsInFilm> ActorsInFilms { get; set; }
    }
}
