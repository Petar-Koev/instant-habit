using InstantHabit.Models;

namespace InstantHabit.Interfaces
{
    public interface IHabitsRepository
    {
        public Task InsertHabit(string name);
        public Task <List<Habit>> GetHabits();
        public Task DeleteAhabit(int id);
        public Task InsertDescription(int id, string description);
        public Task<Habit> GetHabit(int id);
        public Task InsertHabitExtension(int id);
    }
}
