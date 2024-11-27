using misc;

namespace GetSchedule{
    public class Post{
        public List<string> Schedules{get;set;}
        public DateTimeTimeZone StartTime{get;set;}
        public DateTimeTimeZone EndTime{get;set;}
        public string availabilityViewInterval{get;set;}

    }
}