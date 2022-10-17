namespace BuildingRegistry.Api.BackOffice.Handlers.Sqs.Requests.Building
{
    using Abstractions;
    using Abstractions.Building.Requests;

    public sealed class CorrectBuildingNotRealizationSqsRequest : SqsRequest, IHasBackOfficeRequest<BackOfficeCorrectBuildingNotRealizationRequest>
    {
        public BackOfficeCorrectBuildingNotRealizationRequest Request { get; set; }
    }
}