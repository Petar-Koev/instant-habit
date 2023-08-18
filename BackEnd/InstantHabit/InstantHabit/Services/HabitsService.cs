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

        public async Task SetIsExtended(ExtendHabitRequest request)
        {
           await _habitsRepository.InsertHabitExtension(request.HabitId);
        }

        public async Task AddHabitDescription(AddDescriptionRequest request)
        {
           await _habitsRepository.InsertDescription(request.HabitId, request.Description);
        }

        public async Task DeleteHabit(DeleteAhabitRequest request)
        {
            await _habitsRepository.DeleteAhabit(request.Id);
        }

        public async Task CreateNewHabit(AddHabitRequest request)
        {
           await _habitsRepository.InsertHabit(request.Name);
        }

        public async Task<List<Habit>> GetHabitsFromDB()
        {
            return await _habitsRepository.GetHabits();
        }

        public async Task<Habit> GetHabitFromDB(int habitId)
        {
            return await _habitsRepository.GetHabit(habitId);
        }

        // Checks for DB habit match
        public async Task<string> MatchChecker(string name)
        {
            var habits = await _habitsRepository.GetHabits();

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
