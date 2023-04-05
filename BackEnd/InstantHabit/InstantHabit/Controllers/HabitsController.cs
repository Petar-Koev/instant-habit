using InstantHabit.Interfaces;
using InstantHabit.Models;
using InstantHabit.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace InstantHabit.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HabitsController : ControllerBase
    {
        private readonly IHabitsService _habitsService;
        public HabitsController(IHabitsService habitsService)
        {
            _habitsService = habitsService;
        }

        [HttpPost]
        [Route("AddHabit")]
        [ProducesResponseType(201)]
        public async Task<AddHabitResponse> AddHabit([FromBody] AddHabitRequest request)
        {
            var matchChecker = _habitsService.MatchChecker(request.Name);

            if(matchChecker == "No match")
            {
                try
                {
                    _habitsService.CreateNewHabit(request);
                }
                catch (Exception ex)
                {
                    var response = new AddHabitResponse(false, ex.Message);
                    return response;
                }
                return new AddHabitResponse(true, null);
            } else if(matchChecker == "Match")
            {
                return new AddHabitResponse(false, matchChecker);
            } 
            
           return new AddHabitResponse(false, "Something went wrong");
            
        }

        [HttpGet]
        [Route("GetAllHabits")]
        public  async Task<List<Habit>> GetAllHabits()
        {
            var habits = _habitsService.GetHabitsFromDB();
            return habits;
        }

        [HttpDelete]
        [Route("DeleteAhabit")]
        [ProducesResponseType(201)]
        public async Task<DeleteAhabitResponse> DeleteAhabit([FromBody] DeleteAhabitRequest request)
        {
            try
            {
                _habitsService.DeleteHabit(request);
            }
            catch (Exception ex)
            {
                var response = new DeleteAhabitResponse(false, ex.Message);
                return response;
            }
            return new DeleteAhabitResponse(true, null);
        }

        [HttpPut]
        [Route("AddDescription")]
        [ProducesResponseType(201)]
        public async Task<AddDescriptionResponse> AddDescription([FromBody] AddDescriptionRequest request)
        {
            try
            {
                _habitsService.AddHabitDescription(request);
            }
                catch (Exception ex)
            {
                var response = new AddDescriptionResponse(false, ex.Message);

                return response;
            }
            return new AddDescriptionResponse(true, null);
        }

        [HttpGet]
        [Route("GetHabitById")]
        public async Task<Habit> GetHabitById([FromQuery] int id)
        {

            return _habitsService.GetHabitFromDB(id);
                      
        }

        [HttpPut]
        [Route("ExtendHabit")]
        [ProducesResponseType(201)]
        public async Task<ExtendHabitResponse> ExtendHabit([FromBody] ExtendHabitRequest request)
        {
            try
            {
                _habitsService.SetIsExtended(request);
            }
            catch (Exception ex)
            {
                var response = new ExtendHabitResponse(false, ex.Message);  

                return response;
            }
            return new ExtendHabitResponse(true, null);
        }

    }
}
