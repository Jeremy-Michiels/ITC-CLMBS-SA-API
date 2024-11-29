using misc;

namespace Programma{
    
    //Doet Outlook API na
    public class OutlookAPI{

        // Doet GetSchedule functie van Outlook na
        // Haalt alle meetinggegevens op van de personen die op zijn gegeven
        public static GetSchedule.Return GetSchedule(GetSchedule.Post postItem){
            var ret = new GetSchedule.Return(){
                odataContext = "https://graph.microsoft.com/v1.0/$metadata#users('f2e3093c-1e15-44b3-833d-e976b257e0c6')/messages",
                value = new List<GetSchedule.RetItem>(){
                }
            };

        foreach(var item in postItem.Schedules.Where(x => x != "jeremy.michiels@clmbs.nl" && x != "erwin.oenema@silverant.nl" && x != "jacco.schonewille@clmbs.nl")){
            ret.value.Add(new GetSchedule.RetItem(){
                scheduleId = item,
                availabilityView = "00000000000000000000000000000000",
                scheduleItems = new List<GetSchedule.ScheduleItem>(),
                workingHours = new WorkingHours{
                    daysOfWeek = new List<string>(){
                    "monday",
                    "tuesday",
                    "wednesday",
                    "thursday",
                    "friday"
                    },
                    startTijd = TimeSpan.Parse("08:00:00.0000000"),
                    eindTijd = TimeSpan.Parse("17:00:00.0000000"),
                    timeZone = new misc.TimeZone{
                    name = "W. Europe Standard Time"
                }
                }
            });
        }

        if(postItem.Schedules.Contains("jeremy.michiels@clmbs.nl")){
            ret.value.Add(new GetSchedule.RetItem(){
                scheduleId= "jeremy.michiels@clmbs.nl",
            availabilityView = "00000000000000000000000000000000",
            scheduleItems = new List<GetSchedule.ScheduleItem>(),
            workingHours = new WorkingHours{
                daysOfWeek = new List<string>(){
                    "monday",
                    "tuesday",
                    "wednesday",
                    "thursday",
                    "friday"
                },
                startTijd = TimeSpan.Parse("08:00:00.0000000"),
                eindTijd = TimeSpan.Parse("17:00:00.0000000"),
                timeZone = new misc.TimeZone{
                    name = "W. Europe Standard Time"
                }
                
            }
            });
        }

        if(postItem.Schedules.Contains("erwin.oenema@silverant.nl")){
            ret.value.Add(new GetSchedule.RetItem{
            scheduleId = "erwin.oenema@silverant.nl",
            availabilityView = "22222222222222002222000000220000",
            scheduleItems = new List<GetSchedule.ScheduleItem>(){
                ReturnItem.SendItem("busy", "2024-11-19T08:30:00.0000000", "2024-11-19T09:00:00.0000000", "W. Europe Standard Time"),
                ReturnItem.SendItem("busy", "2024-11-19T08:30:00.0000000", "2024-11-19T09:00:00.0000000", "W. Europe Standard Time"),
                ReturnItem.SendItem("busy", "2024-11-19T09:00:00.0000000", "2024-11-19T11:00:00.0000000", "W. Europe Standard Time"),
                ReturnItem.SendItem("busy", "2024-11-19T11:00:00.0000000", "2024-11-19T12:30:00.0000000", "W. Europe Standard Time"),
                ReturnItem.SendItem("busy", "2024-11-19T13:00:00.0000000", "2024-11-19T14:00:00.0000000", "W. Europe Standard Time"),
                ReturnItem.SendItem("busy", "2024-11-19T15:30:00.0000000", "2024-11-19T16:00:00.0000000", "W. Europe Standard Time")

            },
            workingHours = new WorkingHours{
                daysOfWeek = new List<string>(){
                    "monday",
                    "tuesday",
                    "wednesday",
                    "thursday",
                    "friday"
                },
                startTijd= TimeSpan.Parse("08:00:00.0000000"),
                eindTijd= TimeSpan.Parse("17:00:00.0000000"),
                timeZone= new misc.TimeZone{
                    name= "W. Europe Standard Time"
                }
            }
        });
        }

        if(postItem.Schedules.Contains("jacco.schonewille@clmbs.nl")){
        ret.value.Add(new GetSchedule.RetItem{
            scheduleId= "jacco.schonewille@clmbs.nl",
            availabilityView= "22222222222200002222222222220000",
            scheduleItems= new List<GetSchedule.ScheduleItem>(){
                    ReturnItem.SendItem("busy", "2024-11-19T08:30:00.0000000", "2024-11-19T12:00:00.0000000", "W. Europe Standard Time"),
                    ReturnItem.SendItem("busy", "2024-11-19T13:00:00.0000000", "2024-11-19T16:00:00.0000000", "W. Europe Standard Time")
        },
        workingHours= new WorkingHours{
            daysOfWeek= new List<string>(){
                "monday",
                    "tuesday",
                    "wednesday",
                    "thursday",
                    "friday"
            },
            startTijd= TimeSpan.Parse("08:00:00.0000000"),
                eindTijd= TimeSpan.Parse("17:00:00.0000000"),
                timeZone= new misc.TimeZone{
                    name= "W. Europe Standard Time"
                }
        }
            });
        }
    return ret;
        }


        public static PostEvent.Return PostEvent(PostEvent.Post postItem){
            var ret = new PostEvent.Return(){
                odataContext = "https://graph.microsoft.com/v1.0/$metadata#users('64339082-ed84-4b0b-b4ab-004ae54f3747')/events/$entity",
                odataEtag = "W/\"NEXywgsVrkeNsFsyVyRrtAAASBUEsA==\"",
                id = "AAMkADAAABIGYDZAAA=",
                createdDateTime = DateTime.Now,
                lastModifiedDateTime = DateTime.Now,
                changeKey = "NEXywgsVrkeNsFsyVyRrtAAASBUEsA==",
                categories = new List<string>(),
                originalStartTimeZone = postItem.start.timeZone,
                originalEndTimeZone = postItem.end.timeZone,
                iCalUId = "040000008200E00074C5B7101A82E0080000000076B29D94B32CD6010000000000000000100000005F31C591C3C328459653D025BD277439",
                reminderMinutesBeforeStart = 15,
                isReminderOn = true,
                hasAttachments = false,
                subject = postItem.subject,
                bodyPreview = postItem.body.content,
                importance = "normal",
                sensitivity = "normal",
                isAllDay = false,
                isCancelled = false,
                isOrganizer = true,
                responseRequested = true,
                seriesMasterId = null,
                showAs = postItem.showAs,
                type = "singleInstance",
                webLink = "https://outlook.office365.com/owa/?itemid=AAMkADAABIGYDZAAA%3D&exvsurl=1&path=/calendar/item",
                onlineMeetingUrl = null,
                isOnlineMeeting = postItem.isOnlineMeeting,
                onlineMeetingProvider = postItem.isOnlineMeeting ? "teamsForBusiness" : null,
                allowNewTimeProposals = postItem.allowNewTimeProposals,
                recurrence = null,
                responseStatus = new PostEvent.ResStatus{
                    response = "organizer",
                    time = new DateTime(0),
                },
                body = new PostEvent.BodyType{
                    contentType = postItem.body.contentType,
                    content = postItem.body.content,
                },
                start = postItem.start,
                end = postItem.end,
                location = new PostEvent.LocationType{
                    displayName = postItem.location.displayName,
                    locationType = "default",
                    uniqueId = postItem.location.displayName,
                    uniqueIdType =  "private",
                },
                locations = new List<PostEvent.LocationType>{
                    new PostEvent.LocationType{
                        displayName = postItem.location.displayName,
                        locationType = "default",
                        uniqueId = postItem.location.displayName,
                        uniqueIdType =  "private",
                    }
                },
                attendees = new List<PostEvent.Attendee>(),
                organizer = new PostEvent.OrgMail{
                    emailAddress = postItem.attendees.First().emailAddress
                },
                onlineMeeting = new PostEvent.MeetingOnl(){
                    joinUrl = "https://teams.microsoft.com/l/meetup-join/19%3ameeting_ODkyNWFmNGYtZjBjYS00MDdlLTllOWQtN2E3MzJlNjM0ZWRj%40thread.v2/0?context=%7b%22Tid%22%3a%2298a79ebe-74bf-4e07-a017-7b410848cb32%22%2c%22Oid%22%3a%2264339082-ed84-4b0b-b4ab-004ae54f3747%22%7d",
                    conferenceId = "291633251",
                    tollNumber = "+1 323-555-0166"
                }




            };
            foreach(var item in postItem.attendees){
                ret.attendees.Add(new PostEvent.Attendee(){
                    emailAddress = item.emailAddress,
                    status = new PostEvent.ResStatus{
                        response = "none",
                        time = new DateTime(0)
                    },
                    type = "required"
                });
            }

            return ret;
        }







    }
}