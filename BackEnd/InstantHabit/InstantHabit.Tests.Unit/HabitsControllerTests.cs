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
        /*
        private readonly Mock<IHabitsService> _habitsServiceMock;
        private readonly HabitsController _habitsController;

        public HabitsControllerTests(IHabitsService habitsService)
        {
            _habitsServiceMock = new Mock<IHabitsService>();
            _habitsController = new HabitsController(_habitsServiceMock.Object);

        }
        */

        [Fact]
        public async Task AddHabit_ReturnsAddHabitResponse_WithSucceededTrue_And_WithoutErrors_WhenMatchCheckerEqualsNoMatch()
        {
            // Given

            var addHabitRequest = new AddHabitRequest();
            addHabitRequest.Name = "Gosho";

            var habitsServiceMock = new Mock<IHabitsService>();

            habitsServiceMock.Setup((m) => m.MatchChecker(addHabitRequest.Name)).Returns("No match");

            var habitsController = new HabitsController(habitsServiceMock.Object);
           

            // When

            var addHabitResponse = await habitsController.AddHabit(addHabitRequest);

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

            var habitsServiceMock = new Mock<IHabitsService>();

            habitsServiceMock.Setup((m) => m.MatchChecker(addHabitRequest.Name)).Returns("No match");
            habitsServiceMock.Setup((m) => m.CreateNewHabit(addHabitRequest)).Throws(new Exception("bla"));

            var habitsController = new HabitsController(habitsServiceMock.Object);


            // When

            var addHabitResponse = await habitsController.AddHabit(addHabitRequest);

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

            var habitsServiceMock = new Mock<IHabitsService>();

            habitsServiceMock.Setup((m) => m.MatchChecker(addHabitRequest.Name)).Returns("Match");

            var habitsController = new HabitsController(habitsServiceMock.Object);


            // When

            var addHabitResponse = await habitsController.AddHabit(addHabitRequest);

            // Then

            Assert.NotNull(addHabitResponse);
            Assert.False(addHabitResponse.Succeeded);
            Assert.Equal("Match", addHabitResponse.Error);
        }
    }
}
