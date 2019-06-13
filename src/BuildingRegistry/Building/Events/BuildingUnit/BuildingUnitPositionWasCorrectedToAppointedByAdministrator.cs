namespace BuildingRegistry.Building.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using Newtonsoft.Json;
    using ValueObjects;

    [EventName("BuildingUnitPositionWasCorrectedToAppointedByAdministrator")]
    [EventDescription("Gebouweenheid positie werd aangeduid door beheerder.")]
    public class BuildingUnitPositionWasCorrectedToAppointedByAdministrator : IHasProvenance, ISetProvenance
    {
        public Guid BuildingId { get; }
        public Guid BuildingUnitId { get; }
        public string Position { get; }
        public ProvenanceData Provenance { get; private set; }

        public BuildingUnitPositionWasCorrectedToAppointedByAdministrator(
            BuildingId buildingId,
            BuildingUnitId buildingUnitId,
            ExtendedWkbGeometry position)
        {
            BuildingId = buildingId;
            BuildingUnitId = buildingUnitId;
            Position = position.ToString();
        }

        [JsonConstructor]
        private BuildingUnitPositionWasCorrectedToAppointedByAdministrator(
            Guid buildingId,
            Guid buildingUnitId,
            string position,
            ProvenanceData provenance)
            : this(
                new BuildingId(buildingId),
                new BuildingUnitId(buildingUnitId),
                new ExtendedWkbGeometry(position)) => ((ISetProvenance)this).SetProvenance(provenance.ToProvenance());
        
        void ISetProvenance.SetProvenance(Provenance provenance) => Provenance = new ProvenanceData(provenance);
    }
}
