﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDCBank.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(50, ErrorMessage = "Username must be at most 50 characters.")]
        public string UserName { get; set; }

        
        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(100, ErrorMessage = "Password must be at most 100 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        

        // Navigation property
        public Customer Customer { get; set; }





    }
}
