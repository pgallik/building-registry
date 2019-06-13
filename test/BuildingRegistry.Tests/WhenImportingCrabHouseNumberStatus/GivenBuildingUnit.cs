namespace BuildingRegistry.Tests.WhenImportingCrabHouseNumberStatus
{
    using Be.Vlaanderen.Basisregisters.AggregateSource.Testing;
    using Be.Vlaanderen.Basisregisters.Crab;
    using Autofixture;
    using AutoFixture;
    using Building.Commands.Crab;
    using Building.Events;
    using ValueObjects;
    using ValueObjects.Crab;
    using Xunit;
    using Xunit.Abstractions;

    public class GivenBuildingUnit : AutofacBasedTest
    {
        private readonly Fixture _fixture = new Fixture();

        public GivenBuildingUnit(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _fixture.Customize(new InfrastructureCustomization());
            _fixture.Customize(new WithFixedBuildingUnitIdFromHouseNumber());
            _fixture.Customize(new WithNoDeleteModification());
        }

        [Theory]
        [InlineData(CrabAddressStatus.InUse)]
        [InlineData(CrabAddressStatus.OutOfUse)]
        [InlineData(CrabAddressStatus.Unofficial)]
        public void WhenStatusMapsToRealized(CrabAddressStatus status)
        {
            var importStatus = _fixture.Create<ImportHouseNumberStatusFromCrab>()
                .WithStatus(status);

            var buildingId = _fixture.Create<BuildingId>();

            Assert(new Scenario()
                .Given(buildingId,
                    _fixture.Create<BuildingWasRegistered>(),
                    _fixture.Create<BuildingUnitWasAdded>())
                .When(importStatus)
                .Then(buildingId,
                    new BuildingUnitWasRealized(buildingId, _fixture.Create<BuildingUnitId>()),
                    importStatus.ToLegacyEvent()));
        }

        [Theory]
        [InlineData(CrabAddressStatus.Proposed)]
        [InlineData(CrabAddressStatus.Reserved)]
        public void WhenStatusMapsToPlanned(CrabAddressStatus status)
        {
            var importStatus = _fixture.Create<ImportHouseNumberStatusFromCrab>()
                .WithStatus(status);

            var buildingId = _fixture.Create<BuildingId>();

            Assert(new Scenario()
                .Given(buildingId,
                    _fixture.Create<BuildingWasRegistered>(),
                    _fixture.Create<BuildingUnitWasAdded>())
                .When(importStatus)
                .Then(buildingId,
                    new BuildingUnitWasPlanned(buildingId, _fixture.Create<BuildingUnitId>()),
                    importStatus.ToLegacyEvent()));
        }

        [Fact]
        public void WithEmptyStatusWhenModificationRemoved()
        {
            var importStatus = _fixture.Create<ImportHouseNumberStatusFromCrab>()
                .WithModification(CrabModification.Delete);

            var buildingId = _fixture.Create<BuildingId>();

            Assert(new Scenario()
                .Given(buildingId,
                    _fixture.Create<BuildingWasRegistered>(),
                    _fixture.Create<BuildingUnitWasAdded>())
                .When(importStatus)
                .Then(buildingId,
                    importStatus.ToLegacyEvent()));
        }

        [Fact]
        public void WithStatusProposedWhenStatusIsRealizedAndNewerLifetime()
        {
            var importedStatus = _fixture.Create<ImportHouseNumberStatusFromCrab>()
                .WithStatus(CrabAddressStatus.InUse);

            var importStatus = _fixture.Create<ImportHouseNumberStatusFromCrab>()
                .WithStatus(CrabAddressStatus.Proposed)
                .WithLifetime(new CrabLifetime(importedStatus.Lifetime.BeginDateTime.Value.PlusDays(1), importedStatus.Lifetime.EndDateTime));

            var buildingId = _fixture.Create<BuildingId>();

            Assert(new Scenario()
                .Given(buildingId,
                    _fixture.Create<BuildingWasRegistered>(),
                    _fixture.Create<BuildingUnitWasAdded>(),
                    _fixture.Create<BuildingUnitWasRealized>(),
                    importedStatus.ToLegacyEvent())
                .When(importStatus)
                .Then(buildingId,
                    new BuildingUnitWasPlanned(buildingId, _fixture.Create<BuildingUnitId>()),
                    importStatus.ToLegacyEvent()));
        }

        [Fact]
        public void WithStatusProposedWhenStatusIsRealizedAndOlderLifetime()
        {
            var importedStatus = _fixture.Create<ImportHouseNumberStatusFromCrab>()
                .WithStatus(CrabAddressStatus.InUse);

            var importStatus = _fixture.Create<ImportHouseNumberStatusFromCrab>()
                .WithStatus(CrabAddressStatus.Proposed)
                .WithLifetime(new CrabLifetime(importedStatus.Lifetime.BeginDateTime.Value.PlusDays(-1), importedStatus.Lifetime.EndDateTime));

            var buildingId = _fixture.Create<BuildingId>();

            Assert(new Scenario()
                .Given(buildingId,
                    _fixture.Create<BuildingWasRegistered>(),
                    _fixture.Create<BuildingUnitWasAdded>(),
                    _fixture.Create<BuildingUnitWasRealized>(),
                    importedStatus.ToLegacyEvent())
                .When(importStatus)
                .Then(buildingId,
                    importStatus.ToLegacyEvent()));
        }

        // rest of tests: see address registry for status logic => only test if implemented here
    }
}
