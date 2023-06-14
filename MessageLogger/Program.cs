// See https://aka.ms/new-console-template for more information
using MessageLogger.Data;
using MessageLogger.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

Console.WriteLine("Welcome to Message Logger!");
Console.WriteLine();
Console.WriteLine("Let's create a user pofile for you.");
Console.Write("What is your name? ");
string name = Console.ReadLine();
Console.Write("What is your username? (one word, no spaces!) ");
string username = Console.ReadLine();
User user = new User(name, username);
using (var context = new MessageLoggerContext())
{
    context.Users.Add(user);
    context.SaveChanges();
}

Console.WriteLine();
Console.WriteLine("To log out of your user profile, enter `log out`.");

Console.WriteLine();
Console.Write("Add a message (or `quit` to exit): ");

string userInput = Console.ReadLine();
//Save message to Db
using (var context = new MessageLoggerContext())
{
    User databaseUser = context.Users.Find(user.Id);
    Message userMessage = new Message(userInput);
    userMessage.User = databaseUser;
    context.Messages.Add(userMessage);
    context.SaveChanges();
}
List<User> users = new List<User>() { user };

while (userInput.ToLower() != "quit")
{
    while (userInput.ToLower() != "log out") //does not respond to 'quit'
    {
        user.Messages.Add(new Message(userInput)); //if 'log out', do not add

        foreach (var message in user.Messages)
        {
            //string timeZoneId = "Mountain Standard Time";
            //ConvertTZ(message, timeZoneId);
            Console.WriteLine($"{user.Name} {message.CreatedAt:t}: {message.Content}");
        }

        Console.Write("Add a message: ");

        userInput = Console.ReadLine();
        //Save message to Db
        Message newMessage = new Message(userInput);
        using (var context = new MessageLoggerContext())
        {
            User databaseUser = context.Users.Find(user.Id);
            Message userMessage = new Message(userInput);
            userMessage.User = databaseUser;
            context.Messages.Add(userMessage);
            context.SaveChanges();
        }
        //Read previous messages from Db
        using (var context = new MessageLoggerContext())
        {
            var previousMessages = context.Users
                 .Include(user => user.Messages)
                 .Where(user => user.Username == username).First();
            foreach (var message in previousMessages.Messages) //one user's messages
            {
                Console.WriteLine($"{user.Name} {message.CreatedAt:t}: {message.Content}");
            }
        }
        Console.WriteLine();
    }

    Console.Write("Would you like to log in a `new` or `existing` user? Or, `quit`? ");
    userInput = Console.ReadLine();
    if (userInput.ToLower() == "new")
    {
        Console.Write("What is your name? ");
        name = Console.ReadLine();
        Console.Write("What is your username? (one word, no spaces!) ");
        username = Console.ReadLine();
        user = new User(name, username);
        //Save user to Db
        using (var context = new MessageLoggerContext())
        {
            context.Users.Add(user);
            context.SaveChanges();
        }
        users.Add(user);
        Console.Write("Add a message: ");

        userInput = Console.ReadLine();
        //Save message to Db
        Message newMessage = new Message(userInput);
        using (var context = new MessageLoggerContext())
        {
            User databaseUser = context.Users.Find(user.Id);
            Message userMessage = new Message(userInput);
            userMessage.User = databaseUser;
            context.Messages.Add(userMessage);
            context.SaveChanges();
        }
        //Read previous messages from Db
        using (var context = new MessageLoggerContext())
        {
            var previousMessages = context.Users
                 .Include(user => user.Messages)
                 .Where(user => user.Username == username).First();
            foreach (var message in previousMessages.Messages) //one user's messages
            {
                Console.WriteLine($"{user.Name} {message.CreatedAt:t}: {message.Content}");
            }
        }

    }
    else if (userInput.ToLower() == "existing")
    {
        Console.Write("What is your username? ");
        username = Console.ReadLine();
        user = null;
        //Read Db for existing user
        using (var context = new MessageLoggerContext())
        {
            foreach (var existingUser in context.Users)
            {
                if (existingUser.Username == username)
                {
                    user = existingUser;
                }
            }
        }

        if (user != null)
        {
            Console.Write("Add a message: ");
            userInput = Console.ReadLine();
            //Save message to Db
            Message newMessage = new Message(userInput);
            using (var context = new MessageLoggerContext())
            {
                User databaseUser = context.Users.Find(user.Id);
                Message userMessage = new Message(userInput);
                userMessage.User = databaseUser;
                context.Messages.Add(userMessage);
                context.SaveChanges();
            }
            //Read previous messages from Db
            using (var context = new MessageLoggerContext())
            {
                var previousMessages = context.Users
                     .Include(user => user.Messages)
                     .Where(user => user.Username == username).First();
                foreach (var message in previousMessages.Messages) //one user's messages
                {
                    Console.WriteLine($"{user.Name} {message.CreatedAt:t}: {message.Content}");
                }
            }
        }
        else
        {
            Console.WriteLine("could not find user");
            userInput = "quit";

        }
    }

}

Console.WriteLine("Thanks for using Message Logger!");
foreach (var u in users)
{
    Console.WriteLine($"{u.Name} wrote {u.Messages.Count} messages.");
}
