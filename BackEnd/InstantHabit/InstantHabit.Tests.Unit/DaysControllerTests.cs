using InstantHabit.Controllers;
using InstantHabit.Interfaces;
using InstantHabit.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantHabit.Tests.Unit
{
    public class DaysControllerTests
    {
        private readonly Mock<IDaysService> _daysServiceMock;
        private readonly DaysController _daysController;

        public DaysControllerTests()
        {
            _daysServiceMock = new Mock<IDaysService>();
            _daysController = new DaysController(_daysServiceMock.Object);
        }


        // Method: GetAllHabitDays
        // add negative test

        [Fact]
        public async Task GetAllHabitDays_ReturnsListWithAllHabitDaysFromDB()
        {
            // Given

            var listOfHabitDays = new List<Day>();
            int habitId = 2;
            

            _daysServiceMock.Setup((m) => m.GetDaysFromDB(habitId)).Returns(listOfHabitDays);

            // When

            var getAllHabitDaysResponse = await _daysController.GetAllHabitDays(habitId);

            // Then

            Assert.NotNull(getAllHabitDaysResponse);
            Assert.Equal(listOfHabitDays, getAllHabitDaysResponse);
        }

        // AddDay

        [Fact]
        public async Task AddDay_ReturnsAddDayResponse_WithSucceededTrue_And_WithoutErrors_WhenCheckForMatchEqualsNoMatch()
        {
            // Given

            var addDayRequest = new AddDayRequest();
            addDayRequest.HabitId = 2;
            addDayRequest.DayNumber = 2;

            _daysServiceMock.Setup((m) => m.MatchChecker(addDayRequest.HabitId, addDayRequest.DayNumber)).Returns("No match");


            // When

            var addDayResponse = await _daysController.AddDay(addDayRequest);

            // Then

            Assert.NotNull(addDayResponse);
            Assert.True(addDayResponse.Succeeded);
            Assert.Null(addDayResponse.Error);
        }

        
        [Fact]
        public async Task AddDay_ReturnsAddDayResponse_WithSucceededFalse_And_WithErrors_WhenCheckForMatchEqualsNoMatch_And_WhenAddNewDayThrowsException()
        {
            // Given
            var addDayRequest = new AddDayRequest();
            addDayRequest.HabitId = 2;
            addDayRequest.DayNumber = 2;

            _daysServiceMock.Setup((m) => m.MatchChecker(addDayRequest.HabitId, addDayRequest.DayNumber)).Returns("No match");
            _daysServiceMock.Setup((m) => m.AddNewDay(addDayRequest)).Throws(new Exception("bla"));

            // When

            var addDayResponse = await _daysController.AddDay(addDayRequest);

            // Then

            Assert.NotNull(addDayResponse);
            Assert.False(addDayResponse.Succeeded);
            Assert.Equal("bla", addDayResponse.Error);
        }

        
        [Fact]
        public async Task AddDay_ReturnsAddDayResponse_WithSucceededFalse_And_WithhCheckForMatchMessage_WhenMatchCheckerEqualsMatch()
        {
            // Given

            var addDayRequest = new AddDayRequest();
            addDayRequest.HabitId = 2;
            addDayRequest.DayNumber = 2;

            _daysServiceMock.Setup((m) => m.MatchChecker(addDayRequest.HabitId, addDayRequest.DayNumber)).Returns("Match");

            // When

            var addDayResponse = await _daysController.AddDay(addDayRequest);

            // Then

            Assert.NotNull(addDayResponse);
            Assert.False(addDayResponse.Succeeded);
            Assert.Equal("Match", addDayResponse.Error);
        }

        
        [Fact]
        public async Task AddDay_ReturnsAddDayResponse_WithSucceededFalse_And_WithDefaulErrortMessage_WhenBothConditionsAreFalse()
        {
            // Given

            var addDayRequest = new AddDayRequest();
            addDayRequest.HabitId = 2;
            addDayRequest.DayNumber = 2;

            _daysServiceMock.Setup((m) => m.MatchChecker(addDayRequest.HabitId, addDayRequest.DayNumber)).Returns("");

            // When


            var addDayResponse = await _daysController.AddDay(addDayRequest);

            // Then

            Assert.NotNull(addDayResponse);
            Assert.False(addDayResponse.Succeeded);
            Assert.Equal("something went wrong", addDayResponse.Error);
        }

        // Method: DeleteAhabit


        [Fact]
        public async Task DeleteDay_ReturnsDeleteDayResponse_WithSucceededTrue_And_WithoutErrors()
        {
            // Given

            var deleteDayRequest = new DeleteDayRequest();
            deleteDayRequest.HabitId = 2;
            deleteDayRequest.DayNumber = 2;

            _daysServiceMock.Setup((m) => m.DeleteSelectedDay(deleteDayRequest));


            // When

            var deleteDayResponse = await _daysController.DeleteDay(deleteDayRequest);

            // Then

            Assert.NotNull(deleteDayResponse);
            Assert.True(deleteDayResponse.Succeeded);
            Assert.Null(deleteDayResponse.Error);
        }


        [Fact]
        public async Task DeleteDay_ReturnsDeleteDayResponse_WithSucceededFalse_And_WhenDeleteDayThrowsException()
        {
            // Given
            var deleteDayRequest = new DeleteDayRequest();
            deleteDayRequest.HabitId = 2;
            deleteDayRequest.DayNumber = 2;

            _daysServiceMock.Setup((m) => m.DeleteSelectedDay(deleteDayRequest)).Throws(new Exception("fail")); ;

            // When

            var deleteDayResponse = await _daysController.DeleteDay(deleteDayRequest);

            // Then

            Assert.NotNull(deleteDayResponse);
            Assert.False(deleteDayResponse.Succeeded);
            Assert.Equal("fail", deleteDayResponse.Error);
        }

        // Method: AddDayDescription


        [Fact]
        public async Task AddDayDescription_AddDayDescriptionResponse_WithSucceededTrue_And_WithoutErrors()
        {
            // Given

            var addDayDescriptionRequest = new AddDayDescriptionRequest();
            addDayDescriptionRequest.HabitId = 2;
            addDayDescriptionRequest.DayNumber = 2;

            _daysServiceMock.Setup((m) => m.AddDailyDescription(addDayDescriptionRequest));


            // When

            var addDayDescriptionResponse = await _daysController.AddDescription(addDayDescriptionRequest);

            // Then

            Assert.NotNull(addDayDescriptionResponse);
            Assert.True(addDayDescriptionResponse.Succeeded);
            Assert.Null(addDayDescriptionResponse.Error);
        }


        [Fact]
        public async Task AddDayDescription_ReturnsAddDayDescriptionResponse_WithSucceededFalse_And_WhenAddDayDescriptionThrowsException()
        {
            // Given
            var addDayDescriptionRequest = new AddDayDescriptionRequest();
            addDayDescriptionRequest.HabitId = 2;
            addDayDescriptionRequest.DayNumber = 2;

            _daysServiceMock.Setup((m) => m.AddDailyDescription(addDayDescriptionRequest)).Throws(new Exception("fail")); ;

            // When

            var addDayDescriptionResponse = await _daysController.AddDescription(addDayDescriptionRequest);

            // Then

            Assert.NotNull(addDayDescriptionResponse);
            Assert.False(addDayDescriptionResponse.Succeeded);
            Assert.Equal("fail", addDayDescriptionResponse.Error);
        }

        // Method: GetDayByNumber

        [Fact]
        public async Task GetDayByNumber_ReturnsDayFromDB()
        {
            // Given

            var day = new Day();
            day.HabitId = 2;
            day.DayNumber = 2;


            _daysServiceMock.Setup((m) => m.GetDayFromDB(day.HabitId,day.DayNumber)).Returns(day);

            // When

            var getDayByIdResponse = await _daysController.GetDayByNumber(day.HabitId, day.DayNumber);

            // Then

            Assert.NotNull(getDayByIdResponse);
            Assert.Equal(day, getDayByIdResponse);
        }

    }
}
