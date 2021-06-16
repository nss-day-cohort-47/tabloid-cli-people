using System;
using System.Collections.Generic;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class JournalManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private JournalRepository _journalRepository;
        private string _connectionString;

        public JournalManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _journalRepository = new JournalRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Journal Menu");
            Console.WriteLine(" 1) List Journal Entries");
            Console.WriteLine(" 2) Entry Details");
            Console.WriteLine(" 3) Add Entry");
            Console.WriteLine(" 4) Edit Entry");
            Console.WriteLine(" 5) Remove Entry");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Journal journal = Choose();
                    if (journal == null)
                    {
                        return this;
                    }
                    else
                    {
                        //Commented out for now to allow the code to run
                        return this; //new JournalDetailManager(this, _connectionString, journal.Id);
                    }
                case "3":
                    Add();
                    return this;
                case "4":
                    Edit();
                    return this;
                case "5":
                    Remove();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void List()
        {
            List<Journal> entries = _journalRepository.GetAll();
            foreach (Journal e in entries)
            {
                Console.WriteLine(@$"{e.Title}, {e.Content}, {e.CreateDateTime}");
            }
        }

        private Journal Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose an Entry:";
            }

            Console.WriteLine(prompt);

            List<Journal> entries = _journalRepository.GetAll();

            for (int i = 0; i < entries.Count; i++)
            {
                Journal entry = entries[i];
                Console.WriteLine($" {i + 1}) {entry.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return entries[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        private void Add()
        {
            Console.WriteLine("New Entry");
            Journal entry = new Journal();

            Console.Write("Title: ");
            entry.Title = Console.ReadLine();

            Console.Write("Content: ");
            entry.Content = Console.ReadLine();

            entry.CreateDateTime = DateTime.Now;
            Console.WriteLine($"{entry.CreateDateTime}");

            _journalRepository.Insert(entry);
        }

        private void Edit()
        {
            Journal entryToEdit = Choose("Which entry would you like to edit?");
            if (entryToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New title (blank to leave unchanged): ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                entryToEdit.Title = title;
            }
            Console.Write("Edit content (blank to leave unchanged): ");
            string content = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(content))
            {
                entryToEdit.Content = content;
            }

            _journalRepository.Update(entryToEdit);
        }

        private void Remove()
        {
            Journal entryToDelete = Choose("Which entry would you like to remove?");
            if (entryToDelete != null)
            {
                _journalRepository.Delete(entryToDelete.Id);
            }
        }
    }
}
