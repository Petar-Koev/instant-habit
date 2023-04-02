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
        private readonly InstantHabitContext _context;
        public HabitsController(InstantHabitContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("AddHabit")]
        [ProducesResponseType(201)]
        public async Task<AddHabitResponse> AddHabit([FromBody] AddHabitRequest request)
        {
            var matchChecker = HabitsService.MatchChecker(request.Name, _context);

            if(matchChecker == "No match")
            {
                try
                {
                    _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.CreateNewHabit_StoredProcedure {0}", request.Name);
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
            var habits = _context.Habits.ToList<Habit>();
            return habits;
        }

        [HttpDelete]
        [Route("DeleteAhabit")]
        [ProducesResponseType(201)]
        public async Task<DeleteAhabitResponse> DeleteAhabit([FromBody] DeleteAhabitRequest request)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.DeleteAhabit_StoredProcedure {0}", request.Id);
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
                _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.AddDescription_StoredProcedure {0}, {1}", request.HabitId, request.Description);
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

            return HabitsService.getHabitFromDB(_context, id);
                      
        }

        [HttpPut]
        [Route("ExtendHabit")]
        [ProducesResponseType(201)]
        public async Task<ExtendHabitResponse> ExtendHabit([FromBody] ExtendHabitRequest request)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.SetIsExtended_StoredProcedure {0}", request.HabitId);
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
