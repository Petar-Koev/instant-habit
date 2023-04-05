using InstantHabit.Models;

namespace InstantHabit.Interfaces
{
    public interface IDaysService
    {
        public List<Day> GetDaysFromDB(int habitId);
        public Day GetDayFromDB(int habitId, int num);
        public string DaysListResetChecker(int num, int habitId);
        public string MatchChecker(int id, int dayNumber);
        public List<int> CalculateBestStreak(int habitId);
        public void DeleteDays(DeleteHabitDaysRequest request);
        public void AddNewDay(AddDayRequest request);
        public void DeleteSelectedDay(DeleteDayRequest request);
        public void AddDailyDescription(AddDayDescriptionRequest request);
        public BestStreakResponse GetStreakMessage(int habitId);
    }
}
