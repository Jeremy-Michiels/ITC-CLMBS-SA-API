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
        public static ScheduleItem SendItem(string status, string datetimeStart, string dateTimeEnd, string timeZone, DateTime stDate, DateTime enDate){
            if((DateTime.Parse(datetimeStart) < stDate && DateTime.Parse(dateTimeEnd) > stDate) || (DateTime.Parse(datetimeStart) < enDate && DateTime.Parse(dateTimeEnd) > enDate) || (DateTime.Parse(datetimeStart) > stDate && DateTime.Parse(dateTimeEnd) < enDate) || (DateTime.Parse(datetimeStart) < stDate && DateTime.Parse(dateTimeEnd) > enDate)){
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
        else{
            return new ScheduleItem{};
        }
        }
    }
}