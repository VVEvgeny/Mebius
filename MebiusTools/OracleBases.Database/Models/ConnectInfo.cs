using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tasks.Database.Models
{
    public class ConnectInfo
    {
        [Key]
        public int Id { get; set; }

        //Foreign key for Standard
        [Required]
        public string ConnectInfoId { get; set; }
        [Required]
        public virtual Connect Connect { get; set; }

        [Required]
        public DateTime? DateAdd { get; set; }

        [Required]
        public DateTime? LastUsed { get; set; }


        [Required]
        public string OperDay { get; set; }

        [Required]
        public string PrevDay { get; set; }

        [Required]
        public string Regions { get; set; }
    }
}
