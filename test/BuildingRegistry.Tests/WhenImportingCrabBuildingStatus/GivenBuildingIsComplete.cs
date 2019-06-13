namespace BuildingRegistry.Tests.WhenImportingCrabBuildingStatus
{
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using Be.Vlaanderen.Basisregisters.Crab;
    using Autofixture;
    using AutoFixture;
    using Building.Commands.Crab;
    using Building.Events;
    using ValueObjects;
    using Xunit;
    using Xunit.Abstractions;

    public class GivenBuildingIsComplete : AutofacBasedTest
    {
        private readonly Fixture _fixture;

        public GivenBuildingIsComplete(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _fixture = new Fixture();
            _fixture.Customize(new InfrastructureCustomization());
            _fixture.Customize(new WithFixedBuildingId());
        }

        [Fact]
        public void ThenBuildingBecameIncompleteWhenModificationIsDelete()
        {
            var importStatus = _fixture.Create<ImportBuildingStatusFromCrab>()
                .WithCrabModification(CrabModification.Delete);

            var buildingId = _fixture.Create<BuildingId>();
            Assert(new Scenario()
                .Given(buildingId,
                    _fixture.Create<BuildingWasRegistered>(),
                    _fixture.Create<BuildingWasMeasuredByGrb>(),
                    _fixture.Create<BuildingBecameUnderConstruction>(),
                    _fixture.Create<BuildingBecameComplete>())
            .When(importStatus)
            .Then(buildingId,
                    new BuildingStatusWasRemoved(buildingId),
                    new BuildingBecameIncomplete(buildingId),
                    importStatus.ToLegacyEvent())
            );
        }
    }
}
