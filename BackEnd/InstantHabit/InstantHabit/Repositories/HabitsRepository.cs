using Azure.Core;
using InstantHabit.Interfaces;
using InstantHabit.Models;
using Microsoft.EntityFrameworkCore;

namespace InstantHabit.Repositories
{
    public class HabitsRepository : IHabitsRepository
    {
        private readonly InstantHabitContext _context;
        public HabitsRepository(InstantHabitContext context)
        {
            _context = context;
        }

        public async Task InsertHabit(string name)
        {
            await _context.Database.ExecuteSqlRawAsync("EXECUTE InstantHabit.CreateNewHabit_StoredProcedure {0}", name);
        }
       public async Task DeleteAhabit(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("EXECUTE InstantHabit.DeleteAhabit_StoredProcedure {0}", id);
        }
       public async Task InsertDescription(int id, string description)
        {
           await _context.Database.ExecuteSqlRawAsync("EXECUTE InstantHabit.AddDescription_StoredProcedure {0}, {1}", id, description);
        }
        public  async Task InsertHabitExtension(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("EXECUTE InstantHabit.SetIsExtended_StoredProcedure {0}", id);
        }
        public async Task<List<Habit>> GetHabits()
        {
            var habits = await _context.Habits.ToListAsync<Habit>();
            return habits;
        }
        public async Task<Habit> GetHabit(int id)
        {
            var habitsList = await _context.Habits.ToListAsync<Habit>();

            var linqResult = (from habit in habitsList
                              where habit.Id == id
                              select new Habit
                              {
                                  Id = habit.Id,
                                  Name = habit.Name,
                                  Description = habit.Description,
                                  CreationDate = habit.CreationDate,
                                  IsExtended = habit.IsExtended
                              }).FirstOrDefault();
            return linqResult;
        }
    }
}
