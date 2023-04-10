using InstantHabit.Controllers;
using InstantHabit.Interfaces;
using InstantHabit.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InstantHabit.Services
{
    public class HabitsServices : IHabitsService
    {

        private readonly IHabitsRepository _habitsRepository;
        public HabitsServices(IHabitsRepository habitsRepository)
        {
            _habitsRepository = habitsRepository;
        }

        public void SetIsExtended(ExtendHabitRequest request)
        {
            _habitsRepository.InsertHabitExtension(request.HabitId);
        }

        public void AddHabitDescription(AddDescriptionRequest request)
        {
            _habitsRepository.InsertDescription(request.HabitId, request.Description);
        }

        public void DeleteHabit(DeleteAhabitRequest request)
        {
            _habitsRepository.DeleteAhabit(request.Id);
        }

        public void CreateNewHabit(AddHabitRequest request)
        {
            _habitsRepository.InsertHabit(request.Name);
        }

        public List<Habit> GetHabitsFromDB()
        {
            return _habitsRepository.GetHabits();
        }

        public Habit GetHabitFromDB(int habitId)
        {
            return _habitsRepository.GetHabit(habitId);
        }

        // Checks for DB habit match
        public string MatchChecker(string name)
        {
            var habits = _habitsRepository.GetHabits();

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
