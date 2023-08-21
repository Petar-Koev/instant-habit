using InstantHabit.Models;

namespace InstantHabit.Interfaces
{
    public interface IDaysRepository
    {
        public Task AddNewDay(int id, int num);
        public Task DeleteSelectedDay(int id, int num);
        public Task AddDailyDescription(int id, int num, string description);
        public Task DeleteDays(int id);
        public Task<Day> GetDay(int habitId, int num);
        public Task<List<Day>> GetAllDays(int habitId);

    }
}
