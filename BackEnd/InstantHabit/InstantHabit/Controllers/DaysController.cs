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

    }
}
