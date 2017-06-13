using NUnit.Framework;
using System;
using System.Data.Common;
using System.Linq;
using Tasks.Database;
using Tasks.Database.Models;

namespace Tasks.Tests
{
    [TestFixture()]
    public class MainFormTests
    {
        [Test()]
        public void AddTaskTest()
        {
            DbConnection connection = Effort.DbConnectionFactory.CreateTransient();
            EfUnitOfWork efUnitOfWork = new EfUnitOfWork(connection);

            efUnitOfWork.Jobs.Add(new Job()
            {
                Date = DateTime.Now,
                Task = "1",
                Repeat = 1,
                Name = "1",
                Settings = "1",
                Param = "1",
                StopResult = "1",
                ErrorResult = "1"
            });
            efUnitOfWork.SaveChanges();

            Assert.AreEqual(efUnitOfWork.Jobs.Count(), 1);
        }
    }
}