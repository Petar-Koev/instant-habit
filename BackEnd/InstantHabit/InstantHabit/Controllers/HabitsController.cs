using InstantHabit.Models;
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
        public async Task<StatusCodeResult> AddHabit([FromQuery] string name)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.CreateNewHabit_StoredProcedure {0}", name);
            } catch(Exception e)
            {
                return StatusCode(409);
            }
            return StatusCode(201);
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
        public async Task<StatusCodeResult> DeleteAhabit([FromQuery] int id)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.DeleteAhabit_StoredProcedure {0}", id);
            }
            catch (Exception e)
            {
                return StatusCode(409);
            }
            return StatusCode(201);
        }

        [HttpPut]
        [Route("AddDescription")]
        [ProducesResponseType(201)]
        public async Task<AddDescriptionResponse> AddDescription([FromBody] AddDescriptionRequest request)
        {
            try
            {
                //throw new Exception("Neshto losho stana");
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
            var habitsList = _context.Habits.ToList<Habit>();

            var linqResult = (from habit in habitsList
                              where habit.Id == id
                              select new Habit 
                              { 
                                  Id = habit.Id,
                                  Name = habit.Name,
                                  Description = habit.Description,
                                  CreationDate=habit.CreationDate,
                                  IsExtended=habit.IsExtended

                              }).FirstOrDefault();
            return linqResult;
                      
        }

        [HttpPut]
        [Route("ExtendHabit")]
        [ProducesResponseType(201)]
        public async Task<StatusCodeResult> ExtendHabit([FromQuery] int habitId)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("EXECUTE InstantHabit.SetIsExtended_StoredProcedure {0}", habitId);
            }
            catch (Exception e)
            {
                return StatusCode(409);
            }
            return StatusCode(201);
        }

    }
}
