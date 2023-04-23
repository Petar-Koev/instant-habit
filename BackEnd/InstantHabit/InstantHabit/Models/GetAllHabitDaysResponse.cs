namespace InstantHabit.Models
{
    public class GetAllHabitDaysResponse : GlobalResponse
    {
        public List<Day> Days { get; set; } 
        public GetAllHabitDaysResponse(List<Day>days, bool succeeded, string error) : base(succeeded,error)
        {
            Days = days;
        }

        public GetAllHabitDaysResponse(bool succeeded, string error) : base(succeeded, error)
        {

        }
    }
}
