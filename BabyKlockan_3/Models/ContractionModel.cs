namespace BabyKlockan_3.Models;

public class ContractionModel
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    //Duration beräknas automatiskt varje gång den anropas, endast getter
    public TimeSpan Duration => EndTime - StartTime;
    public int Number { get; set; } //för att numrera varje värk
    public TimeSpan? RestTime { get; set; }

}
