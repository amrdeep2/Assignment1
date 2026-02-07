namespace Assignment1.Models
{
    public class Event
    {
        public int id;
        public string title;
        public string Location;
        public List<Attendee> Attendees;
        public DateTime Date;

        // ✅ Add this constructor
        public Event()
        {
            Attendees = new List<Attendee>();
        }

        public int getID() => id;
        public string getTitle() => title;

        public void setLocation(string name) => Location = name;
        public void setTitle(string email) => title = email;
        public void setId(int id) => this.id = id;

        public void addList(Attendee list)
        {
            Attendees.Add(list);
        }
    }
}
