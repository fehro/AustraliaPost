namespace AustraliaPost.Models
{
    public class Locality
    {
        #region Public Properties

        public string Category { get; set; }

        public int Id { get; set; }

        public string Location { get; set; }

        public string Postcode { get; set; }

        public string State { get; set; }

        public float? Latitude { get; set; }

        public float? Longitude { get; set; }

        #endregion
    }
}
