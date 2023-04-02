using InstantHabit.Models;
using System.Linq;

namespace InstantHabit.Services
{
    public class HabitsService
    {
        private readonly InstantHabitContext _context;
        public HabitsService(InstantHabitContext context)
        {
            _context = context;
        }

        public static Habit getHabitFromDB(InstantHabitContext _context, int habitId)
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
        public static string MatchChecker(string name, InstantHabitContext _context)
        {
            var habits = _context.Habits.ToList<Habit>();

            var checker = "";

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
            return checker;
        }
    }
}
