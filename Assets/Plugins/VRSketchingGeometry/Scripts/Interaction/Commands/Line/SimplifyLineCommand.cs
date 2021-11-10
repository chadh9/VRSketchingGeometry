using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRSketchingGeometry.SketchObjectManagement;

namespace VRSketchingGeometry.Commands.Line
{
    /// <summary>
    /// Simplify LineSketchObject by utlizing the Ramer-Douglas-Peucker algorithm
    /// </summary>
    /// <remarks>Original author: 0545617</remarks>
    public class SimplifyLineCommand : ICommand
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
        /// The lower this value, the more control points are kept after the simplification.
        /// </summary>
        private float tolerance;
        /// <summary>
        /// Command for simplifying the curve. This leads to smoothing out knots.
        /// </summary>
        /// <param name="dCurve">The curve that will be transformed.</param>
        /// <param name="tolerance">Should be set bigger than 0. Determines how many points are being kept after simplifying. The lower the value, the more points are being kept. </param>
        public SimplifyLineCommand(LineSketchObject dCurve, float tolerance)
        {
            this.dCurve = dCurve;
            originalDCurve = dCurve.GetControlPoints();
            this.tolerance = tolerance;

        }

        public bool Execute()
        {
            dCurve.SetControlPoints(smoothingCurve(dCurve, tolerance));
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


        private List<Vector3> smoothingCurve(LineSketchObject dCurve, float tolerance)
        {

            List<Vector3> controlPoints = dCurve.GetControlPoints();
            List<Vector3> simplifiedCurve = new List<Vector3>();
            LineUtility.Simplify(controlPoints, tolerance, simplifiedCurve);

            return simplifiedCurve;
        }

    }
}
