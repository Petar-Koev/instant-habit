using InstantHabit.Interfaces;
using InstantHabit.Models;
using InstantHabit.Repositories;
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
        public async Task<AddHabitResponse> AddHabit([FromBody] AddHabitRequest request)
        {
            var matchChecker = _habitsService.MatchChecker(request.Name);

            if(matchChecker == "No match")
            {
                try
                {
                    _habitsService.CreateNewHabit(request);
                    return new AddHabitResponse(true, null);
                }
                catch (Exception ex)
                {
                    var response = new AddHabitResponse(false, ex.Message);
                    return response;
                }
            } 
            return new AddHabitResponse(false, matchChecker);
        }

        [HttpGet]
        [Route("GetAllHabits")]
        public  async Task<List<Habit>> GetAllHabits()
        {
            try
            {
                var habits = _habitsService.GetHabitsFromDB();
                return habits;
            }
            catch (Exception)
            {
                return new List<Habit>();
            }
        }

        [HttpDelete]
        [Route("DeleteAhabit")]
        [ProducesResponseType(201)]
        public async Task<DeleteAhabitResponse> DeleteAhabit([FromBody] DeleteAhabitRequest request)
        {
            try
            {
                _habitsService.DeleteHabit(request);
                return new DeleteAhabitResponse(true, null);
            }
            catch (Exception ex)
            {
                var response = new DeleteAhabitResponse(false, ex.Message);
                return response;
            }
        }

        [HttpPut]
        [Route("AddDescription")]
        [ProducesResponseType(201)]
        public async Task<AddDescriptionResponse> AddDescription([FromBody] AddDescriptionRequest request)
        {
            try
            {
                _habitsService.AddHabitDescription(request);
                return new AddDescriptionResponse(true, null);
            }
                catch (Exception ex)
            {
                var response = new AddDescriptionResponse(false, ex.Message);
                return response;
            }
        }

        [HttpGet]
        [Route("GetHabitById")]
        public async Task<Habit> GetHabitById([FromQuery] int id)
        {
            try
            {
                return _habitsService.GetHabitFromDB(id);
            }
            catch (Exception)
            {
                return new Habit();
            }
        }

        [HttpPut]
        [Route("ExtendHabit")]
        [ProducesResponseType(201)]
        public async Task<ExtendHabitResponse> ExtendHabit([FromBody] ExtendHabitRequest request)
        {
            try
            {
                if(request == null)
                {
                    return new ExtendHabitResponse(false, "Request is null");
                }
                _habitsService.SetIsExtended(request);
                return new ExtendHabitResponse(true, null);
            }
            catch (Exception ex)
            {
                var response = new ExtendHabitResponse(false, ex.Message);  

                return response;
            }
            
        }


        

    }
}
