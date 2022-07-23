using EventBus.Events;

namespace UserService.Events
{
    public record UserCreatedEvent : IntegrationEvent
    {
        public string Name { get; set; }
        public string Mail { get; set; }
        public string OtherData { get; set; }

        public UserCreatedEvent(string name, string mail, string otherData)
        {
            this.OtherData = otherData;
            this.Name = name;
            this.Mail = mail;
        }
    }
}
