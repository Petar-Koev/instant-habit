using InstantHabit.Models;

namespace InstantHabit.Interfaces
{
    public interface IDaysRepository
    {
        public void AddNewDay(int id, int num);
        public void DeleteSelectedDay(int id, int num);
        public void AddDailyDescription(int id, int num, string description);
        public void DeleteDays(int id);
        public Day GetDay(int habitId, int num);
        public List<Day> GetAllDays(int habitId);

    }
}
