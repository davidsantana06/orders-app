using Moq;

using OrdersAppBackend.Data.Repositories;
using OrdersAppBackend.Dtos;
using OrdersAppBackend.Models;
using OrdersAppBackend.Services;

namespace OrdersAppBackend.Tests.Services
{
    public class OrderItemServiceTests
    {
        private readonly Mock<IOrderItemRepository> _orderItemRepoMock;
        private readonly Mock<IOrderRepository> _orderRepoMock;
        private readonly OrderItemService _sut;

        public OrderItemServiceTests()
        {
            _orderItemRepoMock = new Mock<IOrderItemRepository>();
            _orderRepoMock = new Mock<IOrderRepository>();
            _sut = new OrderItemService(_orderItemRepoMock.Object, _orderRepoMock.Object);
            // s u t
            // y n e
            // s d s
            // t e t
            // e r
            // m
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnOrderItem_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;
            var dto = new CreateOrderItemDto
            {
                Make = "Toyota",
                Model = "Corolla",
                Year = 2020,
                Quantity = 2,
                UnitPrice = 95000.00m
            };

            var existingOrder = new Order
            {
                Id = orderId,
                Status = "Solicitado",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            OrderItem? capturedOrderItem = null;

            _orderRepoMock.Setup(r => r.FindFirstByIdAsync(orderId)).ReturnsAsync(existingOrder);
            _orderItemRepoMock.Setup(r => r.AddAsync(It.IsAny<OrderItem>()))
                .Callback<OrderItem>(oi => capturedOrderItem = oi)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.CreateAsync(dto, orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.OrderId);
            Assert.Equal(dto.Make, result.Make);
            Assert.Equal(dto.Model, result.Model);
            Assert.Equal(dto.Year, result.Year);
            Assert.Equal(dto.Quantity, result.Quantity);
            Assert.Equal(dto.UnitPrice, result.UnitPrice);
            Assert.NotNull(capturedOrderItem);
            _orderRepoMock.Verify(r => r.FindFirstByIdAsync(orderId), Times.Once);
            _orderItemRepoMock.Verify(r => r.AddAsync(It.IsAny<OrderItem>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnNull_WhenOrderNotFound()
        {
            // Arrange
            var orderId = 99;
            var dto = new CreateOrderItemDto
            {
                Make = "Toyota",
                Model = "Corolla",
                Year = 2020,
                Quantity = 2,
                UnitPrice = 95000.00m
            };

            _orderRepoMock.Setup(
                r => r.FindFirstByIdAsync(orderId)
            ).ReturnsAsync((Order?)null);

            // Act
            var result = await _sut.CreateAsync(dto, orderId);

            // Assert
            Assert.Null(result);
            _orderRepoMock.Verify(r => r.FindFirstByIdAsync(orderId), Times.Once);
            _orderItemRepoMock.Verify(
                r => r.AddAsync(It.IsAny<OrderItem>()),
                Times.Never
            );
        }

        [Fact]
        public async Task GetManyAsync_ShouldReturnOrderItems_WhenItemsExist()
        {
            // Arrange
            var expectedItems = new List<OrderItem>
            {
                new OrderItem
                {
                    Id = 1,
                    OrderId = 1,
                    Make = "Toyota",
                    Model = "Corolla",
                    Year = 2020,
                    Quantity = 2,
                    UnitPrice = 95000.00m
                },
                new OrderItem
                {
                    Id = 2,
                    OrderId = 1,
                    Make = "Honda",
                    Model = "Civic",
                    Year = 2021,
                    Quantity = 1,
                    UnitPrice = 85000.00m
                }
            };

            _orderItemRepoMock.Setup(r => r.FindManyAsync()).ReturnsAsync(expectedItems);

            // Act
            var result = await _sut.GetManyAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Toyota", result[0].Make);
            Assert.Equal("Honda", result[1].Make);
        }

        [Fact]
        public async Task GetManyAsync_ShouldReturnEmpty_WhenNoItemsExist()
        {
            // Arrange
            _orderItemRepoMock.Setup(r => r.FindManyAsync()).ReturnsAsync(new List<OrderItem>());

            // Act
            var result = await _sut.GetManyAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOrderItem_WhenItemExists()
        {
            // Arrange
            var orderItem = new OrderItem
            {
                Id = 1,
                OrderId = 1,
                Make = "Toyota",
                Model = "Corolla",
                Year = 2020,
                Quantity = 2,
                UnitPrice = 95000.00m
            };

            _orderItemRepoMock.Setup(r => r.FindFirstByIdAsync(1)).ReturnsAsync(orderItem);

            // Act
            var result = await _sut.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Toyota", result.Make);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenItemDoesNotExist()
        {
            // Arrange
            _orderItemRepoMock.Setup(
                r => r.FindFirstByIdAsync(It.IsAny<int>())
            ).ReturnsAsync((OrderItem?)null);

            // Act
            var result = await _sut.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateOrderItem_WhenItemExists()
        {
            // Arrange
            var existingItem = new OrderItem
            {
                Id = 1,
                OrderId = 1,
                Make = "Toyota",
                Model = "Corolla",
                Year = 2020,
                Quantity = 2,
                UnitPrice = 95000.00m
            };

            var dto = new UpdateOrderItemDto
            {
                Make = "Honda",
                Model = "Civic",
                Year = 2021,
                Quantity = 3,
                UnitPrice = 85000.00m
            };

            OrderItem? updatedItem = null;

            _orderItemRepoMock.Setup(r => r.FindFirstByIdAsync(1)).ReturnsAsync(existingItem);
            _orderItemRepoMock.Setup(r => r.UpdateAsync(It.IsAny<OrderItem>()))
                .Callback<OrderItem>(oi => updatedItem = oi)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.UpdateAsync(1, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Honda", result.Make);
            Assert.Equal("Civic", result.Model);
            Assert.Equal(2021, result.Year);
            Assert.Equal(3, result.Quantity);
            Assert.Equal(85000.00m, result.UnitPrice);
            Assert.NotNull(updatedItem);
            _orderItemRepoMock.Verify(r => r.UpdateAsync(It.IsAny<OrderItem>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenItemDoesNotExist()
        {
            // Arrange
            var dto = new UpdateOrderItemDto
            {
                Make = "Honda",
                Model = "Civic",
                Year = 2021,
                Quantity = 3,
                UnitPrice = 85000.00m
            };

            _orderItemRepoMock.Setup(
                r => r.FindFirstByIdAsync(It.IsAny<int>())
            ).ReturnsAsync((OrderItem?)null);

            // Act
            var result = await _sut.UpdateAsync(99, dto);

            // Assert
            Assert.Null(result);
            _orderItemRepoMock.Verify(
                r => r.UpdateAsync(It.IsAny<OrderItem>()),
                Times.Never
            );
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenItemExists()
        {
            // Arrange
            var orderItem = new OrderItem
            {
                Id = 1,
                OrderId = 1,
                Make = "Toyota",
                Model = "Corolla",
                Year = 2020,
                Quantity = 2,
                UnitPrice = 95000.00m
            };

            _orderItemRepoMock.Setup(r => r.FindFirstByIdAsync(1)).ReturnsAsync(orderItem);

            // Act
            var result = await _sut.DeleteAsync(1);

            // Assert
            Assert.True(result);
            _orderItemRepoMock.Verify(r => r.RemoveAsync(orderItem), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenItemDoesNotExist()
        {
            // Arrange
            _orderItemRepoMock.Setup(
                r => r.FindFirstByIdAsync(It.IsAny<int>())
            ).ReturnsAsync((OrderItem?)null);

            // Act
            var result = await _sut.DeleteAsync(99);

            // Assert
            Assert.False(result);
            _orderItemRepoMock.Verify(
                r => r.RemoveAsync(It.IsAny<OrderItem>()),
                Times.Never
            );
        }

        [Fact]
        public async Task GetMakesAsync_ShouldReturnDistinctMakes()
        {
            // Arrange
            var expectedMakes = new List<string> { "Toyota", "Honda", "Ford" };
            _orderItemRepoMock.Setup(r => r.GetDistinctMakesAsync()).ReturnsAsync(expectedMakes);

            // Act
            var result = await _sut.GetMakesAsync();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains("Toyota", result);
            Assert.Contains("Honda", result);
            Assert.Contains("Ford", result);
        }

        [Fact]
        public async Task GetModelsAsync_ShouldReturnDistinctModels_ForGivenMake()
        {
            // Arrange
            var make = "Toyota";
            var expectedModels = new List<string> { "Corolla", "Camry", "RAV4" };
            _orderItemRepoMock.Setup(r => r.GetDistinctModelsAsync(make)).ReturnsAsync(expectedModels);

            // Act
            var result = await _sut.GetModelsAsync(make);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains("Corolla", result);
            Assert.Contains("Camry", result);
            Assert.Contains("RAV4", result);
            _orderItemRepoMock.Verify(r => r.GetDistinctModelsAsync(make), Times.Once);
        }

        [Fact]
        public async Task GetYearsAsync_ShouldReturnDistinctYears_ForGivenMakeAndModel()
        {
            // Arrange
            var make = "Toyota";
            var model = "Corolla";
            var expectedYears = new List<int> { 2020, 2021, 2022 };
            _orderItemRepoMock.Setup(
                r => r.GetDistinctYearsAsync(make, model)
            ).ReturnsAsync(expectedYears);

            // Act
            var result = await _sut.GetYearsAsync(make, model);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(2020, result);
            Assert.Contains(2021, result);
            Assert.Contains(2022, result);
            _orderItemRepoMock.Verify(r => r.GetDistinctYearsAsync(make, model), Times.Once);
        }
    }
}
