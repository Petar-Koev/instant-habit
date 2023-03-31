using InstantHabit.Controllers;
using InstantHabit.Models;
using System.Security.Cryptography;
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

        // Gets days for a specific habit
        public static List<Day> getDaysFromDB(InstantHabitContext _context, int habitId)
        {
            var days = _context.Days.ToList<Day>();
            var result = (from day in days
                          where day.HabitId == habitId
                          select day).ToList();
            return result;
        }

        // Gets a specific day
        public static Day getDayFromDB(InstantHabitContext _context, int habitId, int num)
        {
            var days = _context.Days.ToList<Day>();

            var linqResult = (from day in days
                              where day.HabitId == habitId && day.DayNumber == num
                              select day).FirstOrDefault();

            return linqResult;
        }


        // Checks for grid extensions / days reset.
        public static string DaysListResetChecker(int num, int habitId, InstantHabitContext _context)
        {
            var result = DaysServices.getDaysFromDB(_context, habitId);

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

        // Checks for DB day match
        public static string MatchChecker(int id, int dayNumber, InstantHabitContext _context)
        {
            var result = DaysServices.getDaysFromDB(_context, id);

            var checker = "";

            if (result.Count == 0)
            {
                checker = "No match";
            }
            else
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


        public static List<int> CalculateBestStreak(InstantHabitContext _context, int habitId)
        {
            var days = DaysServices.getDaysFromDB(_context, habitId);
            var numbers = new List<int>();

            foreach (var day in days) { numbers.Add(day.DayNumber); };
            numbers.Sort();

            var bestStreak = 1;
            var bestStreakList = new List<int>();

            if (numbers.Count <= 1) // BestStreak: 0/1
            {
                bestStreak = numbers.Count;
                bestStreakList.Add(bestStreak);
            }
            else
            {    
                // (-1) to ensure (i+1) will work.
                for (var i = 0; i < numbers.Count - 1; i++)
                {
                    // calculates single streak
                    if (numbers[i + 1] - numbers[i] == 1)
                    {
                        // adds streak if the loops is done
                        bestStreak++;
                        if (i + 1 == numbers.Count - 1)
                        {
                            bestStreakList.Add(bestStreak);
                        }
                    }
                    // adds BestStreak if broken and resets it
                    else
                    {
                        bestStreakList.Add(bestStreak);
                        bestStreak = 1;
                    }
                }
            }
            return bestStreakList;
        }

        public static BestStreakResponse GetStreakMessage(InstantHabitContext _context, int habitId)
        {
            var bestStreakInfo = DaysServices.CalculateBestStreak(_context, habitId);

            string msg;

            if (bestStreakInfo.Max() >= 3 && bestStreakInfo.Max() <= 8)
            {
                msg = "Well Done";
            }
            else if (bestStreakInfo.Max() >= 9 && bestStreakInfo.Max() <= 14)
            {
                msg = "Great Job";
            }
            else if (bestStreakInfo.Max() >= 15 && bestStreakInfo.Max() <= 24)
            {
                msg = "You're on FIRE!";
            }
            else if (bestStreakInfo.Max() >= 25 && bestStreakInfo.Max() <= 30)
            {
                msg = "Congratulations!!!";
            }
            else if (bestStreakInfo.Max() >= 31 && bestStreakInfo.Max() <= 41)
            {
                msg = "Hard Work pays off";
            }
            else if (bestStreakInfo.Max() >= 42 && bestStreakInfo.Max() <= 60)
            {
                msg = "TOP NOTCH DEVELOPMENT!";
            }
            else
            {
                msg = "Keep on going";
            }

            var result = new BestStreakResponse
            {
                BestStreak = bestStreakInfo.Max(),
                MotivationalMessage = msg
            };

            return result;
        }

    }
}


