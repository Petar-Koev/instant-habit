using Azure;
using Azure.Core;
using InstantHabit.Interfaces;
using InstantHabit.Models;
using InstantHabit.Repositories;
using InstantHabit.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantHabit.Tests.Unit
{
    public class HabitsServiceTests
    {
        private readonly Mock<IHabitsRepository> _habitsRepositoryMock;
        private readonly HabitsServices _habitsService;

        public HabitsServiceTests()
        {
            _habitsRepositoryMock = new Mock<IHabitsRepository>();
            _habitsService = new HabitsServices(_habitsRepositoryMock.Object);
        }

        // SetIsExtended

        // Test: 01

        [Fact]
        public void  SetIsExtended_Calls_InsertHabitExtension_Successfuly_WithoutErrors()
        {
            // Given

            var request = new ExtendHabitRequest();
            request.HabitId = 2;

            _habitsRepositoryMock.Setup((m) => m.InsertHabitExtension(request.HabitId));

            // When

            _habitsService.SetIsExtended(request);

            // Then

            _habitsRepositoryMock.Verify(m => m.InsertHabitExtension(request.HabitId), Times.Once());
        }

        // Test: 02

        [Fact]
        public void SetIsExtended_When_InsertHabitExtensionThrowsException()
        {
            // Given

            var request = new ExtendHabitRequest();
            request.HabitId = 2;

            _habitsRepositoryMock.Setup((m) => m.InsertHabitExtension(request.HabitId)).Throws(new Exception("Error"));

            // When - Then

            var ex = Assert.Throws<Exception>(() => _habitsService.SetIsExtended(request));

            Assert.Equal("Error", ex.Message);

        }

        // GetHabitsFromDB

        // Test: 01

        [Fact]
        public void GetHabitsFromDB_ReturnsListOfHabits_WithoutErrors()
        {
            // Given

            var habits = new List<Habit>();
            habits.Add(new Habit
            {
                Description = "Daily training.",
                Name = "Working Out"
            });

            _habitsRepositoryMock.Setup((m) => m.GetHabits()).Returns(habits);

            // When

            var response =  _habitsService.GetHabitsFromDB();

            // Then

            Assert.NotNull(response);   
            Assert.Equal(habits, response);
            Assert.Equal(habits[0].Description, response[0].Description);
            Assert.Equal(habits[0].Name, response[0].Name);
        }

        // Test: 02
        // TODO!!!
        [Fact]
        public void GetHabitsFromDB_When_GetHabits_ThrowsException()
        {
            // Given

            string error = "";
            var response = new List<Habit>();

            _habitsRepositoryMock.Setup((m) => m.GetHabits()).Throws(new Exception("Error"));

            // When
            try
            {
                response = _habitsService.GetHabitsFromDB(); //No await???
            }
            catch(Exception ex)
            {
                error = ex.Message;
            }
            

            // Then

            Assert.NotNull(response);
            Assert.Equal("Error", error);

        }

        // MatchChecker

        // Test: 01

        [Fact]
        public void MatchChecker_ReturnsNoMatch_When_GetHabitsReturnsEmptyListOfHabit()
        {
            // Given

            var habits = new List<Habit>();
            var name = "Working Out";

            _habitsRepositoryMock.Setup((m) => m.GetHabits()).Returns(habits);

            // When

            var response = _habitsService.MatchChecker(name);

            // Then

            Assert.NotNull(response);
            Assert.Equal("No match", response);

        }


        // Test: 02

        [Fact]
        public async Task MatchChecker_ReturnsNoMatch_When_GetHabitsReturnsListOfHabits_WithNoMatches()
        {
            // Given

            var habits = new List<Habit>();
            habits.Add(new Habit
            {
                Name = "Studying",
                Description = "Test example"
            });

            var name = "Working Out";

            _habitsRepositoryMock.Setup((m) => m.GetHabits()).Returns(habits);

            // When

            var response = _habitsService.MatchChecker(name);

            // Then

            Assert.NotNull(response);
            Assert.Equal("No match", response);
            Assert.NotEqual(habits[0].Name, name);

        }

        // Test: 03

        [Fact]
        public async Task MatchChecker_ReturnsMatch_When_GetHabitsReturnsListOfHabits_WithMatch()
        {
            // Given

            var habits = new List<Habit>();
            habits.Add(new Habit
            {
                Name = "Studying",
                Description = "Test example"
            });

            _habitsRepositoryMock.Setup((m) => m.GetHabits()).Returns(habits);

            // When

            var response = _habitsService.MatchChecker(habits[0].Name);

            // Then

            Assert.NotNull(response);
            Assert.Equal("Match", response);
            Assert.Equal("Studying",habits[0].Name);

        }

        // Test: 04

        [Fact]
        public void MatchChecker_When_GetHabitsThrowsException()
        {
            // Given
            var name = "Test";
            _habitsRepositoryMock.Setup((m) => m.GetHabits()).Throws(new Exception("Error"));

            // When - Then

            var ex = Assert.Throws<Exception>(() => _habitsService.MatchChecker(name));

            Assert.Equal("Error", ex.Message);



        }
    }
}
