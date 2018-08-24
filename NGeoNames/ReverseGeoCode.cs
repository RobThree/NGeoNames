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
        where T : IGeoLocation, new()
    {
        private KdTree.KdTree<double, T> _tree;

        /// <summary>
        /// Initializes a new, empty <see cref="ReverseGeoCode{T}"/> object.
        /// </summary>
        public ReverseGeoCode()
            : this(Enumerable.Empty<T>()) { }

        /// <summary>
        /// Initializes a new <see cref="ReverseGeoCode{T}"/> object, populating the internal structures with the provided
        /// <see cref="IGeoLocation"/>s.
        /// </summary>
        /// <param name="nodes">
        /// An IEnumerable{T} of <see cref="IGeoLocation"/> to populate the <see cref="ReverseGeoCode{T}"/> with.
        /// </param>
        /// <remarks>
        /// This constructor will, internally, call the <see cref="Balance"/> method when the internal structure is initialized.
        /// </remarks>
        public ReverseGeoCode(IEnumerable<T> nodes)
        {
            _tree = new KdTree.KdTree<double, T>(3, new DoubleMath());
            AddRange(nodes);

            Balance();
        }

        /// <summary>
        /// Adds a node to the <see cref="ReverseGeoCode{T}"/> internal structure.
        /// </summary>
        /// <param name="node">The <see cref="IGeoLocation"/> to add.</param>
        public void Add(T node)
        {
            _tree.Add(GeoUtil.GetCoord(node), node);
        }


        /// <summary>
        /// Adds the specified nodes to the <see cref="ReverseGeoCode{T}"/> internal structure.
        /// </summary>
        /// <param name="nodes">An IEnumerable{T} of <see cref="IGeoLocation"/>s to add.</param>
        public void AddRange(IEnumerable<T> nodes)
        {
            foreach (var i in nodes)
                Add(i);
        }

        /// <summary>
        /// Balance the internal structure (KD-tree).
        /// </summary>
        /// <remarks>
        /// This method only needs to be called when all nodes have been added; it *can* be called earlier but is
        /// not of much use. Note that the <see cref="ReverseGeoCode(IEnumerable{T})"/> calls this
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
            return RadialSearch(CreateFromLatLong(lat, lng), maxcount);
        }

        /// <summary>
        /// Performs a radial search on the nodes from a specified center limiting the number of results.
        /// </summary>
        /// <param name="center">The center of the search.</param>
        /// <param name="maxcount">The maximum number of results to return.</param>
        /// <returns>Returns up to maxcount nodes matching the radial search.</returns>
        public IEnumerable<T> RadialSearch(T center, int maxcount)
        {
            return RadialSearch(center, double.MaxValue, maxcount);
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
            return RadialSearch(CreateFromLatLong(lat, lng), radius);
        }

        /// <summary>
        /// Performs a radial search on the nodes from a specified center within a given radius.
        /// </summary>
        /// <param name="center">The center of the search.</param>
        /// <param name="radius">The radius to search in.</param>
        /// <returns>Returns the nodes matching the radial search.</returns>
        public IEnumerable<T> RadialSearch(T center, double radius)
        {
            return RadialSearch(center, radius, _tree.Count);
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
            return RadialSearch(CreateFromLatLong(lat, lng), radius, maxcount);
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
            return NearestNeighbourSearch(CreateFromLatLong(lat, lng), _tree.Count);
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
            return NearestNeighbourSearch(CreateFromLatLong(lat, lng), maxcount);
        }

        /// <summary>
        /// Performs a nearest neighbour search on the nodes from a specified center.
        /// </summary>
        /// <param name="center">The center of the search.</param>
        /// <returns>Returns nodes in matching order of the nearest neighbour search.</returns>
        public IEnumerable<T> NearestNeighbourSearch(T center)
        {
            return NearestNeighbourSearch(center, _tree.Count);
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
        /// Creates a <see cref="IGeoLocation"/> from a latitude and longitude.
        /// </summary>
        /// <param name="lat">The latitude of the object.</param>
        /// <param name="lng">The longitude of the object.</param>
        /// <returns>Returns a <see cref="GeoName"/>, <see cref="ExtendedGeoName"/> or derived.</returns>
        public T CreateFromLatLong(double lat, double lng)
        {
            return new T()
            {
                Latitude = lat,
                Longitude = lng
            };
        }
    }

    /// <summary>
    /// This class is for internal use (Kd-Tree) only.
    /// </summary>
    internal class DoubleMath : KdTree.Math.TypeMath<double>
    {
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
        public override double DistanceSquaredBetweenPoints(double[] a, double[] b)
        {
            double distance = Zero;
            int dimensions = a.Length;

            // Return the absolute distance between 2 hyper points
            for (var dimension = 0; dimension < dimensions; dimension++)
            {
                double distOnThisAxis = Subtract(a[dimension], b[dimension]);
                double distOnThisAxisSquared = Multiply(distOnThisAxis, distOnThisAxis);

                distance = Add(distance, distOnThisAxisSquared);
            }

            return distance;
        }
    }
}
