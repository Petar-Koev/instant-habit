using InstantHabit.Interfaces;
using InstantHabit.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InstantHabit.Services
{
    public class HabitsServices : IHabitsService
    {

        private readonly InstantHabitContext _context;
        public HabitsServices(InstantHabitContext context)
        {
            _context = context;
        }

        public void SetIsExtended(ExtendHabitRequest request)
        {
            _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.SetIsExtended_StoredProcedure {0}", request.HabitId);
        }

        public void AddHabitDescription(AddDescriptionRequest request)
        {
            _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.AddDescription_StoredProcedure {0}, {1}", request.HabitId, request.Description);
        }

        public void DeleteHabit(DeleteAhabitRequest request)
        {
            _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.DeleteAhabit_StoredProcedure {0}", request.Id);
        }

        public void CreateNewHabit(AddHabitRequest request)
        {
            _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.CreateNewHabit_StoredProcedure {0}", request.Name);
        }

        public List<Habit> GetHabitsFromDB()
        {
            var habits = _context.Habits.ToList<Habit>();
            return habits;
        }

        public Habit GetHabitFromDB(int habitId)
        {
            var habitsList = _context.Habits.ToList<Habit>();


            var linqResult = (from habit in habitsList
                              where habit.Id == habitId
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

        // Checks for DB habit match
        public string MatchChecker(string name)
        {
            var habits = _context.Habits.ToList<Habit>();

            var checker = "";

            if (habits.Count == 0)
            {
                checker = "No match";
            }
            else
            {
                for (int i = 0; i < habits.Count; i++)
                {

                    if (habits[i].Name != name)
                    {
                        checker = "No match";
                    }
                    else if (habits[i].Name == name)
                    {
                        checker = "Match";
                        break;
                    }
                }
            }
            return checker;
        }
    }
}
