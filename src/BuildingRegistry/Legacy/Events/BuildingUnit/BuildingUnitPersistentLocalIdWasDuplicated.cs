namespace BuildingRegistry.Legacy.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using Newtonsoft.Json;
    using NodaTime;

    [EventTags(EventTag.For.Sync)]
    [EventName("BuildingUnitPersistentLocalIdentifierWasDuplicated")]
    [EventDescription("Een gebouweenheid kreeg een tweede persistente lokale identificator toegekend door een bug.")]
    public class BuildingUnitPersistentLocalIdWasDuplicated : IHasProvenance, ISetProvenance, IMessage
    {
        [EventPropertyDescription("Interne GUID van het gebouw waartoe de gebouweenheid behoort.")]
        public Guid BuildingId { get; }

        [EventPropertyDescription("Interne GUID van de gebouweenheid.")]
        public Guid BuildingUnitId { get; }

        [EventPropertyDescription("Duplicate objectidentificator van de gebouweenheid.")]
        public int DuplicatePersistentLocalId { get; }

        [EventPropertyDescription("Originele objectidentificator van de gebouweenheid.")]
        public int OriginalPersistentLocalId { get; }

        [EventPropertyDescription("Tijdstip waarop de duplicate objectidentificator van de gebouweenheid werd toegekend.")]
        public Instant DuplicateAssignmentDate { get; }

        [EventPropertyDescription("Metadata bij het event.")]
        public ProvenanceData Provenance { get; private set; }

        public BuildingUnitPersistentLocalIdWasDuplicated(
            BuildingId buildingId,
            BuildingUnitId buildingUnitId,
            PersistentLocalId duplicatePersistentLocalId,
            PersistentLocalId originalPersistentLocalId,
            PersistentLocalIdAssignmentDate assignmentDate)
        {
            BuildingId = buildingId;
            BuildingUnitId = buildingUnitId;
            DuplicatePersistentLocalId = duplicatePersistentLocalId;
            OriginalPersistentLocalId = originalPersistentLocalId;
            DuplicateAssignmentDate = assignmentDate;
        }

        [JsonConstructor]
        private BuildingUnitPersistentLocalIdWasDuplicated(
            Guid buildingId,
            Guid buildingUnitId,
            int duplicatePersistentLocalId,
            int originalPersistentLocalId,
            Instant duplicateAssignmentDate,
            ProvenanceData provenance)
            : this(
                new BuildingId(buildingId),
                new BuildingUnitId(buildingUnitId),
                new PersistentLocalId(duplicatePersistentLocalId),
                new PersistentLocalId(originalPersistentLocalId),
                new PersistentLocalIdAssignmentDate(duplicateAssignmentDate))
            => ((ISetProvenance)this).SetProvenance(provenance.ToProvenance());

        public void SetProvenance(Provenance provenance) => Provenance = new ProvenanceData(provenance);
    }
}
