using InstantHabit.Models;

namespace InstantHabit.Interfaces
{
    public interface IHabitsService
    {
        public Habit GetHabitFromDB(int habitId);
        public string MatchChecker(string name);
        public void SetIsExtended(ExtendHabitRequest request);
        public void AddHabitDescription(AddDescriptionRequest request);
        public void DeleteHabit(DeleteAhabitRequest request);
        public List<Habit> GetHabitsFromDB();
        public void CreateNewHabit(AddHabitRequest request);
    }
}
