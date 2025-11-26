using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;


namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        


        //Méthode pour créer un mock de IStringLocalizer<ProductService>
        private Mock<IStringLocalizer<ProductService>> CreateLocalizerMock()
        {
            var mock = new Mock<IStringLocalizer<ProductService>>();

            mock.Setup(l => l[It.IsAny<string>()])
                .Returns((string key) => new LocalizedString(key, key));

            return mock;
        }

        // Test: Nom manquant
        [Fact]
        public void CheckProductModelErrors_ShouldReturn_MissingName()
        {
            // Arrange
            var localizer = CreateLocalizerMock();
            var service = new ProductService(null, null, null, localizer.Object);

            var product = new ProductViewModel
            {
                Name = "",
                Price = "10",
                Stock = "5"
            };

            // Act
            var errors = service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("MissingName", errors);
        }

        // Test: Prix manquant
        [Fact]
        public void CheckProductModelErrors_ShouldReturn_MissingPrice()
        {
            // Arrange
            var localizer = CreateLocalizerMock();
            var service = new ProductService(null, null, null, localizer.Object);

            var product = new ProductViewModel
            {
                Name = "Test",
                Price = "",
                Stock = "5"
            };

            // Act
            var errors = service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("MissingPrice", errors);
        }

        // Test: prix non numérique
        [Fact]
        public void CheckProductModelErrors_ShouldReturn_PriceNotANumber()
        {
            // Arrange
            var localizer = CreateLocalizerMock();
            var service = new ProductService(null, null, null, localizer.Object);
            var product = new ProductViewModel
            {
                Name = "Test",
                Price = "abc",
                Stock = "5"
            };
            // Act
            var errors = service.CheckProductModelErrors(product);
            // Assert
            Assert.Contains("PriceNotANumber", errors);
        }

        // Test: prix inférieur ou égal à zéro
        [Fact]
        public void CheckProductModelErrors_ShouldReturn_PriceNotGreaterThanZero()
        {
            // Arrange
            var localizer = CreateLocalizerMock();
            var service = new ProductService(null, null, null, localizer.Object);
            var product = new ProductViewModel
            {
                Name = "Test",
                Price = "0",
                Stock = "5"
            };
            // Act
            var errors = service.CheckProductModelErrors(product);
            // Assert
            Assert.Contains("PriceNotGreaterThanZero", errors);
        }

        // Test: Quantité manquante
        [Fact]
        public void CheckProductModelErrors_ShouldReturn_MissingQuantity()
        {
            // Arrange
            var localizer = CreateLocalizerMock();
            var service = new ProductService(null, null, null, localizer.Object);
            var product = new ProductViewModel
            {
                Name = "Test",
                Price = "10",
                Stock = ""
            };
            // Act
            var errors = service.CheckProductModelErrors(product);
            // Assert
            Assert.Contains("MissingQuantity", errors);
        }

        // Test: Stock non entier
        [Fact]
        public void CheckProductModelErrors_ShouldReturn_StockNotAnInteger()
        {
            // Arrange
            var localizer = CreateLocalizerMock();
            var service = new ProductService(null, null, null, localizer.Object);
            var product = new ProductViewModel
            {
                Name = "Test",
                Price = "10",
                Stock = "abc"
            };
            // Act
            var errors = service.CheckProductModelErrors(product);
            // Assert
            Assert.Contains("StockNotAnInteger", errors);
        }

        // Test: Stock inférieur ou égal à zéro
        [Fact]
        public void CheckProductModelErrors_ShouldReturn_StockNotGreaterThanZero()
        {
            // Arrange
            var localizer = CreateLocalizerMock();
            var service = new ProductService(null, null, null, localizer.Object);
            var product = new ProductViewModel
            {
                Name = "Test",
                Price = "10",
                Stock = "0"
            };
            // Act
            var errors = service.CheckProductModelErrors(product);
            // Assert
            Assert.Contains("StockNotGreaterThanZero", errors);
        }









      
    }
}