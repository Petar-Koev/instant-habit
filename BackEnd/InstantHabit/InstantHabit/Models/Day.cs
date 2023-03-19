namespace InstantHabit.Models
{
    public class Day
    {
        public int Id { get; set; }
        public int DayNumber { get; set; }
        public string Note { get; set; } 
        public bool IsChecked { get; set; }
        public int HabitId { get; set; }
        public virtual Habit Habit { get; set; }

    }

}
