namespace BuildingRegistry.Api.Legacy.Building.Responses
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Filters;

    [DataContract(Name = "CrabGebouwenCollectie", Namespace = "")]
    public class BuildingCrabMappingResponse
    {
        /// <summary>
        /// Collectie van Crab Gebouwen
        /// </summary>
        [DataMember(Name = "CrabGebouwen", Order = 1)]
        [XmlArrayItem(ElementName = "CrabGebouw")]
        [JsonProperty(Required = Required.DisallowNull)]
        public List<BuildingCrabMappingItem> CrabGebouwen { get; set; }
    }

    [DataContract(Name = "CrabGebouw", Namespace = "")]
    public class BuildingCrabMappingItem
    {
        /// <summary>
        /// De identificator van het gebouw.
        /// </summary>
        [DataMember(Name = "Identificator", Order = 1)]
        [JsonProperty(Required = Required.DisallowNull)]
        public GebouwCrabIdentificator Identificator { get; set; }

        /// <summary>
        /// De TerreinObjectId gekend in CRAB.
        /// </summary>
        [DataMember(Name = "TerreinObjectId", Order = 2)]
        [JsonProperty(Required = Required.DisallowNull)]
        public int TerrainObjectId { get; set; }

        /// <summary>
        /// De IdentificatorTerreinObject gekend in CRAB.
        /// </summary>
        [DataMember(Name = "IdentificatorTerreinObject", Order = 3)]
        public string IdentifierTerrainObject { get; set; }

        public BuildingCrabMappingItem(
            int persistentLocalId,
            int terrainObjectId,
            string identifierTerrainObject)
        {
            Identificator = new GebouwCrabIdentificator(persistentLocalId.ToString(CultureInfo.InvariantCulture));
            TerrainObjectId = terrainObjectId;
            IdentifierTerrainObject = identifierTerrainObject;
        }
    }

    [DataContract(Name = "Identificator", Namespace = "")]
    public class GebouwCrabIdentificator
    {
        /// <summary>
        /// De objectidentificator (enkel uniek binnen naamruimte).
        /// </summary>
        [DataMember(Name = "ObjectId", Order = 3)]
        [JsonProperty(Required = Required.DisallowNull)]
        public string ObjectId { get; set; }

        public GebouwCrabIdentificator(string objectId)
        {
            ObjectId = objectId;
        }
    }

    public class BuildingCrabMappingResponseExamples : IExamplesProvider<BuildingCrabMappingResponse>
    {
        public BuildingCrabMappingResponse GetExamples()
            => new BuildingCrabMappingResponse
            {
                CrabGebouwen = new List<BuildingCrabMappingItem>
                {
                    new BuildingCrabMappingItem(15267, 6, "4897515"),
                    new BuildingCrabMappingItem(987415, 7, string.Empty),
                    new BuildingCrabMappingItem(4845125, 8, "7714587"),
                },
            };
    }
}