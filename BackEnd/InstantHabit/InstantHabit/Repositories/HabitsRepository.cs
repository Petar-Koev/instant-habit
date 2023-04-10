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

        public void InsertHabit(string name)
        {
            _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.CreateNewHabit_StoredProcedure {0}", name);
        }
       public void DeleteAhabit(int id)
        {
            _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.DeleteAhabit_StoredProcedure {0}", id);
        }
       public void InsertDescription(int id, string description)
        {
            _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.AddDescription_StoredProcedure {0}, {1}", id, description);
        }
        public  void InsertHabitExtension(int id)
        {
            _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.SetIsExtended_StoredProcedure {0}", id);
        }
        public List<Habit> GetHabits()
        {
            var habits = _context.Habits.ToList<Habit>();
            return habits;
        }
        public Habit GetHabit(int id)
        {
            var habitsList = _context.Habits.ToList<Habit>();

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
