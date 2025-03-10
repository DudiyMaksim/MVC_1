﻿using System.ComponentModel.DataAnnotations;

namespace MVC_1.Models
{
    public class Category
    {
        [Key]
        public string? Id { get; set; }
        public string? Name { get; set; }

        public List<Product> Products { get; set; } = [];
    }
}
