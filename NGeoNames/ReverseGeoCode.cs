using KdTree.Math;
using NGeoNames.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NGeoNames
{
    public class ReverseGeoCode<T>
        where T : GeoName, new()
    {
        public double[] GetCoord(GeoName n)
        {
            const double R = 6371000;   //Radius of the earth, in METERS
            return new[] {
                R * Math.Cos(GeoUtil.Deg2Rad(n.Latitude)) * Math.Cos(GeoUtil.Deg2Rad(n.Longitude)),
                R * Math.Cos(GeoUtil.Deg2Rad(n.Latitude)) * Math.Sin(GeoUtil.Deg2Rad(n.Longitude)),
                R * Math.Sin(GeoUtil.Deg2Rad(n.Latitude))
            };
        }

        private KdTree.KdTree<double, T> _tree;

        public ReverseGeoCode()
            : this(Enumerable.Empty<T>()) { }

        public ReverseGeoCode(IEnumerable<T> geonames)
        {
            _tree = new KdTree.KdTree<double, T>(3, new DoubleMath());
            this.AddRange(geonames);

            this.Balance();
        }

        public void Add(T node)
        {
            _tree.Add(GetCoord(node), node);
        }

        public void AddRange(IEnumerable<T> nodes)
        {
            foreach (var i in nodes)
                this.Add(i);
        }

        public void Balance()
        {
            _tree.Balance();
        }

        public IEnumerable<T> RadialSearch(double lat, double lng, int maxcount)
        {
            return this.RadialSearch(LatLonToDummyGeo(lat, lng), maxcount);
        }

        public IEnumerable<T> RadialSearch(T center, int maxcount)
        {
            return this.RadialSearch(center, double.MaxValue, maxcount);
        }

        public IEnumerable<T> RadialSearch(double lat, double lng, double radius)
        {
            return this.RadialSearch(LatLonToDummyGeo(lat, lng), radius);
        }

        public IEnumerable<T> RadialSearch(T center, double radius)
        {
            return this.RadialSearch(center, radius, _tree.Count);
        }

        public IEnumerable<T> RadialSearch(double lat, double lng, double radius, int maxcount)
        {
            return this.RadialSearch(LatLonToDummyGeo(lat, lng), radius, maxcount);
        }

        public IEnumerable<T> RadialSearch(T center, double radius, int maxcount)
        {
            return _tree.RadialSearch(GetCoord(center), radius, maxcount).Select(v => v.Value);
        }

        public IEnumerable<T> NearestNeighbourSearch(double lat, double lng)
        {
            return this.NearestNeighbourSearch(LatLonToDummyGeo(lat, lng), _tree.Count);
        }

        public IEnumerable<T> NearestNeighbourSearch(double lat, double lng, int maxcount)
        {
            return this.NearestNeighbourSearch(LatLonToDummyGeo(lat, lng), maxcount);
        }

        public IEnumerable<T> NearestNeighbourSearch(T center)
        {
            return this.NearestNeighbourSearch(center, _tree.Count);
        }

        public IEnumerable<T> NearestNeighbourSearch(T center, int maxcount)
        {
            return _tree.GetNearestNeighbours(GetCoord(center), maxcount).Select(v => v.Value);
        }

        private static T LatLonToDummyGeo(double lat, double lng) {
            return new T()
            {
                Latitude = lat,
                Longitude = lng,
                Name = null,
                Id = 0
            };
        }

        private class DoubleMath : KdTree.Math.TypeMath<double>
        {
            static DoubleMath()
            {
                TypeMath<double>.Register(new DoubleMath());
            }

            public override double Add(double a, double b)
            {
                return a + b;
            }

            public override bool AreEqual(double a, double b)
            {
                return a == b;
            }

            public override int Compare(double a, double b)
            {
                return a.CompareTo(b);
            }

            public override double MaxValue
            {
                get { return double.MaxValue; }
            }

            public override double MinValue
            {
                get { return double.MinValue; }
            }

            public override double Multiply(double a, double b)
            {
                return a * b;
            }

            public override double NegativeInfinity
            {
                get { return double.NegativeInfinity; }
            }

            public override double PositiveInfinity
            {
                get { return double.PositiveInfinity; }
            }

            public override double Subtract(double a, double b)
            {
                return a - b;
            }

            public override double Zero
            {
                get { return 0; }
            }
        }
    }
}
