namespace InstantHabit.Models
{
    public class GetHabitByIdResponse : GlobalResponse
    {
        public Habit Habit { get; set; }

        public GetHabitByIdResponse(Habit habit, bool succeeded, string error) : base(succeeded, error)
        {
            Habit = habit;
        }

        public GetHabitByIdResponse(bool succeeded, string error) : base(succeeded, error)
        {

        }
    }
}
