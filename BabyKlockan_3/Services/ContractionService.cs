using BabyKlockan_3.Models;

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
        //skapa en ny värk genom modellen som ett object
        var contraction = new ContractionModel
        {
            StartTime = startTime,
            EndTime = endTime,
            Number = contractionList.Count + 1,
            RestTime = contractionList.Count > 0 //om det redan finns en värk, räkna ut tiden mellan föregående och nuvarande
                    ? endTime - contractionList.Last().StartTime : null,
        };
        //Efter att ha skapat en värk, lägg till den i listan
        contractionList.Add(contraction);
    }


    public List<ContractionModel> GetAllContractions()
    {
        return contractionList;
    }




    //nollställ
    public void ClearList()
    {
        contractionList.Clear();
    }
}
