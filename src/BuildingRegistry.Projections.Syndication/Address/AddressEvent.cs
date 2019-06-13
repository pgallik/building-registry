namespace BuildingRegistry.Projections.Syndication.Address
{
    public enum AddressEvent
    {
        AddressWasRegistered,
        AddressWasRemoved,
        AddressOsloIdWasAssigned,

        AddressBecameIncomplete,
        AddressBecameComplete
    }
}
