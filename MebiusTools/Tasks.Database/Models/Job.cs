using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tasks.Database.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Task { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Repeat { get; set; }

        public string StopResult { get; set; }
        public string ErrorResult { get; set; }

        public string Param { get; set; }

        public enum RepeatModes
        {
            Once = 0,
            EveryHalfMinute, //0.5m=30s
            EveryMinute,     //1m
            EveryHalfHour,   //0.5h=30m
            EveryHour,       //1h
            EveryHalfDay,    //0.5d=12h
            EveryDay,        //1d
            EveryThirdDay,   //0.33d=8h
            EveryFourthDay   //0.25d=6h
        }

        public static IEnumerable<string> GetRepeatseEnumerable => Enum.GetNames(typeof(RepeatModes));

        public override string ToString()
        {
            return "Id=" + Id + ";"
                   + "Name=" + Name + ";"
                   + "Task=" + Task + ";"
                   + "Date=" + Date + ";"
                   + "Repeat=" + (RepeatModes) Repeat + ";"
                   + "StopResult=" + StopResult + ";"
                   + "ErrorResult=" + ErrorResult + ";"
                   + "Param=" + Param + ";"
                ;
        }
    }
}
