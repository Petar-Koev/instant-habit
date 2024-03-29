﻿using InstantHabit.Interfaces;
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
            try
            {
                if (request == null)
                {
                    return new AddHabitResponse(false, "Request is null.");
                }
                var matchChecker = await _habitsService.MatchChecker(request.Name);

                if (matchChecker == "No match")
                {
                    await _habitsService.CreateNewHabit(request);
                    return new AddHabitResponse(true, null);
                }
                return new AddHabitResponse(false, matchChecker);
            }
            catch (Exception ex)
            {
                var response = new AddHabitResponse(false, ex.Message);
                return response;
            }
        }

        [HttpGet]
        [Route("GetAllHabits")]
        public async Task<GetAllHabitsResponse> GetAllHabits()
        {
            try
            {
                var habits = await _habitsService.GetHabitsFromDB();
                return new GetAllHabitsResponse(habits, true, null);
            }
            catch (Exception ex)
            {
                return new GetAllHabitsResponse(false, ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteAhabit")]
        [ProducesResponseType(201)]
        public async Task<DeleteAhabitResponse> DeleteAhabit([FromBody] DeleteAhabitRequest request)
        {
            try
            {
                if (request == null)
                {
                    return new DeleteAhabitResponse(false, "Request is null.");
                }
                await _habitsService.DeleteHabit(request);
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
                if (request == null)
                {
                    return new AddDescriptionResponse(false, "Request is null.");
                }
                await _habitsService.AddHabitDescription(request);
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
        public async Task<GetHabitByIdResponse> GetHabitById([FromQuery] int id)
        {
            try
            {
                var habit = await _habitsService.GetHabitFromDB(id);
                if (habit == null)
                {
                    throw new Exception("Habit does not exist");
                }
                return new GetHabitByIdResponse(habit, true, null);
            }
            catch (Exception ex)
            {
                return new GetHabitByIdResponse(false, ex.Message);
            }
        }

        [HttpPut]
        [Route("ExtendHabit")]
        [ProducesResponseType(201)]
        public async Task<ExtendHabitResponse> ExtendHabit([FromBody] ExtendHabitRequest request)
        {
            try
            {
                if (request == null)
                {
                    return new ExtendHabitResponse(false, "Request is null.");
                }
                await _habitsService.SetIsExtended(request);
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
