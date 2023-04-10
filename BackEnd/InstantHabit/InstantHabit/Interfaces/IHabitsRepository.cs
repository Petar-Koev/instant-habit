using InstantHabit.Models;

namespace InstantHabit.Interfaces
{
    public interface IHabitsRepository
    {
        public void InsertHabit(string name);
        public List<Habit> GetHabits();
        public void DeleteAhabit(int id);
        public void InsertDescription(int id, string description);
        public Habit GetHabit(int id);
        public void InsertHabitExtension(int id);
    }
}
