using InstantHabit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InstantHabit.Models
{
    public class AddHabitRequest
    {
        public string Name { get; set; }
    }
}

