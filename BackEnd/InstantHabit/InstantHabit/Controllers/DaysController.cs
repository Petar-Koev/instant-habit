using InstantHabit.Models;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace InstantHabit.Controllers
{
        [ApiController]
        [Route("[controller]")]
        public class DaysController : ControllerBase
        {
            private readonly InstantHabitContext _context;
            public DaysController(InstantHabitContext context)
            {
                _context = context;
            }

        [HttpGet]
        [Route("GetAllHabitDays")]
        public async Task<List<Day>> GetAllHabitDays([FromQuery] int habitId)
        {
            var days = _context.Days.ToList<Day>();

            var result = (from day in days
                         where day.HabitId == habitId
                         select day).ToList();

            return result;
        }

        [HttpPost]
        [Route("AddDay")]
        [ProducesResponseType(201)]
        public async Task<StatusCodeResult> AddDay([FromQuery] int habitId, int dayNumber)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.AddNewDay_StoredProcedure {0}, {1}", habitId, dayNumber);
            }
            catch (Exception e)
            {
                return StatusCode(409);
            }
            return StatusCode(201);
        }

        [HttpDelete]
        [Route("DeleteDay")]
        [ProducesResponseType(201)]
        public async Task<StatusCodeResult> DeleteDay([FromQuery] int habitId, int dayNumber)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.DeleteDay_StoredProcedure {0}, {1}", habitId, dayNumber);
            }
            catch (Exception e)
            {
                return StatusCode(409);
            }
            return StatusCode(201);
        }

        [HttpPut]
        [Route("AddDayDescription")]
        [ProducesResponseType(201)]
        public async Task<StatusCodeResult> AddDescription([FromQuery] int habitId,int dayNumber, string description)
        {
            try
            {
                _context.Database.ExecuteSqlRaw
                ("EXECUTE InstantHabit.AddDayDescription_StoredProcedure {0}, {1}, {2}", habitId, dayNumber, description);

            }
            catch (Exception e)
            {
                return StatusCode(409);
            }
            return StatusCode(201);
        }

        [HttpGet]
        [Route("GetDayByNumber")]
        public async Task<Day> GetDayByNumber([FromQuery] int dayNumber, int habitId)
        {
            var daysList = _context.Days.ToList<Day>();

            var linqResult = (from day in daysList
                              where day.HabitId == habitId && day.DayNumber == dayNumber
                              select new Day
                              {
                                  Id = day.Id,
                                  DayNumber = day.DayNumber,
                                  Note = day.Note,
                                  IsChecked = day.IsChecked,
                                  HabitId = day.HabitId,

                              }).FirstOrDefault();
            return linqResult;

        }

        [HttpGet]
        [Route("GetBestStreak")]
        public async Task<BestStreakResponse> GetBestStreak([FromQuery]  int habitId)
        {
            var daysList = _context.Days.ToList<Day>();

            var linqResult = (from day in daysList
                              where day.HabitId == habitId 
                              select new Day
                              {
                                  Id = day.Id,
                                  DayNumber = day.DayNumber,
                                  Note = day.Note,
                                  IsChecked = day.IsChecked,
                                  HabitId = day.HabitId,

                              }).ToList();

            var numbers = new List<int>();

            foreach(var day in linqResult)
            {
                numbers.Add(day.DayNumber);
            }

            numbers.Sort();

            var bestStreak = 1;
            var bestStreakList = new List<int>();

            if(numbers.Count <= 1)
            {
                bestStreak = numbers.Count;
                bestStreakList.Add(bestStreak);
            } else
            {
                for (var i = 0; i < numbers.Count - 1; i++)
                {
                    if (numbers[i + 1] - numbers[i] == 1)
                    {
                        bestStreak++;
                        if (i + 1 == numbers.Count - 1)
                        {
                            bestStreakList.Add(bestStreak);
                        }
                    }
                    else
                    {
                        bestStreakList.Add(bestStreak);
                        bestStreak = 1;
                    }
                }
            }

            var msg = "";

           if (bestStreakList.Max() >= 3 && bestStreakList.Max() <= 8)
            {
                msg = "Well Done";
            }else if (bestStreakList.Max() >= 9 && bestStreakList.Max() <= 14)
            {
                msg = "Great Job";
            }else if (bestStreakList.Max() >= 15 && bestStreakList.Max() <= 24)
            {
                msg = "You're on FIRE!";
            }
            else if (bestStreakList.Max() >= 25 && bestStreakList.Max() <= 30)
            {
                msg = "Congratulations!!!";
            }
            else if (bestStreakList.Max() >= 31 && bestStreakList.Max() <= 41)
            {
                msg = "Hard Work pays off";
            }
            else if (bestStreakList.Max() >= 42 && bestStreakList.Max() <= 60)
            {
                msg = "TOP NOTCH DEVELOPMENT!";
            }
            else
            {
                msg = "Keep on going";
            }

            var result = new BestStreakResponse
            {
                BestStreak = bestStreakList.Max(),
                MotivationalMessage = msg
            };

            return result;

        }

        
        [HttpGet]
        [Route("ResetChecker")]
        public async Task<string> ResetChecker([FromQuery] int dayNumber, int habitId)
        {
            var daysList = _context.Days.ToList<Day>();
            string msg = "";

            var result = (from day in daysList
                          where day.HabitId == habitId
                          select day).ToList();

            if ((dayNumber >= 25 && dayNumber <= 30) && result.Count < 20)
            {
                msg = "You failed.";

            } else if ((dayNumber >= 25 && dayNumber <= 30) && result.Count >= 20)
            {
                msg = "You succeeded.";
            }

            return msg;

        }

        [HttpDelete]
        [Route("DeleteHabitDays")]
        [ProducesResponseType(201)]
        public async Task<StatusCodeResult> DeleteHabitDays([FromQuery] int habitId)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.DeleteDays_StoredProcedure {0}", habitId);
            }
            catch (Exception e)
            {
                return StatusCode(409);
            }
            return StatusCode(201);
        }


    }
}
