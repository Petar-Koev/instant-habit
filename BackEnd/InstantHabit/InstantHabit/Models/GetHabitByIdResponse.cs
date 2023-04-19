namespace InstantHabit.Models
{
    public class GetHabitByIdResponse : GlobalResponse
    {
        public Habit Habit { get; set; }

        public GetHabitByIdResponse(Habit habits, bool succeeded, string error) : base(succeeded, error)
        {
            Habit = habits;
        }

        public GetHabitByIdResponse(bool succeeded, string error) : base(succeeded, error)
        {

        }
    }
}
