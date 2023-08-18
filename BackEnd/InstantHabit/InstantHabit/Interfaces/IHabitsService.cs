using InstantHabit.Models;

namespace InstantHabit.Interfaces
{
    public interface IHabitsService
    {
        public Task<Habit> GetHabitFromDB(int habitId);
        public Task<string> MatchChecker(string name);
        public Task SetIsExtended(ExtendHabitRequest request);
        public Task AddHabitDescription(AddDescriptionRequest request);
        public Task DeleteHabit(DeleteAhabitRequest request);
        public Task<List<Habit>> GetHabitsFromDB();
        public Task CreateNewHabit(AddHabitRequest request);
    }
}
