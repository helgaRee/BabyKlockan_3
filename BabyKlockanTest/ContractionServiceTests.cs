using AutoMapper;
using BabyKlockan_3.Data;
using BabyKlockan_3.Mappers;
using BabyKlockan_3.Models;
using BabyKlockan_3.Services;
using BabyKlockanTest.TestHelpers;
using Microsoft.EntityFrameworkCore;

namespace BabyKlockanTest;

public class ContractionServiceTests : IDisposable
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ContractionServiceTests()
    {
        //anvädner DbContextHelper för att skapa en in-memory databas
        _context = DbContextHelper.GetInMemoryDbContext();
        //konfig Automapper med mappingprofile
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = new Mapper(config);
    }
    public void Dispose()
    {
        //rensar databasen efter varje test
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task AddContractionAsync_ShouldSaveContractionToDatabase()
    {
        //arrange
        var startTime = DateTime.Now;
        var endTime = DateTime.Now.AddMinutes(10);
        var service = new ContractionService(_context);

        //act - anropar metoden från servicen
        await service.AddContractionAsync(startTime, endTime);

        //assert
        var contractions = await _context.Contractions.ToListAsync();
        Assert.Single(contractions); //förväntan om att en värk lagts till
        Assert.Equal(startTime, contractions[0].StartTime); //kontrollera starttiden
        Assert.Equal(endTime, contractions[0].EndTime);
        Assert.Equal(endTime - startTime, contractions[0].Duration); //kontrollera längden
    }

    [Fact]
    public async Task GetAllContractionsAsync_ShouldGetAllContractionsFromDatabase()
    {
        //arrange
        var service = new ContractionService(_context);


        //act
        await service.GetAllContractionsAsync();

        //assert
        var contractions = await _context.Contractions.ToListAsync();
    }

    [Fact]
    public async Task RemoveContractionByIdAsync_ShouldRemoveContractionByIdFromDatabase()
    {
        //arrange - hämta in min service via context för att komma åt dess metoder
        var service = new ContractionService(_context);

        //lägg till testdata 
        var contractionId = Guid.NewGuid(); //skapa ett unikt ID för datan
        //skapa instans av contraction
        var contraction = new ContractionModel
        {
            Id = contractionId,

        };
        //lägg till contraction till context
        _context.Contractions.Add(contraction);
        //spara ändringar till db
        await _context.SaveChangesAsync();


        //act - anropa metoden
        await service.RemoveContractionByIdAsync(contractionId);



        //assert - kontroll att värken tagits bort från db - leta efter ID
        var removedContraction = await _context.Contractions.FindAsync(contractionId);
        //förväntar att C inte finns
        Assert.Null(removedContraction);

    }

    [Fact]
    public async Task ClearListAsync_ShouldDeleteAllContractionsInDatabase()
    {
        //arrange - hämta service genom att skapa ny, för att komma åt metoder (via context)
        var service = new ContractionService(_context);

        //skapa contraction object
        var contraction1Model = new ContractionModel
        {
            Id = Guid.NewGuid(),
            Number = 1,
            StartTime = DateTime.Now,
            EndTime = DateTime.Now,
            Duration = TimeSpan.Zero,
            RestTime = TimeSpan.Zero,
        };

        var contraction2Model = new ContractionModel
        {
            Id = Guid.NewGuid(),
            Number = 2,
            StartTime = DateTime.Now.AddMinutes(10),
            EndTime = DateTime.Now.AddMinutes(10),
            Duration = TimeSpan.Zero,
            RestTime = TimeSpan.Zero,
        };
        var contraction1 = _mapper.Map<ContractionModel>(contraction1Model);
        var contraction2 = _mapper.Map<ContractionModel>(contraction2Model);

        //lägg till objekten i databasen
        _context.Contractions.Add(contraction1);
        _context.Contractions.Add(contraction2);
        await _context.SaveChangesAsync();

        //act - anropa metoden för att rensa listan jag precis "skapat"!
        await service.ClearListAsync();

        //assert - komtrollera att inga objekt finns i databasen
        var clearedListCount = await _context.Contractions.CountAsync(); //kontroll
        Assert.Equal(0, clearedListCount); //förväntat utfall
    }


    [Fact]
    public async Task GetContractionsPerTenMinutes_ShouldGetAmountOfContractionsEveryTenMinutes()
    {
        //arrange
        var service = new ContractionService(_context);


        var contraction1model = new ContractionModel
        {
            Id = Guid.NewGuid(),
            Number = 1,
            StartTime = DateTime.Now,
            EndTime = DateTime.Now,
        };
        var contraction2model = new ContractionModel
        {
            Id = Guid.NewGuid(),
            Number = 2,
            StartTime = DateTime.Now.AddMinutes(-5),
            EndTime = DateTime.Now.AddMinutes(-5),
        };
        var contraction3model = new ContractionModel
        {
            Id = Guid.NewGuid(),
            Number = 3,
            StartTime = DateTime.Now.AddMinutes(-8),
            EndTime = DateTime.Now.AddMinutes(-8)
        };
        var contraction4model = new ContractionModel
        {
            Id = Guid.NewGuid(),
            Number = 4,
            StartTime = DateTime.Now.AddMinutes(-11),
            EndTime = DateTime.Now.AddMinutes(-11)

        };
        var contraction5model = new ContractionModel
        {
            Id = Guid.NewGuid(),
            Number = 5,
            StartTime = DateTime.Now.AddMinutes(-15),
            EndTime = DateTime.Now.AddMinutes(-15)

        };
        var contraction6model = new ContractionModel
        {
            Id = Guid.NewGuid(),
            Number = 6,
            StartTime = DateTime.Now.AddMinutes(-20),
            EndTime = DateTime.Now.AddMinutes(-20)
        };

        var contraction1 = _mapper.Map<ContractionModel>(contraction1model);
        var contraction2 = _mapper.Map<ContractionModel>(contraction2model);
        var contraction3 = _mapper.Map<ContractionModel>(contraction3model);
        var contraction4 = _mapper.Map<ContractionModel>(contraction4model);
        var contraction5 = _mapper.Map<ContractionModel>(contraction5model);
        var contraction6 = _mapper.Map<ContractionModel>(contraction6model);

        _context.Contractions.Add(contraction1);
        _context.Contractions.Add(contraction2);
        _context.Contractions.Add(contraction3);
        _context.Contractions.Add(contraction4);
        _context.Contractions.Add(contraction5);
        _context.Contractions.Add(contraction6);
        await _context.SaveChangesAsync();

        //act 
        var result = service.GetContractionsPerTenMinutes();

        //assert - förväntas 3 värkar
        Assert.Equal(3, result);
    }
}
