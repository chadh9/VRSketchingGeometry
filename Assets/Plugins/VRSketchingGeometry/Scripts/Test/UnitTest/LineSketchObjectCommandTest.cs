using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VRSketchingGeometry.SketchObjectManagement;
using VRSketchingGeometry.Commands;
using VRSketchingGeometry.Commands.Line;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class LineSketchObjectCommandsTest
    {
        private LineSketchObject LineSketchObject;
        private LineSketchObject OverSketchObject;
        private CommandInvoker Invoker;

        [UnitySetUp]
        public IEnumerator SetUpScene()
        {
            yield return SceneManager.LoadSceneAsync("CommandTestScene", LoadSceneMode.Single);
            this.LineSketchObject = GameObject.FindObjectsOfType<LineSketchObject>()[0];
            this.OverSketchObject = GameObject.FindObjectsOfType<LineSketchObject>()[1];
            yield return null;
            Invoker = new CommandInvoker();
        }

        [Test]
        public void AddOneControlPointToLineWithCommand()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);
            Assert.IsTrue(this.LineSketchObject.GetControlPoints()[0] == new Vector3(1, 2, 3));
        }

        [Test]
        public void AddOneControlPointToLineWithCommandRedo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);
            Invoker.Undo();
            Invoker.Redo();

            Assert.IsTrue(this.LineSketchObject.GetControlPoints()[0] == new Vector3(1, 2, 3));
            Assert.IsFalse(SketchWorld.ActiveSketchWorld.IsObjectDeleted(this.LineSketchObject));
        }

        [Test]
        public void AddOneControlPointToLineWithCommandUndo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);
            Invoker.Undo();

            Assert.AreEqual(this.LineSketchObject.getNumberOfControlPoints(), 0);
            Assert.IsTrue(SketchWorld.ActiveSketchWorld.IsObjectDeleted(this.LineSketchObject));
        }

        [Test]
        public void DeleteOneControlPointWithCommand()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            DeleteControlPointCommand deleteCommand = new DeleteControlPointCommand(this.LineSketchObject);
            Invoker.ExecuteCommand(deleteCommand);
            Assert.AreEqual(this.LineSketchObject.getNumberOfControlPoints(), 0);
            Assert.IsTrue(SketchWorld.ActiveSketchWorld.IsObjectDeleted(this.LineSketchObject));
        }

        [Test]
        public void DeleteOneControlPointWithCommandUndo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            DeleteControlPointCommand deleteCommand = new DeleteControlPointCommand(this.LineSketchObject);
            Invoker.ExecuteCommand(deleteCommand);
            Invoker.Undo();

            Assert.AreEqual(this.LineSketchObject.getNumberOfControlPoints(), 1);
            Assert.IsFalse(SketchWorld.ActiveSketchWorld.IsObjectDeleted(this.LineSketchObject));
        }

        [Test]
        public void DeleteOneControlPointWithCommandRedo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            DeleteControlPointCommand deleteCommand = new DeleteControlPointCommand(this.LineSketchObject);
            Invoker.ExecuteCommand(deleteCommand);
            Invoker.Undo();
            Invoker.Redo();

            Assert.AreEqual(this.LineSketchObject.getNumberOfControlPoints(), 0);
            Assert.IsTrue(SketchWorld.ActiveSketchWorld.IsObjectDeleted(this.LineSketchObject));
        }

        [Test]
        public void AddMultipleControlPointsWithCommand()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);

            Assert.AreEqual(this.LineSketchObject.getNumberOfControlPoints(), 4);
            Assert.AreEqual((3 * 20 + 2) * 7, this.LineSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void AddMultipleControlPointsWithCommandUndo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);
            Invoker.Undo();

            Assert.AreEqual(this.LineSketchObject.getNumberOfControlPoints(), 3);
            Assert.AreEqual((2 * 20 + 2) * 7, this.LineSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void AddMultipleControlPointsWithCommandRedo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);
            Invoker.Undo();
            Invoker.Redo();

            Assert.AreEqual(this.LineSketchObject.getNumberOfControlPoints(), 4);
            Assert.AreEqual((3 * 20 + 2) * 7, this.LineSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void AddMultipleControlPointsAndDeleteOneWithCommand()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);

            ICommand deleteCommand = new DeleteControlPointCommand(this.LineSketchObject);
            Invoker.ExecuteCommand(deleteCommand);

            Assert.AreEqual(this.LineSketchObject.getNumberOfControlPoints(), 3);
            Assert.AreEqual((2 * 20 + 2) * 7, this.LineSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void AddMultipleControlPointsAndDeleteOneWithCommandUndo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);

            ICommand deleteCommand = new DeleteControlPointCommand(this.LineSketchObject);
            Invoker.ExecuteCommand(deleteCommand);
            Invoker.Undo();

            Assert.AreEqual(this.LineSketchObject.getNumberOfControlPoints(), 4);
            Assert.AreEqual((3 * 20 + 2) * 7, this.LineSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void AddMultipleControlPointsAndDeleteOneWithCommandRedo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);

            ICommand deleteCommand = new DeleteControlPointCommand(this.LineSketchObject);
            Invoker.ExecuteCommand(deleteCommand);
            Invoker.Undo();
            Invoker.Redo();

            Assert.AreEqual(this.LineSketchObject.getNumberOfControlPoints(), 3);
            Assert.AreEqual((2 * 20 + 2) * 7, this.LineSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void AddControlPointContinuousCommand()
        {
            ICommand addControlPointCommand = new AddControlPointContinuousCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(addControlPointCommand);

            addControlPointCommand = new AddControlPointContinuousCommand(this.LineSketchObject, new Vector3(1, 3.001f, 3));
            Invoker.ExecuteCommand(addControlPointCommand);

            Assert.AreEqual(2, this.LineSketchObject.getNumberOfControlPoints());
        }

        [Test]
        public void AddControlPointContinuousCommand2()
        {
            ICommand addControlPointCommand = new AddControlPointContinuousCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(addControlPointCommand);

            addControlPointCommand = new AddControlPointContinuousCommand(this.LineSketchObject, new Vector3(1, 2.999f, 3));
            Invoker.ExecuteCommand(addControlPointCommand);

            Assert.AreEqual(1, this.LineSketchObject.getNumberOfControlPoints());
        }


        [Test]
        public void DeleteByRadiusCommand()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);

            ICommand deleteCommand = new DeleteControlPointsByRadiusCommand(this.LineSketchObject, new Vector3(3, 3.5f, 3), .6f);
            Invoker.ExecuteCommand(deleteCommand);

            Assert.AreEqual(2, this.LineSketchObject.getNumberOfControlPoints());
            Assert.AreEqual((2 + 2) * 7, this.LineSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void DeleteByRadiusCommandUndo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);

            ICommand deleteCommand = new DeleteControlPointsByRadiusCommand(this.LineSketchObject, new Vector3(3, 3.5f, 3), .6f);
            Invoker.ExecuteCommand(deleteCommand);
            Invoker.Undo();

            Assert.AreEqual(4, this.LineSketchObject.getNumberOfControlPoints());
            Assert.AreEqual((3 * 20 + 2) * 7, this.LineSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void DeleteByRadiusCommandRedo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);

            ICommand deleteCommand = new DeleteControlPointsByRadiusCommand(this.LineSketchObject, new Vector3(3, 3.5f, 3), .6f);
            Invoker.ExecuteCommand(deleteCommand);
            Invoker.Undo();
            Invoker.Redo();

            Assert.AreEqual(2, this.LineSketchObject.getNumberOfControlPoints());
            Assert.AreEqual((2 + 2) * 7, this.LineSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);

            LineSketchObject[] lines = GameObject.FindObjectsOfType<LineSketchObject>();
            Assert.AreEqual(2, lines.Length);
            foreach (LineSketchObject line in lines)
            {
                if (line != this.LineSketchObject)
                {
                    Assert.AreEqual(1, line.getNumberOfControlPoints());
                    Assert.AreEqual(new Vector3(4, 3, 2), line.GetControlPoints()[0]);
                }
            }
        }

        [Test]
        public void RefineMeshTest()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);

            this.LineSketchObject.RefineMesh();

            Assert.AreEqual(this.LineSketchObject.getNumberOfControlPoints(), 4);
            Assert.AreEqual((3 * 20 + 2) * 7, this.LineSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void OversketchTest()
        {

            ICommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(0, 1, 0));
            Invoker.ExecuteCommand(command);
            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(0, 1, 1));
            Invoker.ExecuteCommand(command);
            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(0, 1, 2));
            Invoker.ExecuteCommand(command);
            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(0, 1, 3));
            Invoker.ExecuteCommand(command);
            command = new AddControlPointCommand(this.OverSketchObject, new Vector3(0, 2, 1));
            Invoker.ExecuteCommand(command);
            command = new AddControlPointCommand(this.OverSketchObject, new Vector3(0, 2, 2));
            Invoker.ExecuteCommand(command);

            command = new OverSketchLineCommand(LineSketchObject, OverSketchObject, 2, 10);
            Invoker.ExecuteCommand(command);



            List<Vector3> modifiedControlPoints = new List<Vector3> { new Vector3(0, 1.1f, 0.2f), new Vector3(0, 1.4f, 1.1f), new Vector3(0, 1.4f, 1.9f), new Vector3(0, 1.1f, 2.8f) };

            Assert.AreEqual(string.Join(",", modifiedControlPoints), string.Join(",", LineSketchObject.GetControlPoints()));

        }
        [Test]
        public void OversketchTestInsufficientRangeTest()
        {

            ICommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(0, 1, 0));
            Invoker.ExecuteCommand(command);
            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(0, 1, 1));
            Invoker.ExecuteCommand(command);
            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(0, 1, 2));
            Invoker.ExecuteCommand(command);
            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(0, 1, 3));
            Invoker.ExecuteCommand(command);
            command = new AddControlPointCommand(this.OverSketchObject, new Vector3(0, 2, 1));
            Invoker.ExecuteCommand(command);
            command = new AddControlPointCommand(this.OverSketchObject, new Vector3(0, 2, 2));
            Invoker.ExecuteCommand(command);
            command = new OverSketchLineCommand(LineSketchObject, OverSketchObject, 2, 0);
            Invoker.ExecuteCommand(command);

            List<Vector3> modifiedControlPoints = new List<Vector3> { new Vector3(0, 1, 0),new Vector3(0,1,1), new Vector3(0, 1, 2), new Vector3(0, 1, 3)};

            Assert.AreEqual(string.Join(",", modifiedControlPoints), string.Join(",", LineSketchObject.GetControlPoints()));


        }
        [Test]
        public void SimplifyCurveTest()
        {

            ICommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(0, 1, 0));
            Invoker.ExecuteCommand(command);
            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(0, 1, 1));
            Invoker.ExecuteCommand(command);
            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(0, 1, 2));
            Invoker.ExecuteCommand(command);
            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(0, 1, 3));
            Invoker.ExecuteCommand(command);
            command = new SimplifyLineCommand(LineSketchObject, 10);
            Invoker.ExecuteCommand(command);


            Assert.True(LineSketchObject.getNumberOfControlPoints()<4);

        }
        [Test]
        public void PopulateGapTest()
        {

            ICommand command = new AddControlPointCommand(this.LineSketchObject, new Vector3(0, 1, 0));
            Invoker.ExecuteCommand(command);
            command = new AddControlPointCommand(this.LineSketchObject, new Vector3(0, 1, 3));
            Invoker.ExecuteCommand(command);
            command = new PopulateLineCommand(LineSketchObject, 1);
            Invoker.ExecuteCommand(command);


            Assert.AreEqual(LineSketchObject.getNumberOfControlPoints(), 4);

        }
    }
}
