namespace InstantHabit.Models
{
    public class DeleteHabitDaysResponse : GlobalResponse
    {
        public DeleteHabitDaysResponse(bool succeeded, string error) : base(succeeded, error)
        {
        }
    }
}
