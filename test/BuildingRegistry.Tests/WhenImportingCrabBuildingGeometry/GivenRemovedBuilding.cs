namespace BuildingRegistry.Tests.WhenImportingCrabBuildingGeometry
{
    using Autofixture;
    using AutoFixture;
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using Building;
    using Building.Commands.Crab;
    using Building.Events;
    using ValueObjects;
    using Xunit;
    using Xunit.Abstractions;

    public class GivenRemovedBuilding : AutofacBasedTest
    {
        private readonly Fixture _fixture;

        public GivenRemovedBuilding(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _fixture = new Fixture();
            _fixture.Customize(new NodaTimeCustomization());
            _fixture.Customize(new WithFixedBuildingId());
            _fixture.Customize(new SetProvenanceImplementationsCallSetProvenance());
        }

        [Fact]
        public void ThenExceptionIsThrown()
        {
            _fixture.Customize(new WithNoDeleteModification());
            var command = _fixture.Create<ImportBuildingGeometryFromCrab>();

            var buildingId = _fixture.Create<BuildingId>();

            Assert(new Scenario()
                .Given(buildingId,
                    _fixture.Create<BuildingWasRegistered>(),
                    _fixture.Create<BuildingWasRemoved>()
                        .WithNoUnits())
                .When(command)
                .Throws(new BuildingRemovedException()));
        }
    }
}
