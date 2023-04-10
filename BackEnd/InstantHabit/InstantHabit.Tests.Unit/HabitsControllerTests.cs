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

        public HabitsControllerTests(IHabitsService habitsService)
        {
            _habitsServiceMock = new Mock<IHabitsService>();
            _habitsController = new HabitsController(_habitsServiceMock.Object);
        }
        
        [Fact]
        public async Task AddHabit_ReturnsAddHabitResponse_WithSucceededTrue_And_WithoutErrors_WhenMatchCheckerEqualsNoMatch()
        {
            // Given

            var addHabitRequest = new AddHabitRequest();
            addHabitRequest.Name = "Gosho";

            _habitsServiceMock.Setup((m) => m.MatchChecker(addHabitRequest.Name)).Returns("No match");
           

            // When

            var addHabitResponse = await _habitsController.AddHabit(addHabitRequest);

            // Then

            Assert.NotNull(addHabitResponse);
            Assert.True(addHabitResponse.Succeeded);
            Assert.Null(addHabitResponse.Error);    
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

        [Fact]
        public async Task AddHabit_ReturnsAddHabitResponse_WithSucceededFalse_And_WithMatchCheckerMessage_WhenMatchCheckerEqualsMatch()
        {
            // Given

            var addHabitRequest = new AddHabitRequest();
            addHabitRequest.Name = "WorkOut";

            _habitsServiceMock.Setup((m) => m.MatchChecker(addHabitRequest.Name)).Returns("Match");

            // When

            var addHabitResponse = await _habitsController.AddHabit(addHabitRequest);

            // Then

            Assert.NotNull(addHabitResponse);
            Assert.False(addHabitResponse.Succeeded);
            Assert.Equal("Match", addHabitResponse.Error);
        }
    }
}
