using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.IO;
using Xunit;

public class ProductIntegrationWithMoqTests
{
    private readonly P3Referential _context;
    private readonly ProductRepository _repo;
    private readonly ProductService _service;

    public ProductIntegrationWithMoqTests()
    {
        var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.Test.json")
    .Build();

        var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(configuration.GetConnectionString("P3Referential"))
            .Options;

        _context = new P3Referential(options, configuration);

        //  crée les tables dans P3_IntegrationTestDB
        _context.Database.EnsureCreated();

        // 3. test repository vrai
        _repo = new ProductRepository(_context);


        var fakeOrderRepo = new Mock<IOrderRepository>();

        // faux panier
        var fakeCart = new Mock<ICart>();

        //  messages localisés
        var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();

        mockLocalizer
            .Setup(l => l[It.IsAny<string>()])
            .Returns((string key) => new LocalizedString(key, key));

        // produit service vrai
        _service = new ProductService(
            fakeCart.Object,
            _repo,
            fakeOrderRepo.Object,
            mockLocalizer.Object
        );
    }

    // Tester la connexion à la base de données
    [Fact]
    public void SQL_Test_DBConnection_Works()
    {
        Assert.True(_context.Database.CanConnect());
    }

    // Ajouter un produit
    [Fact]
    public void SQL_AddProduct_Should_Appear_For_Client()
    {
        // Arrange
        var vm = new ProductViewModel
        {
            Name = "Ordinateur",
            Price = "100",
            Stock = "5"
        };

        // Act (Admin)
        _service.SaveProduct(vm);

        // Assert (Client)
        var list = _service.GetAllProductsViewModel();
        Assert.Contains(list, p => p.Name == "Ordinateur");
    }

    [Fact]
    public void SQL_DeleteProduct_Should_Disappear_From_Client_List()
    {
        // Arrange  ajouter un produit directement en SQL
        var p = new Product
        {
            Name = "Ordinateur supprimer",
            Price = 50,
            Quantity = 2,
            Description = "",
            Details = ""
        };

        _context.Product.Add(p);
        _context.SaveChanges();
        int id = p.Id;

        // Act
        _service.DeleteProduct(id);

        // Assert
        var list = _service.GetAllProductsViewModel();
        Assert.DoesNotContain(list, x => x.Id == id);
    }




}
