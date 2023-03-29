using InstantHabit.Controllers;
using InstantHabit.Models;
using System.Text.RegularExpressions;

namespace InstantHabit.Services
{
    public class DaysServices
    {
        private readonly InstantHabitContext _context;
        public DaysServices(InstantHabitContext context)
        {
            _context = context;
        }

        public static string DaysListResetChecker(int num, List<Day> daysList, int habitId)
        {
            var result = (from day in daysList
                          where day.HabitId == habitId
                          select day).ToList();

            var response = "";

            if ((num >= 25 && num <= 30) && result.Count < 20)
            {
                response = "You failed.";

            }
            else if ((num >= 25 && num <= 30) && result.Count >= 20)
            {
                response = "You succeeded.";
            }

            return response;
        }
        
        public static string MatchChecker(int id, int dayNumber, InstantHabitContext _context)
        {
            var days = _context.Days.ToList<Day>();
            var result = (from day in days
                          where day.HabitId == id
                          select day).ToList();

            var checker = "";

            if(result.Count ==  0)
            {
                checker = "No match";
            } else
            {
                for (int i = 0; i < result.Count; i++)
                {

                    if (result[i].DayNumber != dayNumber)
                    {
                        checker = "No match";
                    }
                    else if (result[i].DayNumber == dayNumber)
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

