namespace BuildingRegistry.Api.BackOffice.Handlers.Lambda.Requests.Building
{
    using System;
    using System.Collections.Generic;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using MediatR;

    public abstract class BuildingLambdaRequest : IRequest
    {
        public Guid TicketId { get; set; }
        public string MessageGroupId { get; set; }
        public string? IfMatchHeaderValue { get; set; }
        public Provenance Provenance { get; set; }
        public IDictionary<string, object> Metadata { get; set; }

        protected BuildingLambdaRequest(
            Guid ticketId,
            string messageGroupId,
            string? ifMatchHeaderValue,
            Provenance provenance,
            IDictionary<string, object> metadata)
        {
            TicketId = ticketId;
            MessageGroupId = messageGroupId;
            IfMatchHeaderValue = ifMatchHeaderValue;
            Provenance = provenance;
            Metadata = metadata;
        }
    }
}
