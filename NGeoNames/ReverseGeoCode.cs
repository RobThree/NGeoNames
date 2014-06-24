using KdTree.Math;
using NGeoNames.Entities;
using System.Collections.Generic;
using System.Linq;

namespace NGeoNames
{
    /// <summary>
    /// Provides reverse geocoding methods.
    /// </summary>
    /// <typeparam name="T">The type of elements in ReverseGeoCoder.</typeparam>
    public class ReverseGeoCode<T>
        where T : GeoName, new()
    {
        private KdTree.KdTree<double, T> _tree;

        /// <summary>
        /// Initializes a new, empty <see cref="ReverseGeoCode&lt;T&gt;"/> object.
        /// </summary>
        public ReverseGeoCode()
            : this(Enumerable.Empty<T>()) { }

        /// <summary>
        /// Initializes a new <see cref="ReverseGeoCode&lt;T&gt;"/> object, populating the internal structures with the provided
        /// <see cref="GeoName"/>s, <see cref="ExtendedGeoName"/>s or derived.
        /// </summary>
        /// <param name="nodes">
        /// An IEnumerable&lt;T&gt; of <see cref="GeoName"/> (or derived) to populate the <see cref="ReverseGeoCode&lt;T&gt;"/> with.
        /// </param>
        /// <remarks>
        /// This constructor will, internally, call the <see cref="Balance"/> method when the internal structure is initialized.
        /// </remarks>
        public ReverseGeoCode(IEnumerable<T> nodes)
        {
            _tree = new KdTree.KdTree<double, T>(3, new DoubleMath());
            this.AddRange(nodes);

            this.Balance();
        }

        /// <summary>
        /// Adds a node to the <see cref="ReverseGeoCode&lt;T&gt;"/> internal structure.
        /// </summary>
        /// <param name="node">The <see cref="GeoName"/> (or derived) to add.</param>
        public void Add(T node)
        {
            _tree.Add(GeoUtil.GetCoord(node), node);
        }


        /// <summary>
        /// Adds the specified nodes to the <see cref="ReverseGeoCode&lt;T&gt;"/> internal structure.
        /// </summary>
        /// <param name="nodes">An IEnumerable&lt;T&gt; of <see cref="GeoName"/>s (or derived) to add.</param>
        public void AddRange(IEnumerable<T> nodes)
        {
            foreach (var i in nodes)
                this.Add(i);
        }

        /// <summary>
        /// Balance the internal structure (KD-tree).
        /// </summary>
        /// <remarks>
        /// This method only needs to be called when all nodes have been added; it *can* be called earlier but is
        /// not of much use. Note that the <see cref="ReverseGeoCode(IEnumerable&lt;T&gt;)"/> calls this
        /// method automatically; no need to call it when using this constructor.
        /// </remarks>
        public void Balance()
        {
            if (_tree.Count > 0)
                _tree.Balance();
        }

        /// <summary>
        /// Performs a radial search on the nodes from a specified center limiting the number of results.
        /// </summary>
        /// <param name="lat">The latitude of the search center.</param>
        /// <param name="lng">The longitude of the search center.</param>
        /// <param name="maxcount">The maximum number of results to return.</param>
        /// <returns>Returns up to maxcount nodes matching the radial search.</returns>
        public IEnumerable<T> RadialSearch(double lat, double lng, int maxcount)
        {
            return this.RadialSearch(CreateFromLatLong(lat, lng), maxcount);
        }

        /// <summary>
        /// Performs a radial search on the nodes from a specified center limiting the number of results.
        /// </summary>
        /// <param name="center">The center of the search.</param>
        /// <param name="maxcount">The maximum number of results to return.</param>
        /// <returns>Returns up to maxcount nodes matching the radial search.</returns>
        public IEnumerable<T> RadialSearch(T center, int maxcount)
        {
            return this.RadialSearch(center, double.MaxValue, maxcount);
        }

        /// <summary>
        /// Performs a radial search on the nodes from a specified center within a given radius.
        /// </summary>
        /// <param name="lat">The latitude of the search center.</param>
        /// <param name="lng">The longitude of the search center.</param>
        /// <param name="radius">The radius to search in.</param>
        /// <returns>Returns the nodes matching the radial search.</returns>
        public IEnumerable<T> RadialSearch(double lat, double lng, double radius)
        {
            return this.RadialSearch(CreateFromLatLong(lat, lng), radius);
        }

        /// <summary>
        /// Performs a radial search on the nodes from a specified center within a given radius.
        /// </summary>
        /// <param name="center">The center of the search.</param>
        /// <param name="radius">The radius to search in.</param>
        /// <returns>Returns the nodes matching the radial search.</returns>
        public IEnumerable<T> RadialSearch(T center, double radius)
        {
            return this.RadialSearch(center, radius, _tree.Count);
        }

        /// <summary>
        /// Performs a radial search on the nodes from a specified center within a given radius limiting the number of
        /// results.
        /// </summary>
        /// <param name="lat">The latitude of the search center.</param>
        /// <param name="lng">The longitude of the search center.</param>
        /// <param name="radius">The radius to search in.</param>
        /// <param name="maxcount">The maximum number of results to return.</param>
        /// <returns>Returns up to maxcount nodes matching the radial search.</returns>
        public IEnumerable<T> RadialSearch(double lat, double lng, double radius, int maxcount)
        {
            return this.RadialSearch(CreateFromLatLong(lat, lng), radius, maxcount);
        }

        /// <summary>
        /// Performs a radial search on the nodes from a specified center within a given radius limiting the number of
        /// results.
        /// </summary>
        /// <param name="center">The center of the search.</param>
        /// <param name="radius">The radius to search in.</param>
        /// <param name="maxcount">The maximum number of results to return.</param>
        /// <returns>Returns up to maxcount nodes matching the radial search.</returns>
        public IEnumerable<T> RadialSearch(T center, double radius, int maxcount)
        {
            return _tree.RadialSearch(GeoUtil.GetCoord(center), radius, maxcount).Select(v => v.Value);
        }

        /// <summary>
        /// Performs a nearest neighbour search on the nodes from a specified center.
        /// </summary>
        /// <param name="lat">The latitude of the search center.</param>
        /// <param name="lng">The longitude of the search center.</param>
        /// <returns>Returns nodes in matching order of the nearest neighbour search.</returns>
        public IEnumerable<T> NearestNeighbourSearch(double lat, double lng)
        {
            return this.NearestNeighbourSearch(CreateFromLatLong(lat, lng), _tree.Count);
        }

        /// <summary>
        /// Performs a nearest neighbour search on the nodes from a specified center limiting the number of results.
        /// </summary>
        /// <param name="lat">The latitude of the search center.</param>
        /// <param name="lng">The longitude of the search center.</param>
        /// <param name="maxcount">The maximum number of results to return.</param>
        /// <returns>Returns up to maxcount nodes in matching order of the nearest neighbour search.</returns>
        public IEnumerable<T> NearestNeighbourSearch(double lat, double lng, int maxcount)
        {
            return this.NearestNeighbourSearch(CreateFromLatLong(lat, lng), maxcount);
        }

        /// <summary>
        /// Performs a nearest neighbour search on the nodes from a specified center.
        /// </summary>
        /// <param name="center">The center of the search.</param>
        /// <returns>Returns nodes in matching order of the nearest neighbour search.</returns>
        public IEnumerable<T> NearestNeighbourSearch(T center)
        {
            return this.NearestNeighbourSearch(center, _tree.Count);
        }

        /// <summary>
        /// Performs a nearest neighbour search on the nodes from a specified center limiting the number of results.
        /// </summary>
        /// <param name="center">The center of the search.</param>
        /// <param name="maxcount">The maximum number of results to return.</param>
        /// <returns>Returns up to maxcount nodes in matching order of the nearest neighbour search.</returns>
        public IEnumerable<T> NearestNeighbourSearch(T center, int maxcount)
        {
            return _tree.GetNearestNeighbours(GeoUtil.GetCoord(center), maxcount).Select(v => v.Value);
        }

        /// <summary>
        /// Creates a <see cref="GeoName"/>, <see cref="ExtendedGeoName"/> or derived from a latitude and longitude.
        /// </summary>
        /// <param name="lat">The latitude of the object.</param>
        /// <param name="lng">The longitude of the object.</param>
        /// <returns>Returns a <see cref="GeoName"/>, <see cref="ExtendedGeoName"/> or derived.</returns>
        public T CreateFromLatLong(double lat, double lng)
        {
            return this.CreateFromLatLong(lat, lng, 0);
        }

        /// <summary>
        /// Creates a <see cref="GeoName"/>, <see cref="ExtendedGeoName"/> or derived from a latitude and longitude.
        /// </summary>
        /// <param name="lat">The latitude of the object.</param>
        /// <param name="lng">The longitude of the object.</param>
        /// <param name="id">The geoname database Id of the object.</param>
        /// <returns>Returns a <see cref="GeoName"/>, <see cref="ExtendedGeoName"/> or derived.</returns>        
        public T CreateFromLatLong(double lat, double lng, int id)
        {
            return this.CreateFromLatLong(lat, lng, id, null);
        }

        /// <summary>
        /// Creates a <see cref="GeoName"/>, <see cref="ExtendedGeoName"/> or derived from a latitude and longitude.
        /// </summary>
        /// <param name="lat">The latitude of the object.</param>
        /// <param name="lng">The longitude of the object.</param>
        /// <param name="name">The name of the object.</param>
        /// <returns>Returns a <see cref="GeoName"/>, <see cref="ExtendedGeoName"/> or derived.</returns>
        public T CreateFromLatLong(double lat, double lng, string name)
        {
            return this.CreateFromLatLong(lat, lng, 0, name);
        }

        /// <summary>
        /// Creates a <see cref="GeoName"/>, <see cref="ExtendedGeoName"/> or derived from a latitude and longitude.
        /// </summary>
        /// <param name="lat">The latitude of the object.</param>
        /// <param name="lng">The longitude of the object.</param>
        /// <param name="id">The geoname database Id of the object.</param>
        /// <param name="name">The name of the object.</param>
        /// <returns>Returns a <see cref="GeoName"/>, <see cref="ExtendedGeoName"/> or derived.</returns>
        public T CreateFromLatLong(double lat, double lng, int id, string name)
        {
            return new T()
            {
                Latitude = lat,
                Longitude = lng,
                Name = name,
                Id = id
            };
        }

        /// <summary>
        /// This class is for internal use (Kd-Tree) only.
        /// </summary>
        private class DoubleMath : KdTree.Math.TypeMath<double>
        {
            static DoubleMath()
            {
                // Make sure we register ourselves!
                TypeMath<double>.Register(new DoubleMath());
            }

            public override double Add(double a, double b) { return a + b; }
            public override bool AreEqual(double a, double b) { return a == b; }
            public override int Compare(double a, double b) { return a.CompareTo(b); }
            public override double MaxValue { get { return double.MaxValue; } }
            public override double MinValue { get { return double.MinValue; } }
            public override double Multiply(double a, double b) { return a * b; }
            public override double NegativeInfinity { get { return double.NegativeInfinity; } }
            public override double PositiveInfinity { get { return double.PositiveInfinity; } }
            public override double Subtract(double a, double b) { return a - b; }
            public override double Zero { get { return 0; } }
        }
    }
}
