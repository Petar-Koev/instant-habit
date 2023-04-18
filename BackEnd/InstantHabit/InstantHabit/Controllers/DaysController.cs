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
        public async Task<List<Day>> GetAllHabitDays([FromQuery] int habitId)
        {
            try
            {
                var result = _daysService.GetDaysFromDB(habitId);
                return result;
            } catch (Exception)
            {
                return new List<Day>();
            }

        }

        [HttpPost]
        [Route("AddDay")]
        [ProducesResponseType(201)]
        public async Task<AddDayResponse> AddDay([FromBody] AddDayRequest request)
        {
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
                _daysService.DeleteSelectedDay(request);
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
                _daysService.AddDailyDescription(request);
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
            try
            {
                return _daysService.GetDayFromDB(habitId, dayNumber);
            }
            catch (Exception)
            {
                return new Day();
            }
            
        }

        [HttpGet]
        [Route("GetBestStreak")]
        public async Task<BestStreakResponse> GetBestStreak([FromQuery]  int habitId)
        {
            try
            {
                var result = _daysService.GetStreakMessage(habitId);
                return result;
            }
            catch (Exception ex)
            {
                return new BestStreakResponse(succeeded:false,error: ex.Message);
            }
        }
        
        [HttpGet]
        [Route("ResetChecker")]
        public async Task<string> ResetChecker([FromQuery] int dayNumber, int habitId)
        {
            try
            {
                var confirmation = _daysService.DaysListResetChecker(dayNumber, habitId);
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
