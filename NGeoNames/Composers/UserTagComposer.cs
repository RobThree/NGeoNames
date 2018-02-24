using NGeoNames.Entities;

namespace NGeoNames.Composers
{
    /// <summary>
    /// Provides methods for composing a string representing a <see cref="UserTag"/>.
    /// </summary>
    public class UserTagComposer : BaseComposer<UserTag>
    {
        /// <summary>
        /// Composes the specified <see cref="UserTag"/> into a string.
        /// </summary>
        /// <param name="value">The <see cref="UserTag"/> to be composed into a string.</param>
        /// <returns>A string representing the specified <see cref="UserTag"/>.</returns>
        public override string Compose(UserTag value)
        {
            return string.Join(FieldSeparator.ToString(), value.GeoNameId, value.Tag);
        }
    }
}
