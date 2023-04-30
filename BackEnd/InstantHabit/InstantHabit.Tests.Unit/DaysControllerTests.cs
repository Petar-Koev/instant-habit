using InstantHabit.Controllers;
using InstantHabit.Interfaces;
using InstantHabit.Models;
using Moq;


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
        /*
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



        /*

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

        */

    }
}
