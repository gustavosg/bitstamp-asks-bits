using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moq;
using PriceSimulator.Domain.Entities.TradeStream;
using PriceSimulator.Infrastructure.Adapters.Persistence.MySQL;

namespace PriceSimulator.Tests.Infrastructure.Adapters.Persistence.MySQL
{
    public class PriceSimulationRepositoryTest
    {
        private readonly PriceSimulationRepository repository;
        private readonly Mock<AppDbContext> contextMock;
        private readonly Mock<DbSet<PriceSimulation>> dbSetMock;

        public PriceSimulationRepositoryTest()
        {
            contextMock = new Mock<AppDbContext>();
            dbSetMock = new Mock<DbSet<PriceSimulation>>();
            contextMock.Setup(x => x.Set<PriceSimulation>()).Returns(dbSetMock.Object);
            repository = new PriceSimulationRepository(contextMock.Object);
        }

        [Fact]
        public async Task Add_ShouldAddPriceSimulationAndSaveChanges()
        {
            PriceSimulation priceSimulation = new PriceSimulation { Id = Guid.NewGuid() };

            await repository.Add(priceSimulation);

            dbSetMock.Verify(x => x.AddAsync(priceSimulation, default), Times.Once);
            contextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Exists_ShouldReturnTrueIfPriceSimulationExists()
        {
            var id = Guid.NewGuid();
            dbSetMock.Setup(x => x.AnyAsync(
                It.IsAny<Expression<Func<PriceSimulation, bool>>>(), default)
            )
                      .ReturnsAsync(true);

            var result = await repository.Exists(id);

            Assert.True(result);
        }

        [Fact]
        public async Task Exists_ShouldReturnFalseIfPriceSimulationDoesNotExist()
        {
            var id = Guid.NewGuid();
            dbSetMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<PriceSimulation, bool>>>(), default))
                      .ReturnsAsync(false);

            var result = await repository.Exists(id);

            Assert.False(result);
        }

        [Fact]
        public async Task Get_ShouldReturnPriceSimulationById()
        {
            var id = Guid.NewGuid();
            var priceSimulation = new PriceSimulation { Id = id };
            dbSetMock.Setup(x => x.Include(It.IsAny<string>()).AsNoTracking())
                      .Returns(dbSetMock.Object);
            dbSetMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<PriceSimulation, bool>>>(), default))
                      .ReturnsAsync(priceSimulation);

            var result = await repository.Get(id);

            Assert.Equal(priceSimulation, result);
        }

        [Fact]
        public async Task Get_ShouldReturnListOfPriceSimulations()
        {
            var priceSimulations = new List<PriceSimulation>
            {
                new PriceSimulation { Id = Guid.NewGuid() },
                new PriceSimulation { Id = Guid.NewGuid() }
            };
            dbSetMock.Setup(x => x.Include(It.IsAny<string>()).AsNoTracking())
                      .Returns(dbSetMock.Object);
            dbSetMock.Setup(x => x.ToListAsync(default)).ReturnsAsync(priceSimulations);

            var result = await repository.Get();

            Assert.Equal(priceSimulations, result);
        }
    }
}
