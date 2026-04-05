using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ItemsMVCWebApp.Models;
using System.Web.Http.Results;

namespace ItemsMVCWebApp.Tests.Controllers
{
    [TestFixture]
    public class ItemApiControllerTests
    {
        private Mock<IItemRepository> _mockRepo;
        private ItemApiController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IItemRepository>();
            _controller = new ItemApiController(_mockRepo.Object);
        }

        [Test]
        public async Task GetItems_ReturnsOk_WithItems()
        {
            // Arrange
            var items = new List<Item>
        {
            new Item { Id = 1, Name = "Item1" }
        };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(items);

            // Act
            var result = await _controller.GetItems()
                         as OkNegotiatedContentResult<List<Item>>;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetItem_ValidId_ReturnsOk()
        {
            var item = new Item { Id = 1, Name = "Test" };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(item);

            var result = await _controller.GetItem(1)
                         as OkNegotiatedContentResult<Item>;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task GetItem_InvalidId_ReturnsNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Item)null);

            var result = await _controller.GetItem(1);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task PostItem_Valid_ReturnsOk_AndCallsRepo()
        {
            var item = new Item { Name = "New Item" };

            var result = await _controller.PostItem(item)
                         as OkNegotiatedContentResult<Item>;

            Assert.That(result, Is.Not.Null);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Item>()), Times.Once);
        }

        [Test]
        public async Task PostItem_InvalidModel_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("Name", "Required");

            var result = await _controller.PostItem(new Item());

            Assert.That(result, Is.InstanceOf<InvalidModelStateResult>());
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Item>()), Times.Never);
        }

        [Test]
        public async Task PutItem_Valid_ReturnsOk_AndUpdates()
        {
            var existing = new Item { Id = 1, Name = "Old" };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            var updated = new Item { Name = "New Name", Description = "Updated" };

            var result = await _controller.PutItem(1, updated)
                         as OkNegotiatedContentResult<Item>;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content.Name, Is.EqualTo("New Name"));

            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Item>()), Times.Once);
        }

        [Test]
        public async Task PutItem_NotFound_ReturnsNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Item)null);

            var result = await _controller.PutItem(1, new Item());

            Assert.That(result, Is.TypeOf<NotFoundResult>());
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Item>()), Times.Never);
        }

        [Test]
        public async Task PutItem_InvalidModel_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("Name", "Required");

            var result = await _controller.PutItem(1, new Item());

            Assert.That(result, Is.InstanceOf<InvalidModelStateResult>());
        }

        [Test]
        public async Task DeleteItem_Valid_ReturnsOk()
        {
            var existing = new Item { Id = 1 };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

            var result = await _controller.DeleteItem(1);

            Assert.That(result, Is.InstanceOf<OkResult>());
            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Test]
        public async Task DeleteItem_NotFound_ReturnsNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Item)null);

            var result = await _controller.DeleteItem(1);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
            _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }

}


