using BabyKlockan_3.Models;
using System.Diagnostics;

namespace BabyKlockan_3.Services;
//Denna service ska hantera listan med värkar och låter mig lägga till nya, och sparar dem.
public class ContractionService
{
    //skapar lista för ContractionData
    public List<ContractionModel> contractionList { get; set; } = new();




    //Metod för att lägga till en värk, skicka med start och sluttid från modell
    public void AddContraction(DateTime startTime, DateTime endTime)
    {
        //skapa variabel för duration
        var duration = endTime - startTime;
        TimeSpan? restTime = null;

        if (contractionList.Count > 0)
        {
            var previousContractionEndTime = contractionList.Last().EndTime;
            restTime = startTime - previousContractionEndTime;

        }

        //skapa en ny värk genom modellen som ett object
        var contraction = new ContractionModel
        {
            Number = contractionList.Count + 1,
            StartTime = startTime,
            EndTime = endTime,
            Duration = endTime - startTime,
            RestTime = restTime,
        };
        //Efter att ha skapat en värk, lägg till den i listan
        contractionList.Add(contraction);
    }


    public List<ContractionModel> GetAllContractions()
    {
        return contractionList;
    }


    public void RemoveContractionById(Guid id)
    {
        Debug.WriteLine("Inne i servicen");
        var contraction = contractionList.FirstOrDefault(c => c.Id == id);
        if (contraction != null)
        {
            contractionList.Remove(contraction);
        }
        Debug.WriteLine("värk borttagen via service");
    }

    //beräkna antal värkar per 10 minut
    public double GetContractionsPerTenMinutes()
    {
        //filtrera ut de värkar som inträffad under de senaste 10 minuterna
        var recentContractions = contractionList
            .Where(c => c.StartTime >= DateTime.Now.AddMinutes(-10))
            .ToList();

        //returnera antalet värkar som inträffade under de senaste 10 minuiterna
        return recentContractions.Count;
    }


    //beräkna medellängd på värkar
    public double GetAverageDuration()
    {
        if (contractionList.Count == 0) return 0;
        return contractionList.Average(c => c.Duration.TotalMinutes);
    }

    //beräkna genomsnittlig vila mellan värkar
    public double GetAverageRestTime()
    {
        if (contractionList.Count < 2) return 0;
        var restTimes = contractionList.Select(c => c.RestTime).Where(r => r.HasValue).Select(r => r.Value.TotalMinutes);
        return restTimes.Any() ? restTimes.Average() : 0;
    }


    //nollställ
    public void ClearList()
    {
        contractionList.Clear();
    }
}
