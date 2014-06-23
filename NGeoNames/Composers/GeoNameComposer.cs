using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    public class GeoNameComposer : BaseComposer<GeoName>
    {
        public bool UseExtendedFileFormat { get; set; }

        public GeoNameComposer()
            : this(true) { }

        public GeoNameComposer(bool useextendedfileformat)
        {
            this.UseExtendedFileFormat = useextendedfileformat;
        }

        public override string Compose(GeoName value)
        {
            if (this.UseExtendedFileFormat)
            {
                return string.Join(this.FieldSeparator.ToString(), value.Id, value.Name, null, null, DoubleToString(value.Latitude), DoubleToString(value.Longitude),
                    null, null, null, null, null, null, null, null, null, null, null, null, null);
            }
            else
            {
                return string.Join(this.FieldSeparator.ToString(), value.Id, value.Name, DoubleToString(value.Latitude), DoubleToString(value.Longitude));
            }
        }
    }
}
