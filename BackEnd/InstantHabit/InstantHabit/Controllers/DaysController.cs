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
            var result = DaysServices.getDaysFromDB(_context,habitId);

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
            return DaysServices.getDayFromDB(_context,habitId, dayNumber);
        }

        [HttpGet]
        [Route("GetBestStreak")]
        public async Task<BestStreakResponse> GetBestStreak([FromQuery]  int habitId)
        {
            var result = DaysServices.GetStreakMessage(_context,habitId);

            return result;
           
        }
        
        [HttpGet]
        [Route("ResetChecker")]
        public async Task<string> ResetChecker([FromQuery] int dayNumber, int habitId)
        {
            var confirmation = DaysServices.DaysListResetChecker(dayNumber, habitId, _context);

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
