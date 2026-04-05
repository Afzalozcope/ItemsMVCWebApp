using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ItemsMVCWebApp.Models;
using Moq;
using NUnit.Framework;


namespace ItemsMVCWebApp.Tests.Controllers 
{
    [TestFixture]
    public class ItemControllerTests
    {
        private Mock<IItemRepository> _mockRepo;
        private ItemController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IItemRepository>();
            _controller = new ItemController(_mockRepo.Object);
        }

        [Test]
        public async Task Index_ReturnsView_WithItems()
        {
            // Arrange
            var items = new List<Item>
        {
            new Item { Id = 1, Name = "Item1" },
            new Item { Id = 2, Name = "Item2" }
        };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(items);

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<Item>>(result.Model);
            Assert.AreEqual(2, ((List<Item>)result.Model).Count);

            _mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task AddItemAsync_ValidModel_CallsRepo_AndReturnsSuccess()
        {
            // Arrange
            var item = new Item { Name = "Test" };

            // Act
            var result = await _controller.AddItemAsync(item) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Item>()), Times.Once);
        }

        [Test]
        public async Task AddItemAsync_InvalidModel_DoesNotCallRepo()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            var item = new Item();

            // Act
            var result = await _controller.AddItemAsync(item) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Item>()), Times.Never);
        }

        [Test]
        public async Task Edit_Get_ItemExists_ReturnsView()
        {
            // Arrange
            var item = new Item { Id = 1, Name = "Test" };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(item);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(item, result.Model);
        }

        [Test]
        public async Task Edit_Get_ItemNotFound_ReturnsHttpNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Item)null);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public async Task Edit_Post_ValidModel_CallsUpdate_AndRedirects()
        {
            // Arrange
            var item = new Item { Id = 1, Name = "Updated" };

            // Act
            var result = await _controller.Edit(item) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Item>()), Times.Once);
        }

        [Test]
        public async Task Edit_Post_InvalidModel_ReturnsView_AndDoesNotUpdate()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            var item = new Item { Id = 1 };

            // Act
            var result = await _controller.Edit(item) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(item, result.Model);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Item>()), Times.Never);
        }

        [Test]
        public async Task Delete_CallsRepo_AndRedirects()
        {
            // Arrange
            int id = 1;

            // Act
            var result = await _controller.Delete(id) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            _mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
        }
    }

}

