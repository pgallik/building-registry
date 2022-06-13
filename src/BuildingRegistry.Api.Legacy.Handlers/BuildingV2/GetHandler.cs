namespace BuildingRegistry.Api.Legacy.Handlers.BuildingV2
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions.Converters;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Be.Vlaanderen.Basisregisters.GrAr.Common;
    using Be.Vlaanderen.Basisregisters.GrAr.Legacy.Gebouw;
    using BuildingRegistry.Api.Legacy.Abstractions.Building;
    using BuildingRegistry.Api.Legacy.Abstractions.Building.Responses;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class GetHandler : IRequestHandler<GetRequest, BuildingResponse>
    {
        public async Task<BuildingResponse> Handle(GetRequest request, CancellationToken cancellationToken)
        {
            var building = await request.Context
                .BuildingDetailsV2
                .AsNoTracking()
                .SingleOrDefaultAsync(item => item.PersistentLocalId == request.PersistentLocalId, cancellationToken);

            if (building is { IsRemoved: true })
            {
                throw new ApiException("Gebouw werd verwijderd.", StatusCodes.Status410Gone);
            }

            if (building is null)
            {
                throw new ApiException("Onbestaand gebouw.", StatusCodes.Status404NotFound);
            }

            //TODO: improvement getting buildingunits and parcels in parallel.
            var buildingUnitPersistentLocalIds = await request.Context
                .BuildingUnitDetailsV2
                .Where(x => x.BuildingPersistentLocalId == building.PersistentLocalId)
                .Where(x => !x.IsRemoved)
                .Select(x => x.BuildingUnitPersistentLocalId)
                .ToListAsync(cancellationToken);

            var parcels = request.GrbBuildingParcel
                .GetUnderlyingParcels(building.Geometry)
                .Select(s => CaPaKey.CreateFrom(s).VbrCaPaKey)
                .Distinct();

            var caPaKeys = await request.SyndicationContext
                .BuildingParcelLatestItems
                .Where(x => !x.IsRemoved &&
                            parcels.Contains(x.CaPaKey))
                .Select(x => x.CaPaKey)
                .ToListAsync(cancellationToken);

            return new BuildingResponse(
                building.PersistentLocalId,
                request.ResponseOptions.Value.GebouwNaamruimte,
                building.Version.ToBelgianDateTimeOffset(),
                BuildingHelpers.GetBuildingPolygon(building.Geometry),
                building.GeometryMethod.Map(),
                building.Status.Map(),
                buildingUnitPersistentLocalIds
                    .OrderBy(x => x)
                    .Select(x =>
                        new GebouwDetailGebouweenheid(
                            x.ToString(),
                            string.Format(request.ResponseOptions.Value.GebouweenheidDetailUrl, x)))
                    .ToList(),
                caPaKeys.Select(x => new GebouwDetailPerceel(x, string.Format(request.ResponseOptions.Value.PerceelUrl, x))).ToList());
        }
    }
}
