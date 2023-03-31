namespace InstantHabit.Models
{
    public class AddDayDescriptionRequest
    {
        public int HabitId { get; set; }
        public int DayNumber { get; set; }
        public string Description { get; set; }
    }
}
