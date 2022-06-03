namespace BuildingRegistry.Api.Oslo.Handlers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions.Building.Query;
    using Abstractions.Building.Responses;
    using Abstractions.Infrastructure.Options;
    using Be.Vlaanderen.Basisregisters.Api.Search;
    using Be.Vlaanderen.Basisregisters.Api.Search.Filtering;
    using Be.Vlaanderen.Basisregisters.Api.Search.Pagination;
    using Be.Vlaanderen.Basisregisters.Api.Search.Sorting;
    using Be.Vlaanderen.Basisregisters.GrAr.Common;
    using BuildingRegistry.Projections.Legacy;
    using Legacy.Abstractions.Infrastructure;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public record GetListRequest(HttpRequest HttpRequest, HttpResponse HttpResponse, LegacyContext Context, IOptions<ResponseOptions> ResponseOptions) : IRequest<BuildingListOsloResponse>;

    public class GetListHandler : IRequestHandler<GetListRequest, BuildingListOsloResponse>
    {
        public async Task<BuildingListOsloResponse> Handle(GetListRequest request, CancellationToken cancellationToken)
        {
            var filtering = request.HttpRequest.ExtractFilteringRequest<BuildingFilter>();
            var sorting = request.HttpRequest.ExtractSortingRequest();
            var pagination = request.HttpRequest.ExtractPaginationRequest();

            var pagedBuildings = new BuildingListOsloQuery(request.Context)
                .Fetch(filtering, sorting, pagination);

            request.HttpResponse.AddPagedQueryResultHeaders(pagedBuildings);

            var buildings = await pagedBuildings.Items
                .Select(a => new
                {
                    a.PersistentLocalId,
                    a.Version,
                    a.Status
                })
                .ToListAsync(cancellationToken);

            return new BuildingListOsloResponse
            {
                Gebouwen = buildings
                    .Select(x => new GebouwCollectieItemOslo(
                        x.PersistentLocalId.Value,
                        request.ResponseOptions.Value.GebouwNaamruimte,
                        request.ResponseOptions.Value.GebouwDetailUrl,
                        x.Status.Value.MapBuildingStatus(),
                        x.Version.ToBelgianDateTimeOffset()))
                    .ToList(),
                Volgende = pagedBuildings.PaginationInfo.BuildNextUri(request.ResponseOptions.Value.GebouwVolgendeUrl),
                Context = request.ResponseOptions.Value.ContextUrlList
            };
        }
    }
}