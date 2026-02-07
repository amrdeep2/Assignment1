namespace Assignment1.Models
{
    public class Attendee
    {
        public int id { get; set; }
       public String name;
        public String email;


        public String getName()
        {
            return name;
        }
        public String getEmail()
        {
            return email;
        }

        public void setName(String name)
        {
            this.name = name;
        }
        public void setEmail(String email)
        {
            this.email = email;
        }

    }
}