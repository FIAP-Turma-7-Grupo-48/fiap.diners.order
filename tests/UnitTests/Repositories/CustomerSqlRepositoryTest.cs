using System.Linq.Expressions;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.SqlModels.CustomerAggregate;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace UnitTests.Repositories
{
    public class CustomerSqlRepositoryTest
    {
        private readonly CustomerSqlRepository _customerRepository;
        private readonly DinersOrderSqlContext _context;

        public CustomerSqlRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<DinersOrderSqlContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new DinersOrderSqlContext(options);

            var customers = GenerateCustomers();
            _context.Customer.AddRange(customers);
            _context.CommitAsync();

            _customerRepository = new CustomerSqlRepository(_context);
        }

        [Fact]
        public void DevePermitirAdicionarCliente()
        {
            CustomerSqlModel customerSqlModel = new CustomerSqlModel()
            {
                Name = "Alex",
                Email = "alex@email.com",
                Cpf = "332.824.233-67",
                CreatedAt = DateTime.Now,
                Id = 939,
                UpdatedAt = DateTime.Now
            };

            _customerRepository.Add(customerSqlModel);
            _context.SaveChanges();

            var addedCustomer = _context.Customer.Find(customerSqlModel.Id);

            Assert.NotNull(addedCustomer);
            Assert.Equal("Alex", addedCustomer.Name);
        }

        [Fact]
        public async void DevePermitirContarCliente()
        {
            // var result = await _customerRepository.CountAsync(x => x.Cpf.Equals("333.824.233-67"), default);
            var result = await _customerRepository.CountAsync(x => x.Id == 960, CancellationToken.None);

            Assert.True(result > 0);
            Assert.Equal(1, result);
        }

        [Fact]
        public async void DevePermitirObterClientePorCpf()
        {
            var result = await _customerRepository.GetAsync(x => x.Cpf.Equals("333.824.233-67"), false, default);

            Assert.NotNull(result);
            Assert.Equal("Alex", result.Name);
        }

        [Fact]
        public async void DevePermitirObterClientePorId()
        {
            var result = await _customerRepository.GetAsync(x => x.Id.Equals(938), false, default);

            Assert.NotNull(result);
            Assert.Equal("Alex", result.Name);
        }

        private static IList<CustomerSqlModel> GenerateCustomers()
        {
            IList<CustomerSqlModel> customers = new List<CustomerSqlModel>
            {
                new()
                {
                    Name = "Alex",
                    Email = "alex@email.com",
                    Cpf = "333.824.233-67",
                    CreatedAt = DateTime.Now,
                    Id = 960,
                    UpdatedAt = DateTime.Now
                }
            };

            return customers;
        }
    }
}