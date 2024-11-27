using GetSchedule;
using misc;

namespace Programma{
    public class ReturnItem{

        //methode die alle mockdata meegeeft
        

        
        public static Compare CompareAdd(string email, DateTime datum, TimeSpan starttijd, TimeSpan eindtijd){
            Console.WriteLine("Beschikbaar op " + datum.ToString("d")+ " van " + starttijd + " tot " + eindtijd);
            return new Compare{
                            email = email,
                            datum = datum,
                            startTijd = starttijd,
                            eindTijd = eindtijd,
                        };
        }
        public static ScheduleItem SendItem(string status, string datetimeStart, string dateTimeEnd, string timeZone){
            return new ScheduleItem{
                            status= status,
                            start= new DateTimeTimeZone{
                                dateTime= DateTime.Parse(datetimeStart),
                                timeZone= timeZone
                            },
                            end= new DateTimeTimeZone{
                                dateTime= DateTime.Parse(dateTimeEnd),
                                timeZone= timeZone
                            }
                        };
        }
    }
}