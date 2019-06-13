namespace BuildingRegistry.Api.Legacy.Infrastructure.Grb
{
    using System.Collections.Generic;
    using System.Linq;
    using GeoAPI.Geometries;

    public class GrbBuildingParcel : IGrbBuildingParcel
    {
        private readonly IGrbWfsClient _wfsClient;

        public GrbBuildingParcel(IGrbWfsClient wfsClient) => _wfsClient = wfsClient;

        public IEnumerable<string> GetUnderlyingParcels(IGeometry buildingGeometry)
        {
            var features = _wfsClient.GetFeaturesInBoundingBox(GrbFeatureType.Parcel, buildingGeometry.EnvelopeInternal);
            var parcels = new Dictionary<string, IGeometry>();
            foreach (var feature in features)
            {
                if (!parcels.ContainsKey(feature.Item2["CAPAKEY"]))
                    parcels.Add(feature.Item2["CAPAKEY"], feature.Item1);
            }

            foreach (var perceel in parcels.ToList())
            {
                if (!buildingGeometry.Intersects(perceel.Value))
                    parcels.Remove(perceel.Key);
            }

            var parcelCount = parcels.Count;

            foreach (var parcelPair in parcels.ToList())
            {
                var sufficientOverlap = buildingGeometry.Intersection(parcelPair.Value).Area / buildingGeometry.Area >= 0.8 / parcelCount;

                if (!sufficientOverlap)
                    parcels.Remove(parcelPair.Key);
            }

            return parcels.Keys;
        }
    }
}
