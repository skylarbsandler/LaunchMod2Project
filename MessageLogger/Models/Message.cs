using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public User User { get; set; }

        public Message(string content)
        {
            Content = content;
            CreatedAt = DateTime.Now.ToUniversalTime();
        }
    }
        

    ////local time method
    //public void ConvertTZ()
    //    {
    //        message.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(message.CreatedAt, timeZone);
    //    }
    ////return datetime, convert to local time
    //}
}
