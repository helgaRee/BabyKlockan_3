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

    //nollställ
    public void ClearList()
    {
        contractionList.Clear();
    }
}
