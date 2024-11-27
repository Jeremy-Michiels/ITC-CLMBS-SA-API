using misc;

namespace GetSchedule{
    public class RetItem{
        public string scheduleId{get;set;}
        public string availabilityView{get;set;}
        public List<ScheduleItem> scheduleItems{get;set;}
        public WorkingHours workingHours{get;set;}
    }
}