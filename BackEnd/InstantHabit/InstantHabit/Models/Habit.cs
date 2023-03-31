using System.ComponentModel;
using System;
using Microsoft.EntityFrameworkCore;

namespace InstantHabit.Models
{
    public class Habit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
        public bool IsExtended { get; set; }

        public virtual ICollection<Day> Day { get; set; }

    }


    

}
