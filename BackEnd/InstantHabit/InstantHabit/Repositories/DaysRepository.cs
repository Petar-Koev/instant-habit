using InstantHabit.Interfaces;
using InstantHabit.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InstantHabit.Repositories
{
    public class DaysRepository : IDaysRepository
    {
        private readonly InstantHabitContext _context;
        public DaysRepository(InstantHabitContext context)
        {
            _context = context;
        }
        public List<Day> GetAllDays(int habitId)
        {
            var days = _context.Days.ToList<Day>();
            var result = (from day in days
                          where day.HabitId == habitId
                          select day).ToList();
            return result;
        }
        public Day GetDay(int habitId, int num)
        {
            var days = _context.Days.ToList<Day>();

            var linqResult = (from day in days
                              where day.HabitId == habitId && day.DayNumber == num
                              select day).FirstOrDefault();

            return linqResult;
        }
        public void DeleteDays(int id)
        {
            _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.DeleteDays_StoredProcedure {0}", id);
        }
        public void AddDailyDescription(int id, int num, string description)
        {
            _context.Database.ExecuteSqlRaw
                ("EXECUTE InstantHabit.AddDayDescription_StoredProcedure {0}, {1}, {2}", id, num, description);
        }
        public void DeleteSelectedDay(int id, int num)
        {
            _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.DeleteDay_StoredProcedure {0}, {1}", id, num);
        }
        public void AddNewDay(int id, int num)
        {
            _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.AddNewDay_StoredProcedure {0}, {1}", id, num);
        }
    }
}
