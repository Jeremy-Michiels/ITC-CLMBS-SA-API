using misc;
using Programma;
using RegexExamples;

public class SubProgramma{


    //Methode die alle beschikbare tijden van de verschillende mensen opstelt
    public static List<Compare> FreeFromPlanning(GetSchedule.Return ret, DateTime startDate, DateTime endDate){
        var compareList = new List<Compare>();
        var disDates = new List<DateTime>();
        disDates.Add(startDate);
        var btw = startDate.AddDays(1);
        while(btw.Date > endDate.Date){
            disDates.Add(btw);
            btw = btw.AddDays(1);
        }
        if(startDate.Date != endDate.Date){
            disDates.Add(endDate);
        }
        // var disDates = ret.value.Select(x => x.scheduleItems.Select(x => x.start.dateTime.Date).Distinct().ToList()).OrderByDescending(y => y.Count()).FirstOrDefault();
        foreach(var item in ret.value){
            Console.WriteLine("Beschikbaarheid "+ item.scheduleId + ":");
            if(item.scheduleItems.Count() < 1){
                Console.WriteLine("Vrijgepland op de aangegeven momenten.");
                foreach(var date in disDates){
                    compareList.Add(ReturnItem.CompareAdd(item.scheduleId, date,item.workingHours.startTijd,item.workingHours.eindTijd));
                }
            }
            else{
                int i = 1;
                GetSchedule.ScheduleItem prev = null;
                foreach(var sched in item.scheduleItems){
                    if(sched.end.dateTime.TimeOfDay.Hours == 16){
                    }
                    if(prev == null && i == 1){
                        if(sched.start.dateTime.TimeOfDay > item.workingHours.startTijd){
                            compareList.Add(ReturnItem.CompareAdd(item.scheduleId, sched.start.dateTime.Date,item.workingHours.startTijd,sched.start.dateTime.TimeOfDay));
                        }
                    }
                    
                    else {
                        if(sched.start.dateTime.Date == prev.end.dateTime.Date){
                            if(sched.start.dateTime.TimeOfDay > prev.end.dateTime.TimeOfDay){
                                compareList.Add(ReturnItem.CompareAdd(item.scheduleId, prev.end.dateTime.Date,prev.end.dateTime.TimeOfDay,sched.start.dateTime.TimeOfDay));                                
                                if(i == item.scheduleItems.Count() && sched.end.dateTime.TimeOfDay < item.workingHours.eindTijd){
                                    compareList.Add(ReturnItem.CompareAdd(item.scheduleId, sched.start.dateTime.Date,sched.end.dateTime.TimeOfDay,item.workingHours.eindTijd));                                    
                                }
                            }
                        }
                        else{
                            if(prev.end.dateTime.TimeOfDay < item.workingHours.eindTijd){
                                compareList.Add(ReturnItem.CompareAdd(item.scheduleId, prev.end.dateTime.Date,prev.end.dateTime.TimeOfDay,item.workingHours.eindTijd));                                
                            }
                            if(sched.start.dateTime.TimeOfDay > item.workingHours.startTijd){
                                compareList.Add(ReturnItem.CompareAdd(item.scheduleId, sched.start.dateTime.Date,item.workingHours.eindTijd,sched.start.dateTime.TimeOfDay));                                
                            }
                        }
                    }


                    prev = sched;
                    i++;
                }
            }
            Console.WriteLine("");
        }
        return compareList;
    }


    
    
    
    
    
    //Methode die kijkt of er overlappende beschikbaarheden zijn
    public static List<Availability> Availabilities(GetSchedule.Return ret, List<Compare> compareList, TimeSpan time, TimeSpan tijdWeg){
        var users = ret.value.Select(x => x.scheduleId).Distinct().ToList();
        var availability = new List<Availability>();
        foreach(var item in compareList){
            if(availability.Where(x => x.startTijd == item.startTijd && x.eindTijd == item.eindTijd).Count() == 0){
            Console.WriteLine("Tijd checken: " + item.datum + " van " + item.startTijd + " tot " + item.eindTijd);
            Console.WriteLine(item.email + " is beschikbaar");
            var sub = new Availability(){
                datum = item.datum,
                startTijd = item.startTijd,
                eindTijd = item.eindTijd,
                attendees = new List<string>(){
                    item.email
                },
                genoegTijd = item.eindTijd - item.startTijd >= time,
                genoegMetReistijd = item.eindTijd - item.startTijd >= time + tijdWeg
            };
            foreach(var user in users.Where(x => x != item.email)){

                int i = 0;
                var planned = ret.value.SingleOrDefault(x => x.scheduleId == user).scheduleItems;

                foreach (var busyTimes in planned){
                    //Checken of de tijden van de meeting overlappen met deze vrijgeroosterde tijd
                    if((busyTimes.start.dateTime.TimeOfDay < item.startTijd && busyTimes.end.dateTime.TimeOfDay > item.startTijd) || (busyTimes.start.dateTime.TimeOfDay < item.eindTijd && busyTimes.end.dateTime.TimeOfDay > item.eindTijd) || (busyTimes.start.dateTime.TimeOfDay > item.startTijd && busyTimes.end.dateTime.TimeOfDay < item.eindTijd) || (busyTimes.start.dateTime.TimeOfDay < item.startTijd && busyTimes.end.dateTime.TimeOfDay > item.eindTijd)){
                                    i = 0;
                                    break;

                        
                    }
                    else{
                        i++;

                    }
                }
                if(i == planned.Count()){
                    sub.attendees.Add(user);
                    Console.WriteLine(user + " is beschikbaar");
                    if(sub.attendees.Count() == users.Count()){
                        sub.allAvailable = true;
                    }
                }
                else{
                    Console.WriteLine(user + " is niet beschikbaar");
                }
            }
            availability.Add(sub);
            Console.WriteLine("");
            }
            
                
        }
        return availability;
    }



    
    
    
    
    
    //Methode die alle beschikbare en gedeeltelijk beschikbare tijden oplevert.
    public static Availability PrintAvailability(List<Availability> availability){
        int i = 1;
        if(availability.Where(x => x.allAvailable == false).Count() > 0){
        Console.WriteLine("Semi beschikbare tijden");
        foreach(var ava in availability.Where(x => x.allAvailable == false)){
            Console.Write(i + ". " + ava.datum.ToString("d") + "   " + ava.startTijd + "-" + ava.eindTijd + "   Beschikbaar: " );
            foreach(var avaUser in ava.attendees){
                Console.Write(avaUser);
                if(ava.attendees.Last() != avaUser){
                    Console.Write(" + ");
                }
            }
            Console.WriteLine(" Genoeg tijd: " + (ava.genoegTijd ? "Ja" : "Nee") + "    Genoeg tijd met reistijd: " + (ava.genoegMetReistijd ? "Ja" : "Nee"));
            ava.id = i;
            i++;
        }
        }
        if(availability.Where(x => x.allAvailable && x.genoegTijd == false).Count() > 0){
        Console.WriteLine("");
        Console.WriteLine("Te korte tijden: ");
        foreach(var ava in availability.Where(x => x.allAvailable && x.genoegTijd == false)){
            Console.WriteLine(i + ". " + ava.datum.ToString("d") + "   " + ava.startTijd + "-" + ava.eindTijd);
            ava.id = i;
            i++;
        }
        }

        if(availability.Where(x => x.allAvailable && x.genoegMetReistijd == false && x.genoegTijd == true).Count() > 0){
        Console.WriteLine("");
        Console.WriteLine("Te kort met reistijd meegenomen");
        foreach(var ava in availability.Where(x => x.allAvailable && x.genoegMetReistijd == false && x.genoegTijd == true)){
            Console.WriteLine(i + ". " + ava.datum.ToString("d") + "   " + ava.startTijd + "-" + ava.eindTijd);
            ava.id = i;
            i++;
        }
        }





        if(availability.Where(x => x.allAvailable && x.genoegTijd && x.genoegMetReistijd).Count() > 0){
        Console.WriteLine("");
        Console.WriteLine("Beschikbare tijden: ");
        
        
        foreach(var ava in availability.Where(x => x.allAvailable && x.genoegTijd && x.genoegMetReistijd)){
            Console.WriteLine(i + ". " + ava.datum.ToString("d") + "   " + ava.startTijd + "-" + ava.eindTijd);
            ava.id = i;
            i++;
        }
        }
        Console.WriteLine("");
        

        while(true){   
            Console.WriteLine("Welke tijd wilt u selecteren?");
            var selectedTime = Console.ReadLine();
            try{
                int nummer = int.Parse(selectedTime);
                if(nummer > 0 && nummer <= availability.OrderBy(x => x.id).Count()){
                    var gekozen = availability.OrderBy(x => x.id).Skip(nummer - 1).First();
                    return gekozen;
                }
                else{
                    throw new Exception();
                }
            }
            catch{
                Console.WriteLine("Voer een geldig nummer in");
            }
        }

    }


    //Vraagt om alle emails van mensen die tot de meeting behoren, en de tijden van de meeting, en formateert dit voor de Outlook API call
    public static GetSchedule.Post EmailPost(string activeEmail){
        var emails = new List<string>();
        emails.Add(activeEmail);
        while (true)
        {
            Console.WriteLine("Schrijf het mailadres op van een van de uitgenodigden");
            var mail = Console.ReadLine();
            if (!RegexUtilities.IsValidEmail(mail))
            {
                Console.WriteLine("Voer een geldig emailadres in");
            }
            else
            {
                emails.Add(mail);
                Console.WriteLine("Wilt u nog een emailadres toevoegen? y/n");
                var yn = Console.ReadLine();
                if (yn == "n")
                {
                    break;
                }
            }
        }
        DateTime startDate = new DateTime();
        DateTime endDate = new DateTime();
        while (true)
        {
            Console.WriteLine("Vanaf welke datum kan de meeting plaatsvinden?");
            var startdatum = Console.ReadLine();
            try
            {
                startDate = DateTime.Parse(startdatum);
                break;
            }
            catch
            {
                Console.WriteLine("Voer een geldige datum in");
            }
        }
        while (true)
        {
            Console.WriteLine("Tot welke datum kan de meeting plaatsvinden?");
            var einddatum = Console.ReadLine();
            try
            {
                endDate = DateTime.Parse(einddatum);
                break;
            }
            catch
            {
                Console.WriteLine("Voer een geldige datum in");
            }
        }

        var postItem = new GetSchedule.Post()
        {
            Schedules = emails,
            StartTime = new DateTimeTimeZone
            {
                dateTime = startDate,
                timeZone = "W. Europe Standard Zone"
            },
            EndTime = new DateTimeTimeZone
            {
                dateTime = endDate,
                timeZone = "W. Europe Standard Zone"
            },
            availabilityViewInterval = "15",
        };
        return postItem;
    }


    public static PostEvent.Post PItem(Availability meeting, string subject, string body, bool online, string location, string showAs){
        var item = new PostEvent.Post(){
            subject = subject,
            body = new PostEvent.BodyType(){
                contentType = "html",
                content = body,
            },
            start = new DateTimeTimeZone(){
                dateTime = DateTime.Parse(meeting.datum.ToString("d") + " " + meeting.startTijd),
                timeZone = "W. Europe Standard Time",
            },
            end = new DateTimeTimeZone(){
                dateTime = DateTime.Parse(meeting.datum.ToString("d") + " " + meeting.eindTijd),
                timeZone = "W. Europe Standard Time",
            },
            location = new PostEvent.Location(){
                displayName = location
            },
            attendees = new List<PostEvent.Attendees>(),
            allowNewTimeProposals = true,
            isOnlineMeeting = online,
            onlineMeetingProvider = online ? "teamsForBusiness" : null,
            showAs = showAs
        };

        foreach(var at in meeting.attendees){
        
            item.attendees.Add(new PostEvent.Attendees(){
                type = "required",
                emailAddress = new PostEvent.MailName(){
                    address = at,
                    name = "naam",
                }
            });
        }

        return item;
    }


    public static void PostEvent(string onderwerp, string body, misc.Availability gekozenTijdstip, bool online, string locatie, TimeSpan tijdWeg){

        while(true){
            Console.WriteLine("Uitnodiging versturen? y/n");
            var ans = Console.ReadLine();
            if(ans == "y" || ans == "n"){
                if(ans == "y"){
                    
                    
                    
                    //Data formatteren voor Outlook API
                    var pEvent = SubProgramma.PItem(gekozenTijdstip, onderwerp, body, online, locatie, "busy");
                    PostTravelTime(gekozenTijdstip, tijdWeg , onderwerp, body);
                    var postEvent = OutlookAPI.PostEvent(pEvent);
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
    public static void PostTravelTime(misc.Availability gekozenTijdstip, TimeSpan tijdWeg, string onderwerp, string body){
        var voortijd = new Availability(){
                                        datum = gekozenTijdstip.datum,
                                        startTijd = gekozenTijdstip.startTijd - tijdWeg,
                                        eindTijd = gekozenTijdstip.startTijd,
                                        attendees = gekozenTijdstip.attendees,
                                        allAvailable = gekozenTijdstip.allAvailable,
                                        genoegTijd = gekozenTijdstip.genoegTijd,
                                    };
                                    var natijd = new Availability(){
                                        datum = gekozenTijdstip.datum,
                                        startTijd = gekozenTijdstip.eindTijd,
                                        eindTijd = gekozenTijdstip.eindTijd + tijdWeg,
                                        attendees = gekozenTijdstip.attendees,
                                        allAvailable = gekozenTijdstip.allAvailable,
                                        genoegTijd = gekozenTijdstip.genoegTijd,
                                    };
                                    Console.WriteLine("Heen: Onderweg van " + voortijd.startTijd + " tot " + voortijd.eindTijd);
                                    Console.WriteLine("Terug: Onderweg van " + natijd.startTijd + " tot " + natijd.eindTijd);
                                    Console.WriteLine("");
                                    var ond = "Reistijd" + onderwerp;
                                    var bod = "Reistijd" + body;
                                    var voor = PItem(voortijd, ond, bod, false, "", "workingElsewhere");
                                    var na = PItem(natijd, ond, bod, false, "", "workingElsewhere");

                                    var pvoor = OutlookAPI.PostEvent(voor);
                                    if(pvoor != null){
                                        Console.WriteLine("Heenweg ingepland");
                                    }
                                    var pna = OutlookAPI.PostEvent(na);
                                    if(pna != null){
                                        Console.WriteLine("Terugweg ingepland");
                                    }
    }

    public static misc.Availability LongerThanPlanned(misc.Availability gekozenTijdstip, TimeSpan tijdWeg, TimeSpan time){
        var maxEndTime = gekozenTijdstip.eindTijd - tijdWeg;
        var minStartTime = gekozenTijdstip.startTijd + tijdWeg;
        var MaxStartTime = maxEndTime - time;
        Console.WriteLine("Minimum starttijd: " + minStartTime);
        Console.WriteLine("Maximum starttijd: " + MaxStartTime);
        while(true){
            Console.WriteLine("Kies een starttijdstip tussen deze twee tijdstippen in. uu:mm");
            var tijdstip = Console.ReadLine();
            try{
                var tParse = TimeSpan.Parse(tijdstip);
                if(tParse >= minStartTime && tParse <= MaxStartTime){
                    gekozenTijdstip.startTijd = tParse;
                    gekozenTijdstip.eindTijd = gekozenTijdstip.startTijd + time;
                    return gekozenTijdstip;
                    
                }
                else{
                    throw new Exception();
                }
            }
            catch{
                Console.WriteLine("Voer een geldig tijdstip in");
            }
        }
        
        
    }
}