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
        public async Task<List<Day>> GetAllDays(int habitId)
        {
            var days = await _context.Days.ToListAsync<Day>();
            var result = (from day in days
                          where day.HabitId == habitId
                          select day).ToList();
            return result;
        }
        public async Task<Day> GetDay(int habitId, int num)
        {
            var days = await _context.Days.ToListAsync<Day>();

            var linqResult = (from day in days
                              where day.HabitId == habitId && day.DayNumber == num
                              select day).FirstOrDefault();

            return linqResult;
        }
        public async Task DeleteDays(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("EXECUTE InstantHabit.DeleteDays_StoredProcedure {0}", id);
        }
        public async Task AddDailyDescription(int id, int num, string description)
        {
            await _context.Database.ExecuteSqlRawAsync
                ("EXECUTE InstantHabit.AddDayDescription_StoredProcedure {0}, {1}, {2}", id, num, description);
        }
        public async Task DeleteSelectedDay(int id, int num)
        {
            await _context.Database.ExecuteSqlRawAsync("EXECUTE InstantHabit.DeleteDay_StoredProcedure {0}, {1}", id, num);
        }
        public async Task AddNewDay(int id, int num)
        {
            await _context.Database.ExecuteSqlRawAsync("EXECUTE InstantHabit.AddNewDay_StoredProcedure {0}, {1}", id, num);
        }
    }
}
