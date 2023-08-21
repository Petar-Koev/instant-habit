using InstantHabit.Models;

namespace InstantHabit.Interfaces
{
    public interface IDaysService
    {
        public Task<List<Day>> GetDaysFromDB(int habitId);
        public Task<Day> GetDayFromDB(int habitId, int num);
        public Task<string> DaysListResetChecker(int num, int habitId);
        public Task<string> MatchChecker(int id, int dayNumber);
        public Task<List<int>> CalculateBestStreak(int habitId);
        public Task DeleteDays(DeleteHabitDaysRequest request);
        public Task AddNewDay(AddDayRequest request);
        public Task DeleteSelectedDay(DeleteDayRequest request);
        public Task AddDailyDescription(AddDayDescriptionRequest request);
        public Task<BestStreakResponse> GetStreakMessage(int habitId);
    }
}
