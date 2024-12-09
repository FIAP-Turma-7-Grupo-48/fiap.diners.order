using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.SqlModels.CustomerAggregate;
using Moq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Repository.CustomerSqlTest
{
    public class CustomerSqlRepositoryTest
    {
        private readonly DinersOrderSqlContext _context;
        private readonly CustomerSqlRepository _customerRepository;

        public CustomerSqlRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<DinersOrderSqlContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new DinersOrderSqlContext(options);
            _customerRepository = new CustomerSqlRepository(_context);
        }

        [Fact]
        public void DevePermitirAdicionarCliente()
        {
            CustomerSqlModel customerSqlModel = new CustomerSqlModel()
            {
                Name = "Alex",
                Email = "alex@email.com",
                Cpf = "333.824.233-67",
                CreatedAt = DateTime.Now,
                Id = 939,
                UpdatedAt = DateTime.Now
            };

            _customerRepository.Add(customerSqlModel);
            _context.SaveChanges();

            var addedCustomer = _customerRepository.CountAsync(x => x.Id == customerSqlModel.Id, CancellationToken.None);
            Assert.NotNull(addedCustomer);
        }

        [Fact]
        public async Task DevePermitirObterClientePorCpf()
        {
            CustomerSqlModel customerSqlModel = new CustomerSqlModel()
            {
                Name = "Alex",
                Email = "alex@email.com",
                Cpf = "333.824.233-67",
                CreatedAt = DateTime.Now,
                Id = 938,
                UpdatedAt = DateTime.Now
            };

            _customerRepository.Add(customerSqlModel);
            await _context.SaveChangesAsync();

            var result = await _customerRepository.GetAsync(x => x.Cpf.Equals("333.824.233-67"), false, default);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DevePermitirObterClientePorId()
        {
            CustomerSqlModel customerSqlModel = new CustomerSqlModel()
            {
                Name = "Alex",
                Email = "alex@email.com",
                Cpf = "333.824.233-67",
                CreatedAt = DateTime.Now,
                Id = 940,
                UpdatedAt = DateTime.Now
            };

            _customerRepository.Add(customerSqlModel);
            await _context.SaveChangesAsync();

            var result = await _customerRepository.GetAsync(x => x.Id == customerSqlModel.Id, false, default);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DevePermitirContarCliente()
        {
            CustomerSqlModel customerSqlModel = new CustomerSqlModel()
            {
                Name = "Alex",
                Email = "alex@email.com",
                Cpf = "333.824.233-67",
                CreatedAt = DateTime.Now,
                Id = 941,
                UpdatedAt = DateTime.Now
            };

            _customerRepository.Add(customerSqlModel);
            await _context.SaveChangesAsync();

            var result = await _customerRepository.CountAsync(x => x.Id == customerSqlModel.Id, default);

            Assert.True(result > 0);
            Assert.Equal(1, result);
        }
    }
}