using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRSketchingGeometry.SketchObjectManagement;

namespace VRSketchingGeometry.Commands.Line
{
    /// <summary>
    /// Populate LineSketchObject with additional control points in order to fill gaps
    /// </summary>
    /// <remarks>Original author: 0545617</remarks>
    public class PopulateLineCommand : ICommand
    {
        /// <summary>
        /// Destination curve, which will be transformed.
        /// </summary>
        private LineSketchObject dCurve;
        /// <summary>
        /// Backup for destination curve control points, which will be used to restore original curve.
        /// </summary>
        private List<Vector3> originalDCurve;
        /// <summary>
        /// Populate gaps with control points while keeping this distance distance. These gaps are defined as distance between two neighbouring control points that exceed this distance
        /// </summary>
        private float minDistance;
        /// <summary>
        /// Command for populating LineSketchObject with additional control points in order to fill gaps
        /// </summary>
        /// <param name="dCurve">The curve that will be transformed.</param>
        /// <param name="minDistance">Should be set bigger than 0. Determines the distance between the generated control points and minimum distance in order for this to happen.</param>
        public PopulateLineCommand(LineSketchObject dCurve, float minDistance)
        {
            this.dCurve = dCurve;
            originalDCurve = dCurve.GetControlPoints();
            this.minDistance = minDistance;

        }

        public bool Execute()
        {
            dCurve.SetControlPoints(fillControlPointGap(dCurve.GetControlPoints(), minDistance));
            return true;
        }

        public void Redo()
        {
            Execute();
        }

        public void Undo()
        {
            dCurve.SetControlPoints(originalDCurve);
        }


        private List<Vector3> fillControlPointGap(List<Vector3> dCurveControlPoints, float minDistance)
        {
            List<Vector3> controlPoints = new List<Vector3>(dCurveControlPoints);
            List<Vector3> gapPoints = new List<Vector3>();


            for (int i = 0; i < controlPoints.Count - 1; i++)
            {
                gapPoints.Add(controlPoints[i]);

                if ((controlPoints[i] - controlPoints[i + 1]).magnitude > minDistance)
                {
                    gapPoints.AddRange(generatePoints(controlPoints[i], controlPoints[i + 1], minDistance));
                }

            }
            gapPoints.Add(controlPoints[controlPoints.Count - 1]);


            return gapPoints;
        }

        private List<Vector3> generatePoints(Vector3 firstPoint, Vector3 secondPoint, float minDistance)
        {
            List<Vector3> generatedPoints = new List<Vector3>();

            float distance = (firstPoint - secondPoint).magnitude / minDistance;
            int roundDistance = Mathf.FloorToInt((firstPoint - secondPoint).magnitude / minDistance);

            for (int k = 1; k < roundDistance; k++)
            {
                Vector3 newPoint = firstPoint + (k * ((secondPoint - firstPoint) / distance));

                generatedPoints.Add(newPoint);
            }

            return generatedPoints;
        }
    }

}
