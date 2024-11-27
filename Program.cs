using System.Text.Json;
using GetSchedule;
using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Functions.YearFrac;
using Microsoft.Graph.Models;
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
        var gekozenTijstip = SubProgramma.PrintAvailability(availability);

        //Keuze uitprinten
        Console.WriteLine(JsonSerializer.Serialize(gekozenTijstip));

        
        while(true){
            Console.WriteLine("Uitnodiging versturen? y/n");
            var ans = Console.ReadLine();
            if(ans == "y" || ans == "n"){
                if(ans == "y"){
                    var online = true;

                    var locatie = "";
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

                                    Console.WriteLine("Hoe lang is de Reistijd? uu:mm");
                                    var reistijd = Console.ReadLine();
                                
                                
                            }
                            break;
                        }
                        else{
                            Console.WriteLine("Voer een geldig antwoord in");
                        }
                    }
                    
                    
                    //Data formatteren voor Outlook API
                    
                    var pItem = SubProgramma.PItem(gekozenTijstip, onderwerp, body, online, locatie);
                    var postEvent = OutlookAPI.PostEvent(pItem);
                    if(postEvent == null){
                        Console.WriteLine("Onbekende fout, probeer later opnieuw.");
                    }
                    else{
                        Console.WriteLine("Afspraak gemaakt");
                        Console.WriteLine("Onderwerp: " + postEvent.subject);
                        Console.Write("Uitgenodigden: " );
                        foreach(var ev in postEvent.attendees){
                            Console.Write(ev.emailAddress.address);
                            if(ev != postEvent.attendees.Last()){
                                Console.Write(" + ");
                            }
                            else{
                                Console.WriteLine("");
                            }
                        }
                        Console.WriteLine("Locatie: " + postEvent.location.displayName);
                        Console.WriteLine("Startdatum: " + postEvent.start.dateTime);
                        Console.WriteLine("Einddatum: " + postEvent.end.dateTime);
                        Console.ReadLine();
                    }
                }
                else{
                    Console.WriteLine("Uitnodiging word niet verstuurd.");
                }
                break;
            }
            else{
                Console.WriteLine("Voer een geldig antwoord in");
            }
        }
        
    }
}