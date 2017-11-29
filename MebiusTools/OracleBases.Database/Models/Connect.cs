using System;
using System.ComponentModel.DataAnnotations;

namespace OracleBases.Database.Models
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

        [Required]
        public DateTime DateAdd { get; set; }

        public DateTime LastUsed { get; set; }

        public string OperDay { get; set; }

        public string PrevDay { get; set; }

        public string Regions { get; set; }
    }
}
