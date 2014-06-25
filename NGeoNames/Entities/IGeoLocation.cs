namespace NGeoNames.Entities
{
    /// <summary>
    /// Defines a generalized interface to define a location by latitude/longitude.
    /// </summary>
    public interface IGeoLocation
    {
        /// <summary>
        /// Gets the latitude of the location.
        /// </summary>
        double Latitude { get; set; }

        /// <summary>
        /// Sets the latitude of the location.
        /// </summary>
        double Longitude { get; set; }
    }
}
