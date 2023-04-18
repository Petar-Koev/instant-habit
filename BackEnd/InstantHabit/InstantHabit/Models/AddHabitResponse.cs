namespace InstantHabit.Models
{
    public class AddHabitResponse : GlobalResponse
    {
        public AddHabitResponse(bool succeeded, string error) : base(succeeded, error)
        {
        }
    }
}
