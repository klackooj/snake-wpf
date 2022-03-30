using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SnakeWPF.Model;
using SnakeWPF.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace SnakeWPFTest
{
    [TestClass]
    public class SnakeTest
    {
        private SnakeModel model;
        private Mock<ISnakeDataAccess> mock;

        [TestInitialize]
        public void Init()
        {
            mock = new Mock<ISnakeDataAccess>();

            model = new SnakeModel(mock.Object, new SnakeTable());
        }

        [TestMethod]
        public void TestNewGame10()
        {
            Int32 size = 10;
            model.Size = size;

            model.firstSnake(size);
            model.getWalls(size);

            Assert.AreEqual(10, model.Size);
            Assert.AreEqual(Directions.UP, model.Direction);
            Assert.AreEqual(2, model.Snake.First.Value.X);
            Assert.AreEqual(6, model.Snake.First.Value.Y);
            Assert.AreEqual(6, model.Snake.Last.Value.X);
            Assert.AreEqual(6, model.Snake.Last.Value.Y);
            Assert.AreEqual(5, model.Snake.Count);
            Assert.AreEqual(2, model.Walls[1].X);
            Assert.AreEqual(4, model.Walls[1].Y);
            Assert.AreEqual(53, model.Walls.Count);
            Assert.IsFalse(model.isGameOver());
            Assert.IsFalse(model.shouldCandy());
            Assert.AreEqual(0, model.Seconds);

        }

        [TestMethod]
        public void TestNewGame15()
        {
            Int32 size = 15;
            model.Size = size;

            model.firstSnake(size);
            model.getWalls(size);

            Assert.AreEqual(15, model.Size);
            Assert.AreEqual(Directions.UP, model.Direction);
            Assert.AreEqual(4, model.Snake.First.Value.X);
            Assert.AreEqual(8, model.Snake.First.Value.Y);
            Assert.AreEqual(8, model.Snake.Last.Value.X);
            Assert.AreEqual(8, model.Snake.Last.Value.Y);
            Assert.AreEqual(5, model.Snake.Count);
            Assert.AreEqual(4, model.Walls[1].X);
            Assert.AreEqual(14, model.Walls[1].Y);
            Assert.AreEqual(80, model.Walls.Count);
            Assert.IsFalse(model.isGameOver());
            Assert.IsFalse(model.shouldCandy());
            Assert.AreEqual(0, model.Seconds);

        }

        [TestMethod]
        public void TestNewGame20()
        {
            Int32 size = 20;
            model.Size = size;

            model.firstSnake(size);
            model.getWalls(size);

            Assert.AreEqual(20, model.Size);
            Assert.AreEqual(Directions.UP, model.Direction);
            Assert.AreEqual(7, model.Snake.First.Value.X);
            Assert.AreEqual(11, model.Snake.First.Value.Y);
            Assert.AreEqual(11, model.Snake.Last.Value.X);
            Assert.AreEqual(11, model.Snake.Last.Value.Y);
            Assert.AreEqual(5, model.Snake.Count);
            Assert.AreEqual(3, model.Walls[1].X);
            Assert.AreEqual(10, model.Walls[1].Y);
            Assert.AreEqual(111, model.Walls.Count);
            Assert.IsFalse(model.isGameOver());
            Assert.IsFalse(model.shouldCandy());
            Assert.AreEqual(0, model.Seconds);

        }

        [TestMethod]
        public async Task TestLoadGame()
        {
            mock.Setup(m => m.LoadAsync("path")).Returns(() => Task.FromResult(new SnakeTable(3000, 10, Directions.DOWN, new Point(2, 2), new List<Point>() { new Point(3, 3) }, new List<Point>() { new Point(3, 3) })));

            await model.OpenAsync("path");
            mock.Verify(access => access.LoadAsync("path"), Times.Once());

            Assert.AreEqual(Directions.DOWN, model.Direction);
            Assert.AreEqual(3000, model.Seconds);
            Assert.IsTrue(model.isGameOver());
            Assert.IsFalse(model.shouldCandy());
            Assert.AreEqual(1, model.Snake.Count);
        }

        [TestMethod]
        public async Task TestCherryGame()
        {
            mock.Setup(m => m.LoadAsync("path")).Returns(() => Task.FromResult(new SnakeTable(3000, 10, Directions.DOWN, new Point(2, 2), new List<Point>() { new Point(3, 3) }, new List<Point>() { new Point(2, 2) })));

            await model.OpenAsync("path");
            mock.Verify(access => access.LoadAsync("path"), Times.Once());
            model.snakeMove();

            Assert.AreEqual(Directions.DOWN, model.Direction);
            Assert.AreEqual(3000, model.Seconds);
            Assert.IsFalse(model.isGameOver());
            Assert.IsFalse(model.shouldCandy());
            Assert.AreEqual(2, model.Snake.Count);
        }

        [TestMethod]
        public async Task TestMove()
        {
            mock.Setup(m => m.LoadAsync("path")).Returns(() => Task.FromResult(new SnakeTable(3000, 10, Directions.DOWN, new Point(2, 2), new List<Point>() { new Point(3, 3) }, new List<Point>() { new Point(2, 2) })));
            await model.OpenAsync("path");
            mock.Verify(access => access.LoadAsync("path"), Times.Once());
            model.snakeMove();

            Assert.AreEqual(3, model.Snake.First.Value.X);
            Assert.AreEqual(2, model.Snake.First.Value.Y);
            Assert.AreEqual(2, model.Snake.Last.Value.X);
            Assert.AreEqual(2, model.Snake.Last.Value.Y);

            model.snakeMove();

            Assert.AreEqual(4, model.Snake.First.Value.X);
            Assert.AreEqual(2, model.Snake.First.Value.Y);
            Assert.AreEqual(3, model.Snake.Last.Value.X);
            Assert.AreEqual(2, model.Snake.Last.Value.Y);
        }

        [TestMethod]
        public async Task TestInvalidMove()
        {
            mock.Setup(m => m.LoadAsync("path")).Returns(() => Task.FromResult(new SnakeTable(3000, 10, Directions.DOWN, new Point(2, 2), new List<Point>() { new Point(3, 3) }, new List<Point>() { new Point(2, 2) })));
            await model.OpenAsync("path");
            mock.Verify(access => access.LoadAsync("path"), Times.Once());
            model.snakeMove();

            Assert.AreEqual(3, model.Snake.First.Value.X);
            Assert.AreEqual(2, model.Snake.First.Value.Y);
            Assert.AreEqual(2, model.Snake.Last.Value.X);
            Assert.AreEqual(2, model.Snake.Last.Value.Y);

            model.Direction = Directions.UP;
            model.snakeMove();

            Assert.AreEqual(4, model.Snake.First.Value.X);
            Assert.AreEqual(2, model.Snake.First.Value.Y);
            Assert.AreEqual(3, model.Snake.Last.Value.X);
            Assert.AreEqual(2, model.Snake.Last.Value.Y);
        }

        [TestMethod]
        public async Task SaveTest()
        {
            mock.Setup(m => m.SaveAsync("path", model.Table));
            await model.SaveAsync("path", model.Table);
            mock.Verify(access => access.SaveAsync("path", model.Table), Times.Once());
        }
    }
}
