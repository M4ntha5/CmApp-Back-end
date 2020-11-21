using CmApp.Contracts.Interfaces.Repositories;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Repositories
{
    public class MakeRepositoryTests
    {
        IMakeRepository repoMock;

        [SetUp]
        public void Setup()
        {
            repoMock = Substitute.For<IMakeRepository>();
        }

        [Test]
        public async Task GetAllMakes()
        {

        }
    }
}
