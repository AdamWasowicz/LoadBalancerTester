using LBT_Api.Entities;
using LBT_Api.Interfaces.Services;

namespace LBT_Api.Services
{
    public class TestingService : ITestingService
    {
        private readonly LBT_DbContext _dbContext;

        public TestingService(LBT_DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void ClearAll()
        {
            _dbContext.RemoveRange(_dbContext.Companys);
            _dbContext.SaveChanges();
            _dbContext.RemoveRange(_dbContext.Workers);
            _dbContext.SaveChanges();
            _dbContext.RemoveRange(_dbContext.ContactInfos);
            _dbContext.SaveChanges();
            _dbContext.RemoveRange(_dbContext.Addresses);
            _dbContext.SaveChanges();
            _dbContext.RemoveRange(_dbContext.Sales);
            _dbContext.SaveChanges();
            _dbContext.RemoveRange(_dbContext.ProductsSold);
            _dbContext.SaveChanges();
            _dbContext.RemoveRange(_dbContext.Products);
            _dbContext.SaveChanges();
            _dbContext.RemoveRange(_dbContext.Suppliers);
            _dbContext.SaveChanges();
        }
    }
}
