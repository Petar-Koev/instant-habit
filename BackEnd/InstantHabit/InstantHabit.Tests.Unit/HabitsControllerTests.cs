using Azure.Core;
using InstantHabit.Controllers;
using InstantHabit.Interfaces;
using InstantHabit.Models;
using InstantHabit.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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

        }



        /*


            Assert.NotNull(addHabitResponse);
            Assert.True(addHabitResponse.Succeeded);
            Assert.Null(addHabitResponse.Error);
            _habitsServiceMock.Verify(m => m.CreateNewHabit(addHabitRequest), Times.Once());
        }

        [Fact]
        public async Task AddHabit_ReturnsAddHabitResponse_WithSucceededFalse_And_WithErrors_WhenMatchCheckerEqualsNoMatch_And_WhenCreateNewHabitThrowsException()
        {
            // Given

            var addHabitRequest = new AddHabitRequest();
            addHabitRequest.Name = "WorkOut";

            _habitsServiceMock.Setup((m) => m.MatchChecker(addHabitRequest.Name)).Returns("No match");
            _habitsServiceMock.Setup((m) => m.CreateNewHabit(addHabitRequest)).Throws(new Exception("bla"));

            // When

            var addHabitResponse = await _habitsController.AddHabit(addHabitRequest);

            // Then

            Assert.NotNull(addHabitResponse);
            Assert.False(addHabitResponse.Succeeded);
            Assert.Equal("bla",addHabitResponse.Error);
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("lowercase")]
        [InlineData("UPPERCASE")]
        [InlineData("Match")]
        [InlineData("")]
        public async Task AddHabit_ReturnsAddHabitResponse_WithSucceededFalse_And_WithMatchCheckerMessage_WhenMatchCheckerEqualsMatch(string matchChecker)
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

       
        // Method: GetAllHabits

        [Fact]
        public async Task GetAllHabits_ReturnsListWithAllHabitsFromDB()
        {
            // Given

            var listOfHabits = new List<Habit>();  
            listOfHabits.Add(new Habit
            {
                Description = "It Works Fine!"
            });

            _habitsServiceMock.Setup((m) => m.GetHabitsFromDB()).Returns(listOfHabits);

            // When

            var getAllHabitsResponse = await _habitsController.GetAllHabits();

            // Then

            Assert.NotNull(getAllHabitsResponse);
            Assert.Equal(listOfHabits, getAllHabitsResponse);
            Assert.Equal(listOfHabits[0].Description, getAllHabitsResponse[0].Description);
        }

        // Test To Be Checked: 1(new test)

        [Fact]
        public async Task GetAllHabits_ThrowsException_And_ReturnsNewHabitList()
        {
            // Given


            _habitsServiceMock.Setup((m) => m.GetHabitsFromDB()).Throws(new Exception("no such habit"));

            // When

            var getAllHabitsResponse = await _habitsController.GetAllHabits();

            // Then

            Assert.NotNull(getAllHabitsResponse);
           // Assert.Equal(listOfHabits, getAllHabitsResponse);
            Assert.Empty(getAllHabitsResponse);
        }


        // Method: DeleteAhabit

        // Test To Be Checked: 2(verify, happy path / removed from Exception Test)

        [Fact]
        public async Task DeleteAhabit_ReturnsDeleteAhabitResponse_WithSucceededTrue_And_WithoutErrors()
        {
            // Given

            var deleteHabitRequest = new DeleteAhabitRequest();

            _habitsServiceMock.Setup((m) => m.DeleteHabit(deleteHabitRequest));

            // When

            var deleteHabitResponse = await _habitsController.DeleteAhabit(deleteHabitRequest);

            // Then

            Assert.NotNull(deleteHabitResponse);
            Assert.True(deleteHabitResponse.Succeeded);
            Assert.Null(deleteHabitResponse.Error);
            _habitsServiceMock.Verify(m => m.DeleteHabit(deleteHabitRequest), Times.Once());
        }


        [Fact]
        public async Task DeleteAhabit_ReturnsDeleteAhabitResponse_WithSucceededFalse_And_WhenDeleteHabitThrowsException()
        {
            // Given

            var deleteHabitRequest = new DeleteAhabitRequest();

            _habitsServiceMock.Setup((m) => m.DeleteHabit(deleteHabitRequest)).Throws(new Exception("Fail")); ;

            // When

            var deleteHabitResponse = await _habitsController.DeleteAhabit(deleteHabitRequest);

            // Then

            Assert.NotNull(deleteHabitResponse);
            Assert.False(deleteHabitResponse.Succeeded);
            Assert.Equal("Fail", deleteHabitResponse.Error);
        }

        // Method: AddDescription

        // Test To Be Checked: 4(verify???)


        [Fact]
        public async Task AddDescription_ReturnsAddDescriptionResponse_WithSucceededTrue_And_WithoutErrors()
        {
            // Given

            var addDescriptionRequest = new AddDescriptionRequest();

            _habitsServiceMock.Setup((m) => m.AddHabitDescription(addDescriptionRequest));

            // When

            var addDescriptionResponse = await _habitsController.AddDescription(addDescriptionRequest);

            // Then

            Assert.NotNull(addDescriptionResponse);
            Assert.True(addDescriptionResponse.Succeeded);
            Assert.Null(addDescriptionResponse.Error);
            _habitsServiceMock.Verify(m => m.AddHabitDescription(addDescriptionRequest), Times.Once());
        }


        [Fact]
        public async Task AddDescription_ReturnsAddDescriptionResponse_WithSucceededFalse_And_WhenAddDesciptionThrowsException()
        {
            // Given

            var addDescriptionRequest = new AddDescriptionRequest();

            _habitsServiceMock.Setup((m) => m.AddHabitDescription(addDescriptionRequest)).Throws(new Exception("Fail")); ;

            // When

            var addDescriptionResponse = await _habitsController.AddDescription(addDescriptionRequest);

            // Then

            Assert.NotNull(addDescriptionResponse);
            Assert.False(addDescriptionResponse.Succeeded);
            Assert.Equal("Fail", addDescriptionResponse.Error);
        }


        // Method: GetHabitById

        // Test To Be Checked: 5(new)

        [Theory]
        [InlineData(2,"name",true)]
        [InlineData(6, "UPPER", true)]
        [InlineData(3, "lower", false)]
        public async Task GetHabitById_ReturnsHabitFromDB(int id, string name, bool isExtended)
        {
            // Given

            var habit = new Habit();
            habit.Id = id;
            habit.Name = name;
            habit.IsExtended = isExtended;    

            _habitsServiceMock.Setup((m) => m.GetHabitFromDB(id)).Returns(habit);

            // When

            var getHabitByIdResponse = await _habitsController.GetHabitById(id);

            // Then

            Assert.NotNull(getHabitByIdResponse);
            Assert.Equal(habit, getHabitByIdResponse);
            Assert.Equal(habit.Name, getHabitByIdResponse.Name);
            Assert.Equal(habit.IsExtended, getHabitByIdResponse.IsExtended);
        }

        // Method: ExtendHabit



        [Fact]
        public async Task ExtendHabit_ReturnsExxtendHabitResponse_WithSucceededTrue_And_WithoutErrors()
        {
            // Given

            var extendHabitRequest = new ExtendHabitRequest();
            extendHabitRequest.HabitId = 2;

            _habitsServiceMock.Setup((m) => m.SetIsExtended(extendHabitRequest));


            // When

            var extendHabitResponse = await _habitsController.ExtendHabit(extendHabitRequest);

            // Then

            Assert.NotNull(extendHabitResponse);
            Assert.True(extendHabitResponse.Succeeded);
            Assert.Null(extendHabitResponse.Error);
        }


        [Fact]
        public async Task ExtendHabit_ReturnsExtendHabitResponse_WithSucceededFalse_And_WhenExtendHabitThrowsException()
        {
            // Given

            var extendHabitRequest = new ExtendHabitRequest();
            extendHabitRequest.HabitId = 2;

            _habitsServiceMock.Setup((m) => m.SetIsExtended(extendHabitRequest)).Throws(new Exception("fail")); ;


            // When

            var extendHabitResponse = await _habitsController.ExtendHabit(extendHabitRequest);

            // Then

            Assert.NotNull(extendHabitResponse);
            Assert.False(extendHabitResponse.Succeeded);
            Assert.Equal("fail", extendHabitResponse.Error);
        }


        */
    }
}


