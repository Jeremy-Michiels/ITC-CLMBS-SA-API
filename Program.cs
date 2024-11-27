using System.Text.Json;
using Programma;
using RegexExamples;

public class Program
{
    private static void Main(string[] args)
    {
        string activeEmail = "";
        while(true){
            Console.WriteLine("Wat is uw email?");
            activeEmail = Console.ReadLine();
            if(RegexUtilities.IsValidEmail(activeEmail)){
                break;
            }
            else{
                Console.WriteLine("Vul een geldig emailadres in");
            }
        }
        Console.WriteLine("Waarover gaat de meeting?");
        var onderwerp = Console.ReadLine();
        Console.WriteLine("Wat wilt u opsturen naar de uitgenodigden");
        var body = Console.ReadLine();

        TimeSpan time = new TimeSpan();
        while(true){
            Console.WriteLine("Hoe lang duurt de meeting? uu:mm");
        var tijdR = Console.ReadLine();
        try{
            time = TimeSpan.Parse(tijdR);
            break;

        }
        catch{
            Console.WriteLine("Geef een geldig tijdformat op");
        }
        
        }
        


        //Emails en tijden van meeting verzamelen
        var postItem = SubProgramma.EmailPost(activeEmail);

        // Emails en tijden opsturen naar Outlook API, om te weten wie wanneer meetings heeft
        var ret = OutlookAPI.GetSchedule(postItem);

        //Uit de Outlook API data halen wie wanneer vrijgepland is
        var compareList = SubProgramma.FreeFromPlanning(ret);

        //Checken wie op dezelfde tijdstippen vrijgepland is
        var availability = SubProgramma.Availabilities(ret, compareList, time);

        //Beschikbare tijdstippen printen, en keuze bevestigen
        var gekozenTijdstip = SubProgramma.PrintAvailability(availability);

        //Keuze uitprinten
        Console.WriteLine(JsonSerializer.Serialize(gekozenTijdstip));

        //Event in agenda zetten en uitnodigingen sturen
        SubProgramma.PostEvent(onderwerp, body, gekozenTijdstip);
        
        
        
    }
}