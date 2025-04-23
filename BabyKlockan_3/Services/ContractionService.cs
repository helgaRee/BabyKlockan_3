using BabyKlockan_3.Data;
using BabyKlockan_3.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BabyKlockan_3.Services;
//Denna service ska hantera listan med värkar och låter mig lägga till nya, och sparar dem.
public class ContractionService
{
    //skapar lista för ContractionData
    //public List<ContractionModel> contractionList { get; set; } = new();
    private readonly DataContext _context;

    public ContractionService(DataContext context)
    {

        _context = context;
    }


    /// <summary>
    /// Metod för att lägga till en värk, skicka med start och sluttid från modell
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    public async Task AddContractionAsync(DateTime startTime, DateTime endTime)
    {
        var duration = endTime - startTime;
        TimeSpan? restTime = null;
        var contractions = await _context.Contractions.ToListAsync();

        if (contractions.Count() > 0)
        {
            //var previousContractionEndTime = contractions.Last().EndTime;
            //restTime = startTime - previousContractionEndTime;

            var previousContraction = contractions
                .OrderByDescending(c => c.EndTime)
                .FirstOrDefault();

            if (previousContraction != null)
            {
                restTime = startTime - previousContraction.EndTime;
            }
        }

        //skapa en ny värk genom modellen som ett object
        var contraction = new ContractionModel
        {
            Number = contractions.Count() + 1,
            StartTime = startTime,
            EndTime = endTime,
            Duration = endTime - startTime,
            RestTime = restTime,
        };
        //Efter att ha skapat en värk, lägg till den i listan
        await _context.Contractions.AddAsync(contraction);
        await _context.SaveChangesAsync();


    }

    /// <summary>
    /// Metod för att hämta alla värkar
    /// </summary>
    /// <returns></returns>
    public async Task<List<ContractionModel>> GetAllContractionsAsync()
    {
        var contractions = await _context.Contractions.ToListAsync();
        return contractions;
    }

    public async Task<List<ContractionModel>> GetContractionsLastHourAsync()
    {
        //jämför tiden nu med minus 60 min
        //skapa variabel för tiden för en timma sedan
        var OneHourAgo = DateTime.Now.AddHours(-1);
        //hämta alla värkar från senaste timmen från db
        var recentContractions = await _context.Contractions
            .Where(c => c.StartTime >= OneHourAgo)
            .ToListAsync();

        return recentContractions;
    }

    /// <summary>
    /// Metod för att ta bort en värk av dess ID
    /// </summary>
    /// <param name="id"></param>
    public async Task RemoveContractionByIdAsync(Guid id)
    {
        Debug.WriteLine("Inne i servicen");
        var contraction = _context.Contractions.FirstOrDefault(c => c.Id == id);
        if (contraction != null)
        {
            _context.Contractions.Remove(contraction);
            await _context.SaveChangesAsync();
            Debug.WriteLine("värk borttagen ur db");
        }
    }


    /// <summary>
    /// Metod för att hämta antalet värkar som samlats in under de senaste 10 min
    /// </summary>
    /// <returns></returns>
    public double GetContractionsPerTenMinutes()
    {
        //filtrera ut de värkar som inträffad under de senaste 10 minuterna
        var recentContractions = _context.Contractions
            .Where(c => c.StartTime >= DateTime.Now.AddMinutes(-10))
            .ToList();

        //returnera antalet värkar som inträffade under de senaste 10 minuiterna
        return recentContractions.Count;
    }


    /// <summary>
    /// Hämtar data från db till minnet, och sen gör beräkning
    /// </summary>
    /// <returns></returns>
    public double GetAverageDuration()
    {
        //if (_context.Contractions.Count() == 0) return 0;
        //return _context.Contractions.Average(c => c.Duration.TotalMinutes);

        //Hämta listan i minnet med asEnumerable
        var contractions = _context.Contractions.AsEnumerable();
        if (!contractions.Any()) return 0;

        //beräkna genomsnittliga durationen efter att data hämtats
        return contractions.Average(c => c.Duration.TotalSeconds);


    }


    /// <summary>
    /// Beräknar den genomsnittliga vilan mellan värkar
    /// </summary>
    /// <returns></returns>
    public double GetAverageRestTime()
    {

        //if (_context.Contractions.Count() < 2) return 0;
        //var restTimes = _context.Contractions
        //    .Select(c => c.RestTime)
        //    .Where(r => r.HasValue)
        //    .Select(r => r!.Value.TotalMinutes);
        //return restTimes.Any() ? restTimes.Average() : 0;

        var restTimes = _context.Contractions
            .Where(c => c.RestTime.HasValue)
            .AsEnumerable() //hämtar data till klient/minnet
            .Select(c => c.RestTime!.Value.TotalSeconds);

        if (!restTimes.Any()) return 0;

        return restTimes.Average();

    }


    /// <summary>
    /// Nollställer listan med värkar
    /// </summary>
    public async Task ClearListAsync()
    {
        //hämta alla värkar från db och gör tille n lista
        var allContractions = _context.Contractions.ToList();
        //ta bort alla värkar från databasen
        _context.Contractions.RemoveRange(allContractions);
        await _context.SaveChangesAsync();
    }
}
