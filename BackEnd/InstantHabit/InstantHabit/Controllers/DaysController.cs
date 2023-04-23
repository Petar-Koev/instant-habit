using InstantHabit.Interfaces;
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
        private readonly IDaysService _daysService;
        public DaysController(IDaysService daysService)
        {
                _daysService = daysService;
        }

        [HttpGet]
        [Route("GetAllHabitDays")]
        public async Task<GetAllHabitDaysResponse> GetAllHabitDays([FromQuery] int habitId)
        {
            try
            {
                var result = _daysService.GetDaysFromDB(habitId);
                return new GetAllHabitDaysResponse(result, true, null);
            } 
            catch (Exception ex)
            {
                return new GetAllHabitDaysResponse(false, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddDay")]
        [ProducesResponseType(201)]
        public async Task<AddDayResponse> AddDay([FromBody] AddDayRequest request)
        {
            if (request == null)
            {
                return new AddDayResponse(false, "Request is null.");
            }
            var checkForMatch = _daysService.MatchChecker(request.HabitId, request.DayNumber);

            if(checkForMatch == "No match")
            {
                try
                {
                    _daysService.AddNewDay(request);
                    return new AddDayResponse(true, null);
                }
                catch (Exception ex)
                {
                    var response = new AddDayResponse(false, ex.Message);
                    return response;
                }
            } 
            return new AddDayResponse(false, checkForMatch);  
        }

        [HttpDelete]
        [Route("DeleteDay")]
        [ProducesResponseType(201)]
        public async Task<DeleteDayResponse> DeleteDay([FromBody] DeleteDayRequest request)
        {
            try
            {
                if (request == null)
                {
                    return new DeleteDayResponse(false, "Request is null.");
                }
                _daysService.DeleteSelectedDay(request);
                return new DeleteDayResponse(true, null);
            }
            catch (Exception ex)
            {
                var response = new DeleteDayResponse(false, ex.Message);
                return response;
            }
        }

        [HttpPut]
        [Route("AddDayDescription")]
        [ProducesResponseType(201)]
        public async Task<AddDayDescriptionResponse> AddDescription([FromBody] AddDayDescriptionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return new AddDayDescriptionResponse(false, "Request is null.");
                }
                _daysService.AddDailyDescription(request);
                return new AddDayDescriptionResponse(true, null);
            }
            catch (Exception ex)
            {
                var responce = new AddDayDescriptionResponse(false, ex.Message);
                return responce;
            }
        }

        [HttpGet]
        [Route("GetDayByNumber")]
        public async Task<GetDayByNumberResponse> GetDayByNumber([FromQuery] int dayNumber, int habitId)
        {
            try
            {
                var day = _daysService.GetDayFromDB(habitId, dayNumber);
                if (day == null)
                {
                    throw new Exception("Day does not exist");
                }
                return new GetDayByNumberResponse(day, true, null);
            }
            catch (Exception ex)
            {
                return new GetDayByNumberResponse(false, ex.Message);
            }
            
        }

        [HttpGet]
        [Route("GetBestStreak")]
        public async Task<BestStreakResponse> GetBestStreak([FromQuery]  int habitId)
        {
            try
            {
                var result = _daysService.GetStreakMessage(habitId);
                if (result == null)
                {
                    throw new Exception("Result is null");
                }
                return result;
            }
            catch (Exception ex)
            {
                return new BestStreakResponse(false, ex.Message);
            }
        }
        
        [HttpGet]
        [Route("ResetChecker")]
        public async Task<string> ResetChecker([FromQuery] int dayNumber, int habitId)
        {
            try
            {
                var confirmation = _daysService.DaysListResetChecker(dayNumber, habitId);
                if(confirmation == null)
                {
                    throw new Exception("Something went wrong");
                }
                return confirmation;
            }catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpDelete]
        [Route("DeleteHabitDays")]
        [ProducesResponseType(201)]
        public async Task<DeleteHabitDaysResponse> DeleteHabitDays([FromBody] DeleteHabitDaysRequest request)
        {
            
            try
            {
                if(request == null)
                {
                    return new DeleteHabitDaysResponse(false, "Request is null");
                }
                _daysService.DeleteDays(request);
                return new DeleteHabitDaysResponse(true, null);
            }
            catch (Exception ex)
            {
                var response = new DeleteHabitDaysResponse(false, ex.Message);
                return response;
            } 
        }
    }
}
