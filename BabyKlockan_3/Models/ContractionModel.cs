using System.ComponentModel.DataAnnotations;

namespace BabyKlockan_3.Models;

public class ContractionModel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid(); //genererar ett unikt ID
    public int Number { get; set; } //för att numrera varje värk
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    //Duration beräknas automatiskt varje gång den anropas, endast getter
    public TimeSpan Duration { get; set; }
    public TimeSpan? RestTime { get; set; }

}
