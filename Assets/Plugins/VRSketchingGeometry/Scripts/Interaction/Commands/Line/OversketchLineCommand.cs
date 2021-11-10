using System.Collections.Generic;
using UnityEngine;
using VRSketchingGeometry.SketchObjectManagement;

namespace VRSketchingGeometry.Commands.Line
{
    /// <summary>
    /// Transform LineSKetchObject by approaching another LinesketchObject
    /// </summary>
    /// <remarks>Original author: 0545617</remarks>
    public class OverSketchLineCommand : ICommand
    {
        /// <summary>
        /// Destination curve, which will be transformed.
        /// </summary>
        private LineSketchObject dCurve;
        /// <summary>
        /// Oversketching curve, which will be approached.
        /// </summary>
        private LineSketchObject oCurve;
        /// <summary>
        /// Backup for destination curve control points, which will be used to restore original curve.
        /// </summary>
        private List<Vector3> originalDCurve;
        /// <summary>
        /// Determines how strong the attraction will be. The lower the value the greater the transformation.
        /// </summary>
        private float distanceScaling;
        /// In order for a destination control point to be modified by an Oversketching control point, their distance has to be below this value.
        private float affectedRange;
        /// <summary>
        /// Command for transforming a curve by the means of oversketching.
        /// </summary>
        /// <param name="dCurve">The curve that will be transformed.</param>
        /// <param name="oCurve">The curve that serves as reference for transformation.</param>
        /// <param name="distanceScaling">Should be set bigger than 1. Determines how strong the attraction will be. The more the value approaches 0, the stronger the attraction.</param>
        /// <param name="affectedRange">Should be set bigger than 0. Oversketching control points and destination control points need to have their distances below this value in order to interact. </param>
        public OverSketchLineCommand(LineSketchObject dCurve, LineSketchObject oCurve, float distanceScaling, float affectedRange)
        {
            this.dCurve = dCurve;
            this.oCurve = oCurve;
            originalDCurve = dCurve.GetControlPoints();
            this.distanceScaling = distanceScaling;
            this.affectedRange = affectedRange;
        }

        public bool Execute()
        {
            dCurve.SetControlPoints(calculateScaledCurve(distanceScaling, affectedRange, oCurve, dCurve));
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


        private List<Vector3> calculateScaledCurve(float weight, float range, LineSketchObject oCurve, LineSketchObject dCurve)
        {
            List<Vector3> list = new List<Vector3>();

            for (int i = 0; i < dCurve.getNumberOfControlPoints(); i++)
            {
                list.Add(new Vector3());

                for (int j = 0; j < oCurve.GetControlPoints().Count; j++)
                {

                    if ((oCurve.GetControlPoints()[j] - dCurve.GetControlPoints()[i]).magnitude <= range)
                    {
                        //sqrMagnitude can be used instead of magnitude to improve performance
                        list[i] += ((oCurve.GetControlPoints()[j] - dCurve.GetControlPoints()[i]) / (Mathf.Pow(weight,(oCurve.GetControlPoints()[j] - dCurve.GetControlPoints()[i]).magnitude)));
                    }
                }
                list[i] = dCurve.GetControlPoints()[i] + (list[i] / oCurve.getNumberOfControlPoints());
            }

            return list;
        }

    }

}
