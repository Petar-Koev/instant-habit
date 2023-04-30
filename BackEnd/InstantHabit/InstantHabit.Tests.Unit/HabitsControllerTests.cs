using Azure.Core;
using InstantHabit.Controllers;
using InstantHabit.Interfaces;
using InstantHabit.Models;
using InstantHabit.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace InstantHabit.Tests.Unit
{
    public class HabitsControllerTests
    {
        
        private readonly Mock<IHabitsService> _habitsServiceMock;
        private readonly HabitsController _habitsController;

        public HabitsControllerTests()
        {
            _habitsServiceMock = new Mock<IHabitsService>();
            _habitsController = new HabitsController(_habitsServiceMock.Object);
        }

        // Method: AddHabit 
        // Test: 01

        [Fact]
        public async Task AddHabit_ReturnsAddHabitResponse_WithSucceededTrue_And_WithoutErrors_WhenMatchCheckerEqualsNoMatch()
        {
            // Given 

            var addHabitRequest = new AddHabitRequest();
            addHabitRequest.Name = "Working Out";

            _habitsServiceMock.Setup((m) => m.MatchChecker(addHabitRequest.Name)).Returns("No match");

            // When

            var addHabitResponse = await _habitsController.AddHabit(addHabitRequest);   

            // Then

            Assert.NotNull(addHabitResponse);
            Assert.True(addHabitResponse.Succeeded);
            Assert.Null(addHabitResponse.Error);
            _habitsServiceMock.Verify(m => m.CreateNewHabit(addHabitRequest), Times.Once());
            /*
            _habitsServiceMock.Verify(m => m.MatchChecker(addHabitRequest.Name), Times.Once());
            */
        }

        // Test: 02

        [Fact]
        public async Task AddHabit_ReturnsAddHabitResponse_WithSucceededFalse_And_WithErrors_WhenMatchCheckerEqualsNoMatch_And_WhenCreateNewHabitThrowsException()
        {
            // Given

            var addHabitRequest = new AddHabitRequest();
            addHabitRequest.Name = "Text";

            _habitsServiceMock.Setup((m) => m.MatchChecker(addHabitRequest.Name)).Returns("No match");
            _habitsServiceMock.Setup((m) => m.CreateNewHabit(addHabitRequest)).Throws(new Exception("Error"));

            // When

            var addHabitResponse = await _habitsController.AddHabit(addHabitRequest);

            // Then

            Assert.NotNull(addHabitResponse);
            Assert.False(addHabitResponse.Succeeded);
            Assert.Equal("Error", addHabitResponse.Error);

        }

        // Test: 03

        [Theory]
        [InlineData("Test")]
        [InlineData("Match")]
        [InlineData("")]
        public async Task AddHabit_ReturnsAddHabitResponse_WithSucceededFalse_And_WithMatchCheckerMessage_WhenMatchCheckerIsNotNoMatch(string matchChecker)
        {
            // Given

            var addHabitRequest = new AddHabitRequest();
            addHabitRequest.Name = "WorkOut";

            _habitsServiceMock.Setup((m) => m.MatchChecker(addHabitRequest.Name)).Returns(matchChecker);

            // When

            var addHabitResponse = await _habitsController.AddHabit(addHabitRequest);

            // Then

            Assert.NotNull(addHabitResponse);
            Assert.False(addHabitResponse.Succeeded);
            Assert.Equal(matchChecker, addHabitResponse.Error);
        }

        // Test: 04

        [Fact]
        public async Task AddHabit_ReturnsAddHabitResponse_WithSucceededFalse_And_WithError_WhenRequestIsNull()
        {

            // When 

            var addHabitResponse = await _habitsController.AddHabit(null);

            // Then

            Assert.NotNull(addHabitResponse);
            Assert.False(addHabitResponse.Succeeded);
            Assert.Equal("Request is null.", addHabitResponse.Error);
        }

        // Test: 05

        [Fact]
        public async Task AddHabit_ReturnsAddHabitResponse_WithSucceededFalse_And_WithErrors_WhenMatchCheckerThrowsException()
        {
            // Given

            var addHabitRequest = new AddHabitRequest();
            addHabitRequest.Name = "Text";

            _habitsServiceMock.Setup((m) => m.MatchChecker(addHabitRequest.Name)).Throws(new Exception("Error"));

            // When

            var addHabitResponse = await _habitsController.AddHabit(addHabitRequest);

            // Then

            Assert.NotNull(addHabitResponse);
            Assert.False(addHabitResponse.Succeeded);
            Assert.Equal("Error", addHabitResponse.Error);

        }


        // Method: GetAllHabits

        // Test: 01


        [Fact]
        public async Task GetAllHabits_ReturnsGetAllHabitsResponse_WithListOfAllHabits_And_SucceedTrue_And_WithoutErrors()
        {
            // Given

            var habits = new List<Habit>();
            habits.Add(new Habit
            {
                Description = "Daily training.",
                Name = "Working Out"
            });

            _habitsServiceMock.Setup((m) => m.GetHabitsFromDB()).Returns(habits);

            // When

            var getAllHabitsResponse = await _habitsController.GetAllHabits();

            // Then

            Assert.NotNull(getAllHabitsResponse);
            Assert.Equal(habits, getAllHabitsResponse.Habits);
            Assert.Equal(habits[0].Description, getAllHabitsResponse.Habits[0].Description);
            Assert.Equal(habits[0].Name, getAllHabitsResponse.Habits[0].Name);
            Assert.True(getAllHabitsResponse.Succeeded);
            Assert.Null(getAllHabitsResponse.Error);
        }

        // Test: 02

        [Fact]
        public async Task GetAllHabits_ReturnsGetAllHabitsResponse_When_GetHabitsFromDBThrowsexception()
        {
            // Given

            _habitsServiceMock.Setup((m) => m.GetHabitsFromDB()).Throws(new Exception("Error"));

            // When

            var getAllHabitsResponse = await _habitsController.GetAllHabits();

            // Then

            Assert.NotNull(getAllHabitsResponse);
            Assert.False(getAllHabitsResponse.Succeeded);
            Assert.Equal("Error", getAllHabitsResponse.Error);
        }

        // Method: DeleteAHabit

        // Test: 01

        [Fact]
        public async Task DeleteAHabit_ReturnsNewDeleteAHabitResponse_WhenRequestEqualsNull()
        {
            // When 

            var deleteHabitResponse = await _habitsController.DeleteAhabit(null);

            // Then

            Assert.NotNull(deleteHabitResponse);
            Assert.False(deleteHabitResponse.Succeeded);
            Assert.Equal("Request is null.", deleteHabitResponse.Error);
        }

        // Test: 02


        [Fact]
        public async Task DeleteAHabit_ReturnsNewDeleteAHabitResponse_WithSucceededTrue_And_WithoutErrors()
        {
            // Given

            var deleteHabitRequest = new DeleteAhabitRequest();

            _habitsServiceMock.Setup((m) => m.DeleteHabit(deleteHabitRequest));

            // When

            var response = await _habitsController.DeleteAhabit(deleteHabitRequest);

            // Then

            Assert.NotNull(response);
            Assert.True(response.Succeeded);
            Assert.Null(response.Error);
            _habitsServiceMock.Verify(m => m.DeleteHabit(deleteHabitRequest), Times.Once());
        }

        // Test: 03


        [Fact]
        public async Task DeleteAHabit_ReturnsNewDeleteAHabitResponse_WithSucceededFalse_And_DeleteHabitThrowsException()
        {
            // Given

            var deleteHabitRequest = new DeleteAhabitRequest();

            _habitsServiceMock.Setup((m) => m.DeleteHabit(deleteHabitRequest)).Throws(new Exception("Error"));

            // When

            var response = await _habitsController.DeleteAhabit(deleteHabitRequest);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Error", response.Error);
        }

        // Method: AddDescription

        // Test: 01

        [Fact]
        public async Task AddDescription_ReturnsNewAddDescriptionResponse_WhenRequestEqualsNull()
        {

            // When 

            var response = await _habitsController.AddDescription(null);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Request is null.", response.Error);
        }

        // Test: 02


        [Fact]
        public async Task AddDescription_ReturnsNewAddDescriptionResponse_WithSucceededTrue_And_WithoutErrors()
        {
            // Given

            var request = new AddDescriptionRequest();

            _habitsServiceMock.Setup((m) => m.AddHabitDescription(request));

            // When

            var response = await _habitsController.AddDescription(request);

            // Then

            Assert.NotNull(response);
            Assert.True(response.Succeeded);
            Assert.Null(response.Error);
            _habitsServiceMock.Verify(m => m.AddHabitDescription(request), Times.Once());
        }

        // Test: 03


        [Fact]
        public async Task AddDescription_ReturnsNewAddDescriptiontResponse_WithSucceededFalse_And_AddHabitDescriptionThrowsException()
        {
            // Given

            var request = new AddDescriptionRequest();

            _habitsServiceMock.Setup((m) => m.AddHabitDescription(request)).Throws(new Exception("Error"));

            // When

            var response = await _habitsController.AddDescription(request);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Error", response.Error);
        }


        // Method: GetHabitById

        // Test: 01


        [Fact]
        public async Task GetHabitById_ReturnsGetHabitByIdResponse_WithHabit_And_SucceededTrue_And_WithoutErrors()
        {
            // Given

            var habit = new Habit();
            habit.Name = "Working Out";
            habit.Description = "Test";
            habit.Id = 2;

            _habitsServiceMock.Setup((m) => m.GetHabitFromDB(habit.Id)).Returns(habit);

            // When

            var response = await _habitsController.GetHabitById(habit.Id);

            // Then

            Assert.NotNull(response);
            Assert.Equal(habit, response.Habit);
            Assert.Equal(habit.Description, response.Habit.Description);
            Assert.Equal(habit.Name, response.Habit.Name);
            Assert.True(response.Succeeded);
            Assert.Null(response.Error);
        }

        // Test: 02


        [Fact]
        public async Task GetHabitById_ReturnsGetHabitByIdResponse_WhenHabitEqualsNull()
        {
            _habitsServiceMock.Setup((m) => m.GetHabitFromDB(1)).Returns((Habit)null);

            // When

            var response = await _habitsController.GetHabitById(1);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Habit does not exist",response.Error);
        }

        // Method: ExtendHabit

        // Test: 01

        [Fact]
        public async Task ExtendHabit_ReturnsNewExtendHabitResponse_WhenRequestEqualsNull()
        {

            // When 

            var response = await _habitsController.ExtendHabit(null);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Request is null.", response.Error);
        }

        // Test: 02

        
        [Fact]
        public async Task ExtendHabit_ReturnsNewExtendHabitResponse_WithSucceededTrue_And_WithoutErrors()
        {
            // Given

            var request = new ExtendHabitRequest();

            _habitsServiceMock.Setup((m) => m.SetIsExtended(request));

            // When

            var response = await _habitsController.ExtendHabit(request);

            // Then

            Assert.NotNull(response);
            Assert.True(response.Succeeded);
            Assert.Null(response.Error);
            _habitsServiceMock.Verify(m => m.SetIsExtended(request), Times.Once());
        }
        

        // Test: 03


        [Fact]
        public async Task ExtendHabit_ReturnsNewExtendHabitResponse_WithSucceededFalse_And_ExtendHabitThrowsException()
        {
            // Given

            var request = new ExtendHabitRequest();

            _habitsServiceMock.Setup((m) => m.SetIsExtended(request)).Throws(new Exception("Error"));

            // When

            var response = await _habitsController.ExtendHabit(request);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Error", response.Error);
        }




    }
}


