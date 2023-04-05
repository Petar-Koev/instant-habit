using InstantHabit.Controllers;
using InstantHabit.Interfaces;
using InstantHabit.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace InstantHabit.Services
{
    public class DaysServices : IDaysService
    {
        private readonly InstantHabitContext _context;
        public DaysServices(InstantHabitContext context)
        {
            _context = context;
        }

        // Gets days for a specific habit
        public List<Day> GetDaysFromDB(int habitId)
        {
            var days = _context.Days.ToList<Day>();
            var result = (from day in days
                          where day.HabitId == habitId
                          select day).ToList();
            return result;
        }

        // Gets a specific day
        public Day GetDayFromDB( int habitId, int num)
        {
            var days = _context.Days.ToList<Day>();

            var linqResult = (from day in days
                              where day.HabitId == habitId && day.DayNumber == num
                              select day).FirstOrDefault();

            return linqResult;
        }


        // Checks for grid extensions / days reset.
        public string DaysListResetChecker(int num, int habitId )
        {
            var result = GetDaysFromDB(habitId);

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
        public string MatchChecker(int id, int dayNumber)
        {
            var result = GetDaysFromDB(id);

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


        public List<int> CalculateBestStreak(int habitId)
        {
            var days = GetDaysFromDB(habitId);
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

        private string CalculateMessage(int bestStreak)
        {
            string msg;

            if (bestStreak >= 3 && bestStreak <= 8)
            {
                msg = "Well Done";
            }
            else if (bestStreak >= 9 && bestStreak <= 14)
            {
                msg = "Great Job";
            }
            else if (bestStreak >= 15 && bestStreak <= 24)
            {
                msg = "You're on FIRE!";
            }
            else if (bestStreak >= 25 && bestStreak <= 30)
            {
                msg = "Congratulations!!!";
            }
            else if (bestStreak >= 31 && bestStreak <= 41)
            {
                msg = "Hard Work pays off";
            }
            else if (bestStreak >= 42 && bestStreak <= 60)
            {
                msg = "TOP NOTCH DEVELOPMENT!";
            }
            else
            {
                msg = "Keep on going";
            }
            return msg;
        }

        public void DeleteDays(DeleteHabitDaysRequest request)
        {
            _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.DeleteDays_StoredProcedure {0}", request.HabitId);
        }

        public void AddDailyDescription(AddDayDescriptionRequest request)
        {
            _context.Database.ExecuteSqlRaw
                ("EXECUTE InstantHabit.AddDayDescription_StoredProcedure {0}, {1}, {2}", request.HabitId, request.DayNumber, request.Description);
        }

        public void DeleteSelectedDay(DeleteDayRequest request)
        {
            _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.DeleteDay_StoredProcedure {0}, {1}", request.HabitId, request.DayNumber);
        }

        public void AddNewDay(AddDayRequest request)
        {
            _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.AddNewDay_StoredProcedure {0}, {1}", request.HabitId, request.DayNumber);
        }

        public BestStreakResponse GetStreakMessage(int habitId)
        {
            var bestStreakInfo = CalculateBestStreak(habitId);
            var msg = CalculateMessage(bestStreakInfo.Max());
           
            var result = new BestStreakResponse
            {
                BestStreak = bestStreakInfo.Max(),
                MotivationalMessage = msg
            };

            return result;
        }
    }
}


