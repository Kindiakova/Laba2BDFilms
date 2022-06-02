using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Laba2FilmsBD.Models
{
    public partial class ActorsInFilm
    {
        [Display(Name = "Актор(ка)")]
        public int ActorId { get; set; }

        [Display(Name = "Фільм")]
        //[Remote("NotExists", "Actors", AdditionalFields = "ActorId", ErrorMessage = "Запис вже існує")]
        public int FilmId { get; set; }

        [Display(Name = "Персонаж")]
       
        public string? Сharacter { get; set; }

        [Display(Name = "Головна роль")]
        public bool? IsMain { get; set; }

        [Display(Name = "Актор(ка)")]
        public virtual Actor Actor { get; set; } = null!;

        [Display(Name = "Фільм")]
        public virtual Film Film { get; set; } = null!;
    }
}
