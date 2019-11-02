using System.Collections.Generic;

namespace PathScheduler.Helpers
{
    /// <summary>
    /// Represents point data source.
    /// </summary>
    public abstract class PointDataSource<T>
    {
        /// <summary>
        /// List of points.
        /// </summary>
        protected List<T> _pointList;

        /// <summary>
        /// Distance matrix.
        /// </summary>
        protected double[,] _distanceMatrix;

        /// <summary>
        /// List of points accessor.
        /// </summary>
        public List<T> Points { get { return _pointList; } }

        /// <summary>
        /// Distance matrix accessor.
        /// </summary>
        public double[,] DistanceMatrix { get { return _distanceMatrix; } }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PointDataSource() {
            this._pointList = new List<T>();
            this.GenerateDistanceMatrix();
        }

        /// <summary>
        /// Adds a point and regenerates the distance matrix.
        /// </summary>
        /// <param name="point"></param>
        public void AddPoint(T point) {
            this._pointList.Add(point);
            this.OnPointAdd(point);
            this.GenerateDistanceMatrix();
        }

        /// <summary>
        /// Adds a new point to the point list.
        /// </summary>
        /// <param name="point"></param>
        protected virtual void OnPointAdd(T point) { }

        /// <summary>
        /// Generates distance matrix from the point list.
        /// </summary>
        protected virtual void GenerateDistanceMatrix() { }
    }
}
