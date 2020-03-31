using CmApp.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VechicleAPI.Tests
{
    class MakesAPi
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task GetAll()
        {
            var repo = new VehicleAPI();

            var result = await repo.GetAllMakes();

            var list = result.Cars.Take(50).ToList();
            var res = new List<string>();
            string stuff="";
            foreach (var l in list)
            {
                res.Add(l.Name);
                stuff += l.Name + "\n";

            }

           


            res.Sort();
            res.RemoveAt(0);
            res.RemoveAt(4);
            res.RemoveAt(6);
            res.RemoveAt(10);
            res.RemoveAt(11);
            res.RemoveAt(12);
            res.RemoveAt(13);
            res.RemoveAt(16);
            res.RemoveAt(17);
            res.RemoveAt(18);
            res.RemoveAt(19);
            res.RemoveAt(20);
            res.RemoveAt(21);
            res.RemoveAt(26);
            res.RemoveAt(28);
            res.RemoveAt(29);
            res.RemoveAt(34);
            res.RemoveAt(37);
            res.RemoveAt(38);
            res.RemoveAt(41);
            res.RemoveAt(44);
            res.RemoveAt(48);
            res.RemoveAt(49);
            res.Add("Peugeot");
            Assert.AreNotEqual(0, result.Count);
        }
    }
}
