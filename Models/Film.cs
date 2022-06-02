using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Laba2FilmsBD.Models
{
    public partial class Film
    {
        public Film()
        {
            ActorsInFilms = new HashSet<ActorsInFilm>();
            Purchases = new HashSet<Purchase>();
        }
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не має бути порожнім")]
        [Display(Name = "Назва")]
        [Remote("NameNotExists", "Films", AdditionalFields = "Title", ErrorMessage = "Фільм з такою назвою вже існує")]
        public string Title { get; set; } = null!;

        [Display(Name = "Опис")]
        public string? Description { get; set; }

        [Display(Name = "Рік виходу")]
        [Range(1895, 2022, ErrorMessage = "Рік виходу має бути в межах від 1895 до 2022")]
        public int? ReleaseYear { get; set; }

        [Display(Name = "Мова")]
        public string? Language { get; set; }

        [Display(Name = "Тривалість")]
        [Range(0, 100000009, ErrorMessage = "Тривалість має бути >= 0")]
        public int? LenghtInMinutes { get; set; }

        [Display(Name = "Жанр")]
        public string? Genre { get; set; }

        public virtual ICollection<ActorsInFilm> ActorsInFilms { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
