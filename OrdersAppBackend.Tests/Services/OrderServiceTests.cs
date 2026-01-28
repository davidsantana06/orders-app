using Moq;

using OrdersAppBackend.Data.Repositories;
using OrdersAppBackend.Dtos;
using OrdersAppBackend.Models;
using OrdersAppBackend.Services;

namespace OrdersAppBackend.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepoMock;
        private readonly Mock<IOrderItemRepository> _orderItemRepoMock;
        private readonly OrderService _sut;

        public OrderServiceTests()
        {
            _orderRepoMock = new Mock<IOrderRepository>();
            _orderItemRepoMock = new Mock<IOrderItemRepository>();
            _sut = new OrderService(_orderRepoMock.Object, _orderItemRepoMock.Object);
            // s u t
            // y n e
            // s d s
            // t e t
            // e r
            // m
        }

        [Fact]
        public async Task GetManyAsync_ShouldReturnOrders_WhenFiltersAreProvided()
        {
            // Arrange
            var make = "Toyota";
            var model = "Corolla";
            var year = 2020;
            var expectedOrders = new List<Order>
            {
                new Order { Id = 1, Status = "Solicitado", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Order { Id = 2, Status = "Aprovado", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };

            _orderRepoMock.Setup(
                r => r.FindManyAsync(make, model, year)
            ).ReturnsAsync(expectedOrders);

            // Act
            var result = await _sut.GetManyAsync(make, model, year);

            // Assert
            Assert.Equal(2, result.Count);
            _orderRepoMock.Verify(
                r => r.FindManyAsync(make, model, year),
                Times.Once
            );
        }

        [Fact]
        public async Task GetManyAsync_ShouldReturnOrders_WhenNoFilters()
        {
            // Arrange
            var expectedOrders = new List<Order>
            {
                new Order { Id = 1, Status = "Solicitado", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Order { Id = 2, Status = "Aprovado", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Order { Id = 3, Status = "Concluído", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };

            _orderRepoMock.Setup(
                r => r.FindManyAsync(null, null, null)
            ).ReturnsAsync(expectedOrders);

            // Act
            var result = await _sut.GetManyAsync();

            // Assert
            Assert.Equal(3, result.Count);
            _orderRepoMock.Verify(
                r => r.FindManyAsync(null, null, null),
                Times.Once
            );
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateOrderWithItems_WhenDtoIsValid()
        {
            // Arrange
            var dto = new CreateOrderDto
            {
                Items = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto
                    {
                        Make = "Toyota",
                        Model = "Corolla",
                        Year = 2020,
                        Quantity = 2,
                        UnitPrice = 95000.00m
                    }
                }
            };

            Order? capturedOrder = null;
            List<OrderItem>? capturedItems = null;

            _orderRepoMock.Setup(r => r.AddAsync(It.IsAny<Order>()))
                .Callback<Order>(o => capturedOrder = o)
                .Returns(Task.CompletedTask);

            _orderItemRepoMock.Setup(r => r.AddManyAsync(It.IsAny<int>(), It.IsAny<List<OrderItem>>()))
                .Callback<int, List<OrderItem>>((id, items) => capturedItems = items)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.CreateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Solicitado", result.Status);
            Assert.NotNull(capturedOrder);
            Assert.Equal("Solicitado", capturedOrder.Status);
            Assert.NotNull(capturedItems);
            Assert.Single(capturedItems);
            Assert.Equal("Toyota", capturedItems[0].Make);
            Assert.Equal("Corolla", capturedItems[0].Model);
            Assert.Equal(2020, capturedItems[0].Year);
            _orderRepoMock.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
            _orderItemRepoMock.Verify(
                r => r.AddManyAsync(It.IsAny<int>(), It.IsAny<List<OrderItem>>()),
                Times.Once
            );
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateOrderWithoutItems_WhenNoItemsProvided()
        {
            // Arrange
            var dto = new CreateOrderDto { Items = null };
            Order? capturedOrder = null;

            _orderRepoMock.Setup(r => r.AddAsync(It.IsAny<Order>()))
                .Callback<Order>(o => capturedOrder = o)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.CreateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Solicitado", result.Status);
            Assert.NotNull(capturedOrder);
            _orderRepoMock.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
            _orderItemRepoMock.Verify(
                r => r.AddManyAsync(It.IsAny<int>(), It.IsAny<List<OrderItem>>()),
                Times.Never
            );
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                Status = "Solicitado",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _orderRepoMock.Setup(r => r.FindFirstByIdAsync(1)).ReturnsAsync(order);

            // Act
            var result = await _sut.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Solicitado", result.Status);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            _orderRepoMock.Setup(
                r => r.FindFirstByIdAsync(It.IsAny<int>())
            ).ReturnsAsync((Order?)null);

            // Act
            var result = await _sut.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateOrder_WhenOrderExists()
        {
            // Arrange
            var existingOrder = new Order
            {
                Id = 1,
                Status = "Solicitado",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var dto = new UpdateOrderDto { Status = "Aprovado" };
            Order? updatedOrder = null;

            _orderRepoMock.Setup(r => r.FindFirstByIdAsync(1)).ReturnsAsync(existingOrder);
            _orderRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Order>()))
                .Callback<Order>(o => updatedOrder = o)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.UpdateAsync(1, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Aprovado", result.Status);
            Assert.NotNull(updatedOrder);
            Assert.Equal("Aprovado", updatedOrder.Status);
            _orderRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            var dto = new UpdateOrderDto { Status = "Aprovado" };
            _orderRepoMock.Setup(
                r => r.FindFirstByIdAsync(It.IsAny<int>())
            ).ReturnsAsync((Order?)null);

            // Act
            var result = await _sut.UpdateAsync(99, dto);

            // Assert
            Assert.Null(result);
            _orderRepoMock.Verify(
                r => r.UpdateAsync(It.IsAny<Order>()),
                Times.Never
            );
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenOrderExists()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                Status = "Solicitado",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _orderRepoMock.Setup(r => r.FindFirstByIdAsync(1)).ReturnsAsync(order);

            // Act
            var result = await _sut.DeleteAsync(1);

            // Assert
            Assert.True(result);
            _orderRepoMock.Verify(r => r.RemoveAsync(order), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenOrderDoesNotExist()
        {
            // Arrange
            _orderRepoMock.Setup(
                r => r.FindFirstByIdAsync(It.IsAny<int>())
            ).ReturnsAsync((Order?)null);

            // Act
            var result = await _sut.DeleteAsync(99);

            // Assert
            Assert.False(result);
            _orderRepoMock.Verify(
                r => r.RemoveAsync(It.IsAny<Order>()),
                Times.Never
            );
        }
    }
}
