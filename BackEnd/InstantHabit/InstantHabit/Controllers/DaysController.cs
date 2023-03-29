using InstantHabit.Models;
using InstantHabit.Services;
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
        public async Task<AddDayResponse> AddDay([FromBody] AddDayRequest request)
        {
            var checkForMatch = DaysServices.MatchChecker(request.HabitId, request.DayNumber, _context);

            if(checkForMatch == "No match")
            {
                try
                {
                    _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.AddNewDay_StoredProcedure {0}, {1}", request.HabitId, request.DayNumber);
                }
                catch (Exception ex)
                {

                    var response = new AddDayResponse(false, ex.Message);
                    return response;
                }
                return new AddDayResponse(true, null);
            } else if(checkForMatch == "Match")
            {
                return new AddDayResponse(false, checkForMatch);
            }
            return new AddDayResponse(false, "something went wrong");
            
        }

        [HttpDelete]
        [Route("DeleteDay")]
        [ProducesResponseType(201)]
        public async Task<DeleteDayResponse> DeleteDay([FromBody] DeleteDayRequest request)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.DeleteDay_StoredProcedure {0}, {1}", request.HabitId, request.DayNumber);
            }
            catch (Exception ex)
            {
                var response = new DeleteDayResponse(false, ex.Message);
                return response;
            }
            return new DeleteDayResponse(true, null);
        }

        [HttpPut]
        [Route("AddDayDescription")]
        [ProducesResponseType(201)]
        public async Task<AddDayDescriptionResponse> AddDescription([FromBody] AddDayDescriptionRequest request)
        {
            try
            {
                _context.Database.ExecuteSqlRaw
                ("EXECUTE InstantHabit.AddDayDescription_StoredProcedure {0}, {1}, {2}", request.HabitId, request.DayNumber, request.Description);

            }
            catch (Exception ex)
            {
                var responce = new AddDayDescriptionResponse(false, ex.Message);
                return responce;
            }
            return new AddDayDescriptionResponse(true, null);
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

            var confirmation = DaysServices.DaysListResetChecker(dayNumber, daysList, habitId);

            return confirmation ;
        }

        [HttpDelete]
        [Route("DeleteHabitDays")]
        [ProducesResponseType(201)]
        public async Task<DeleteHabitDaysResponse> DeleteHabitDays([FromBody] DeleteHabitDaysRequest request)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.DeleteDays_StoredProcedure {0}", request.HabitId);
            }
            catch (Exception ex)
            {
                var response = new DeleteHabitDaysResponse(false, ex.Message);
                return response;
            }
            return new DeleteHabitDaysResponse(true, null);
        }

    }
}
