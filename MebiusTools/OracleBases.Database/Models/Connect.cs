using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tasks.Database.Models
{
    public class Connect
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Host { get; set; }

        [Required]
        public int Port { get; set; }

        [Required]
        public string ServiceName { get; set; }
    }
}
