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

        var online = true;

        var locatie = "";
        var tijdWeg = new TimeSpan(0);
                while(true){
                        Console.WriteLine("Meeting online? y/n");
                        var ans2 = Console.ReadLine();
                        if(ans2 == "y" || ans2 == "n"){
                            if(ans2 == "y"){
                                locatie = "Online";
                                online = true;
                            }
                            else{
                                online = false;
                                while(true){
                                    Console.WriteLine("Op welke locatie vind de meeting plaats?");
                                    var ans3 = Console.ReadLine();
                                    if(ans3 != "" && ans3 != null){
                                        locatie = ans3;
                                        break;
                                    }
                                    else{
                                        Console.WriteLine("Voer een antwoord in");
                                    }
                                }
                                    
                                    while(true){
                                        Console.WriteLine("Hoe lang is de Reistijd? uu:mm");
                                        var reistijd = Console.ReadLine();
                                        try{
                                            tijdWeg = TimeSpan.Parse(reistijd);
                                            break;
                                        }
                                        catch{
                                            Console.WriteLine("Voer een geldig antwoord in");
                                        }
                                    }
                                    //Reistijd in agenda zetten



                                    // PostTravelTime(gekozenTijdstip, tijdWeg, onderwerp, body);
                            }
                            break;
                        }
                        else{
                            Console.WriteLine("Voer een geldig antwoord in");
                        }
                    }
        


        //Emails en tijden van meeting verzamelen
        var postItem = SubProgramma.EmailPost(activeEmail);

        // Emails en tijden opsturen naar Outlook API, om te weten wie wanneer meetings heeft
        var ret = OutlookAPI.GetSchedule(postItem);

        //Uit de Outlook API data halen wie wanneer vrijgepland is
        var compareList = SubProgramma.FreeFromPlanning(ret);

        //Checken wie op dezelfde tijdstippen vrijgepland is
        var availability = SubProgramma.Availabilities(ret, compareList, time, tijdWeg);

        //Beschikbare tijdstippen printen, en keuze bevestigen
        var gekozenTijdstip = SubProgramma.PrintAvailability(availability);

        if(gekozenTijdstip.eindTijd - gekozenTijdstip.startTijd > time + (2 * tijdWeg)){
            gekozenTijdstip = SubProgramma.LongerThanPlanned(gekozenTijdstip, tijdWeg, time);
            Console.ReadLine();
        }

        //Keuze uitprinten
        Console.WriteLine(JsonSerializer.Serialize(gekozenTijdstip));

        //Event in agenda zetten en uitnodigingen sturen
        SubProgramma.PostEvent(onderwerp, body, gekozenTijdstip, online, locatie);
        
        
        
    }
}