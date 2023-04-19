namespace InstantHabit.Models
{
    public class GetAllHabitsResponse : GlobalResponse
    {
        public List<Habit> Habits { get; set; }

        public GetAllHabitsResponse(List<Habit> habits, bool succeeded, string error) : base(succeeded, error)
        {
            Habits = habits;
        }

        public GetAllHabitsResponse(bool succeeded, string error) : base(succeeded, error)
        {

        }

    }
}
