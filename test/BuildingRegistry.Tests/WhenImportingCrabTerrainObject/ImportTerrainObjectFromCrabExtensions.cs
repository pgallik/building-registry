namespace BuildingRegistry.Tests.WhenImportingCrabTerrainObject
{
    using Be.Vlaanderen.Basisregisters.Crab;
    using Building.Commands.Crab;
    using Building.Events.Crab;

    public static class ImportTerrainObjectFromCrabExtensions
    {
        public static TerrainObjectWasImportedFromCrab ToLegacyEvent(this ImportTerrainObjectFromCrab command)
        {
            return new TerrainObjectWasImportedFromCrab(
                command.TerrainObjectId,
                command.IdentifierTerrainObject,
                command.TerrainObjectNatureCode,
                command.XCoordinate,
                command.YCoordinate,
                command.BuildingNature,
                command.Lifetime,
                command.Timestamp,
                command.Operator,
                command.Modification,
                command.Organisation);
        }

        public static ImportTerrainObjectFromCrab WithModification(this ImportTerrainObjectFromCrab command,
            CrabModification? modification)
        {
            return new ImportTerrainObjectFromCrab(
                command.TerrainObjectId,
                command.IdentifierTerrainObject,
                command.TerrainObjectNatureCode,
                command.XCoordinate,
                command.YCoordinate,
                command.BuildingNature,
                command.Lifetime,
                command.Timestamp,
                command.Operator,
                modification,
                command.Organisation);
        }

        public static ImportTerrainObjectFromCrab WithLifetime(this ImportTerrainObjectFromCrab command, CrabLifetime lifetime)
        {
            return new ImportTerrainObjectFromCrab(
                command.TerrainObjectId,
                command.IdentifierTerrainObject,
                command.TerrainObjectNatureCode,
                command.XCoordinate,
                command.YCoordinate,
                command.BuildingNature,
                lifetime,
                command.Timestamp,
                command.Operator,
                command.Modification,
                command.Organisation);
        }
    }
}
