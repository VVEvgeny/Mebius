using System;

namespace Tasks
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Repeat { get; set; }

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

        public override string ToString()
        {
            //return Name + " "+ Date + " " + (RepeatModes) Enum.Parse(typeof(RepeatModes), Repeat.ToString());
            return Name + " " + Date + " " + (RepeatModes)Repeat;
        }
    }
}
