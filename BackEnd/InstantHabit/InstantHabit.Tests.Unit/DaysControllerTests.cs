using InstantHabit.Controllers;
using InstantHabit.Interfaces;
using InstantHabit.Models;
using Moq;
using System.Security.Cryptography;


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

        // Test: 01

        [Fact]
        public async Task GetAllHabitDays_ReturnsGetAllHabitDaysResponse_WithListOfAllHabitDays_And_SucceedTrue_And_WithoutErrors()
        {
            // Given

            var days = new List<Day>();
            days.Add(new Day
            {
                HabitId = 1,
                DayNumber = 3,
                Note = "Test"
            });

            _daysServiceMock.Setup((m) => m.GetDaysFromDB(days[0].HabitId)).Returns(days);

            // When

            var getAllHabitsResponse = await _daysController.GetAllHabitDays(days[0].HabitId);

            // Then

            Assert.NotNull(getAllHabitsResponse);
            Assert.Equal(days, getAllHabitsResponse.Days);
            Assert.Equal(days[0].DayNumber, getAllHabitsResponse.Days[0].DayNumber);
            Assert.Equal(days[0].Note, getAllHabitsResponse.Days[0].Note);
            Assert.True(getAllHabitsResponse.Succeeded);
            Assert.Null(getAllHabitsResponse.Error);
        }


        // Test: 02


        [Fact]
        public async Task GetAllHabitDays_ReturnsGetAllHabitDaysResponse_When_GetDaysFromDBThrowsException()
        {
            // Given

            _daysServiceMock.Setup((m) => m.GetDaysFromDB(1)).Throws(new Exception("Error"));

            // When

            var getAllHabitsResponse = await _daysController.GetAllHabitDays(1);

            // Then

            Assert.NotNull(getAllHabitsResponse);
            Assert.False(getAllHabitsResponse.Succeeded);
            Assert.Equal("Error", getAllHabitsResponse.Error);
        }

        // Test: 03


        [Fact]
        public async Task GetAllHabitDays_ReturnsGetAllHabitDaysResponse_When_ResultEqualsNull()
        {
            // Given

            _daysServiceMock.Setup((m) => m.GetDaysFromDB(1)).Returns((List<Day>)null);

            // When

            var response = await _daysController.GetAllHabitDays(1);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Result is null", response.Error);
        }

        // Method: AddDay 
        // Test: 01

        [Fact]
        public async Task AddDay_ReturnsAddDayResponse_WithSucceededTrue_And_WithoutErrors_WhenMatchCheckerEqualsNoMatch()
        {
            // Given 

            var addDayRequest = new AddDayRequest();
            addDayRequest.HabitId = 1;
            addDayRequest.DayNumber = 3;

            _daysServiceMock.Setup((m) => m.MatchChecker(addDayRequest.HabitId,addDayRequest.DayNumber)).Returns("No match");

            // When

            var addDayResponse = await _daysController.AddDay(addDayRequest);

            // Then

            Assert.NotNull(addDayResponse);
            Assert.True(addDayResponse.Succeeded);
            Assert.Null(addDayResponse.Error);
            _daysServiceMock.Verify(m => m.AddNewDay(addDayRequest), Times.Once());

        }

        
        // Test: 02

        [Fact]
        public async Task AddDay_ReturnsAddDayResponse_WithSucceededFalse_And_WithErrors_WhenMatchCheckerEqualsNoMatch_And_WhenAddNewDayThrowsException()
        {
            // Given

            var addDayRequest = new AddDayRequest();
            addDayRequest.HabitId = 1;
            addDayRequest.DayNumber = 3;

            _daysServiceMock.Setup((m) => m.MatchChecker(addDayRequest.HabitId,addDayRequest.DayNumber)).Returns("No match");
            _daysServiceMock.Setup((m) => m.AddNewDay(addDayRequest)).Throws(new Exception("Error"));

            // When

            var addHabitResponse = await _daysController.AddDay(addDayRequest);

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
        public async Task AddDay_ReturnsAddDayResponse_WithSucceededFalse_And_WithCheckForMatchMessage_WhenMatchCheckerIsNotNoMatch(string matchChecker)
        {
            // Given

            var addDayRequest = new AddDayRequest();
            addDayRequest.HabitId = 1;
            addDayRequest.DayNumber = 3;

            _daysServiceMock.Setup((m) => m.MatchChecker(addDayRequest.HabitId, addDayRequest.DayNumber)).Returns(matchChecker);

            // When

            var addHabitResponse = await _daysController.AddDay(addDayRequest);

            // Then

            Assert.NotNull(addHabitResponse);
            Assert.False(addHabitResponse.Succeeded);
            Assert.Equal(matchChecker, addHabitResponse.Error);
        }
        

        // Test: 04

        [Fact]
        public async Task AddDay_ReturnsAddDayResponse_WithSucceededFalse_And_WithError_WhenRequestIsNull()
        {

            // When 

            var addHabitResponse = await _daysController.AddDay(null);

            // Then

            Assert.NotNull(addHabitResponse);
            Assert.False(addHabitResponse.Succeeded);
            Assert.Equal("Request is null.", addHabitResponse.Error);
        }
        
        // Test: 05

        [Fact]
        public async Task AddDay_ReturnsAddDayResponse_WithSucceededFalse_And_WithErrors_WhenMatchCheckerThrowsException()
        {
            // Given

            var addDayRequest = new AddDayRequest();
            addDayRequest.HabitId = 1;
            addDayRequest.DayNumber = 3;

            _daysServiceMock.Setup((m) => m.MatchChecker(addDayRequest.HabitId, addDayRequest.DayNumber)).Throws(new Exception("Error"));

            // When

            var addHabitResponse = await _daysController.AddDay(addDayRequest);

            // Then

            Assert.NotNull(addHabitResponse);
            Assert.False(addHabitResponse.Succeeded);
            Assert.Equal("Error", addHabitResponse.Error);

        }

        // Method: DeleteDay

        // Test: 01

        [Fact]
        public async Task DeleteDay_ReturnsNewDeleteDayResponse_WhenRequestEqualsNull()
        {
            // When 

            var response = await _daysController.DeleteDay(null);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Request is null.", response.Error);
        }

        // Test: 02
        

        [Fact]
        public async Task DeleteDay_ReturnsNewDeleteDayResponse_WithSucceededTrue_And_WithoutErrors()
        {
            // Given

            var request = new DeleteDayRequest();

            _daysServiceMock.Setup((m) => m.DeleteSelectedDay(request));

            // When

            var response = await _daysController.DeleteDay(request);

            // Then

            Assert.NotNull(response);
            Assert.True(response.Succeeded);
            Assert.Null(response.Error);
            _daysServiceMock.Verify(m => m.DeleteSelectedDay(request), Times.Once());
        }

        // Test: 03
        

        [Fact]
        public async Task DeleteDay_ReturnsNewDayHabitResponse_WithSucceededFalse_And_DeleteSelectedDayThrowsException()
        {
            // Given

            var request = new DeleteDayRequest();

            _daysServiceMock.Setup((m) => m.DeleteSelectedDay(request)).Throws(new Exception("Error"));

            // When

            var response = await _daysController.DeleteDay(request);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Error", response.Error);
        }

        // Method: AddDayDescription

        // Test: 01

        [Fact]
        public async Task AddDayDescription_ReturnsNewAddDayDescriptionResponse_WhenRequestEqualsNull()
        {
            // When 

            var response = await _daysController.AddDescription(null);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Request is null.", response.Error);
        }

        // Test: 02

        
        [Fact]
        public async Task AddDayDescription_ReturnsNewAddDayDescriptionResponse_WithSucceededTrue_And_WithoutErrors()
        {
            // Given

            var request = new AddDayDescriptionRequest();

            _daysServiceMock.Setup((m) => m.AddDailyDescription(request));

            // When

            var response = await _daysController.AddDescription(request);

            // Then

            Assert.NotNull(response);
            Assert.True(response.Succeeded);
            Assert.Null(response.Error);
            _daysServiceMock.Verify(m => m.AddDailyDescription(request), Times.Once());
        }

        // Test: 03

        
        [Fact]
        public async Task AddDayDescription_ReturnsNewAddDayDescriptionResponse_WithSucceededFalse_And_AddDailyDescriptionThrowsException()
        {
            // Given

            var request = new AddDayDescriptionRequest();

            _daysServiceMock.Setup((m) => m.AddDailyDescription(request)).Throws(new Exception("Error"));

            // When

            var response = await _daysController.AddDescription(request);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Error", response.Error);
        }

        // GetDayByNumber

        // Test: 01


        [Fact]
        public async Task GetDayByNumber_ReturnsGetDayByNumberResponse_WithDay_And_SucceededTrue_And_WithoutErrors()
        {
            // Given

            var day = new Day();
            day.DayNumber = 4;
            day.HabitId = 1;
            day.Note = "Test Description";
           

            _daysServiceMock.Setup((m) => m.GetDayFromDB(day.HabitId,day.DayNumber)).Returns(day);

            // When

            var response = await _daysController.GetDayByNumber(day.DayNumber, day.HabitId);

            // Then

            Assert.NotNull(response);
            Assert.Equal(day, response.Day);
            Assert.Equal(day.Note, response.Day.Note);
            Assert.True(response.Succeeded);
            Assert.Null(response.Error);
        }

        // Test: 02


        [Fact]
        public async Task GetDayByNumber_ReturnsGetDayByNumberResponse_WhenDayEqualsNull()
        {
            // Given

            int dayNumber = 4;
            int habitId = 1;
            _daysServiceMock.Setup((m) => m.GetDayFromDB(habitId,dayNumber)).Returns((Day)null);

            // When

            var response = await _daysController.GetDayByNumber(dayNumber,habitId);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Day does not exist", response.Error);
        }

        // Test: 03


        [Fact]
        public async Task GetDayByNumber_ReturnsGetDayByNumberResponse_When_GetDayFromDBThrowsException()
        {

            // Given

            int dayNumber = 1;
            int habitId = 4;    

            _daysServiceMock.Setup((m) => m.GetDayFromDB(habitId,dayNumber)).Throws(new Exception("Error"));

            // When

            var response = await _daysController.GetDayByNumber(1,4);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Error", response.Error);
        }

        // GetBestStreak

        // Test: 01

        [Fact]
        public async Task GetBestStreak_ReturnsBestStreakResponse_WhenResultIsNull()
        {
            // Given

            _daysServiceMock.Setup((m) => m.GetStreakMessage(1)).Returns((BestStreakResponse)null);

            // When

            var response = await _daysController.GetBestStreak(1);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Result is null", response.Error);
        }

        // Test: 02

        [Fact]
        public async Task GetBestStreak_ReturnsBestStreakResponse_WithSucceededTrue_And_WithoutErrors()
        {
            // Given

            _daysServiceMock.Setup((m) => m.GetStreakMessage(1)).Throws(new Exception("Error"));
            // When

            var response = await _daysController.GetBestStreak(1);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Error", response.Error);
        }

        // Test: 03

        [Fact]
        public async Task GetBestStreak_ReturnsBestStreakResponse_WithBestStreak_And_Message()
        {
            // Given
            var streak = new BestStreakResponse(12,"Well Done",true,null);

            _daysServiceMock.Setup((m) => m.GetStreakMessage(1)).Returns(streak);
            // When

            var response = await _daysController.GetBestStreak(1);

            // Then

            Assert.NotNull(response);
            Assert.Equal(streak.BestStreak,response.BestStreak);
            Assert.Equal(streak.MotivationalMessage,response.MotivationalMessage);  
            Assert.True(response.Succeeded);
            Assert.Null(response.Error);
        }
        
        // ResetChecker

        // Test: 01

        [Fact]
        public async Task ResetChecker_ReturnsExceptionMessage_WhenConfirmationIsNull()
        {
            // Given
            int dayNumber = 25;
            int habitId = 1;

            _daysServiceMock.Setup((m) => m.DaysListResetChecker(dayNumber,habitId)).Returns((string)null);

            // When

            var response = await _daysController.ResetChecker(dayNumber,habitId);

            // Then

            Assert.NotNull(response);
            Assert.Equal("Something went wrong", response);
        }

        // Test: 02

        [Theory]
        [InlineData("You succeeded")]
        [InlineData("You failed.")]
        public async Task ResetChecker_ReturnsConfirmation(string confirmation)
        {
            // Given

            int dayNumber = 25;
            int habitId = 1;

            _daysServiceMock.Setup((m) => m.DaysListResetChecker(dayNumber, habitId)).Returns(confirmation);

            // When

            var response = await _daysController.ResetChecker(dayNumber,habitId);

            // Then

            Assert.NotNull(response);
            Assert.Equal(confirmation, response);
        }

        // Test: 03

        [Fact]
        public async Task ResetChecker_ReturnsExceptionMessage_WhenDaysListResetCheckerThrowsException()
        {
            // Given
            int dayNumber = 25;
            int habitId = 1;

            _daysServiceMock.Setup((m) => m.DaysListResetChecker(dayNumber, habitId)).Throws(new Exception("Error"));

            // When

            var response = await _daysController.ResetChecker(dayNumber, habitId);

            // Then

            Assert.NotNull(response);
            Assert.Equal("Error", response);
        }

        // Method: DeleteHabitDays

        // Test: 01

        [Fact]
        public async Task DeleteHabitDays_DeleteHabitDaysResponse_WhenRequestEqualsNull()
        {

            // When 

            var response = await _daysController.DeleteHabitDays(null);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Request is null", response.Error);
        }

        // Test: 02
        

        [Fact]
        public async Task DeleteHabitDays_DeleteHabitDaysResponse_WithSucceededTrue_And_WithoutErrors()
        {
            // Given

            var request = new DeleteHabitDaysRequest();

            _daysServiceMock.Setup((m) => m.DeleteDays(request));

            // When

            var response = await _daysController.DeleteHabitDays(request);

            // Then

            Assert.NotNull(response);
            Assert.True(response.Succeeded);
            Assert.Null(response.Error);
            _daysServiceMock.Verify(m => m.DeleteDays(request), Times.Once());
        }

        // Test: 03

        
        [Fact]
        public async Task DeleteHabitDays_DeleteHabitDaysResponse_WithSucceededFalse_And_DeleteDaysThrowsException()
        {
            // Given

            var request = new DeleteHabitDaysRequest();

            _daysServiceMock.Setup((m) => m.DeleteDays(request)).Throws(new Exception("Error"));

            // When

            var response = await _daysController.DeleteHabitDays(request);

            // Then

            Assert.NotNull(response);
            Assert.False(response.Succeeded);
            Assert.Equal("Error", response.Error);
        }


    }
}
