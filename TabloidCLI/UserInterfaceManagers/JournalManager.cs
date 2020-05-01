﻿using System;
using System.Collections.Generic;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class JournalManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private JournalRepository _journalRepository;

        public JournalManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _journalRepository = new JournalRepository(connectionString);
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Journal Menu");
            Console.WriteLine(" 1) List Entries");
            Console.WriteLine(" 2) Add Entry");
            Console.WriteLine(" 3) Remove Entry");
            Console.WriteLine(" 0) Return to Main Menu");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Add();
                    return this;
                case "3":
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
            foreach (Journal entry in entries)
            {
                Console.WriteLine(entry);
            }
        }

        private void Add()
        {
            Console.WriteLine("New Journal Entry");

            Journal entry = new Journal();

            Console.Write("Title: ");
            entry.Title = Console.ReadLine();

            Console.WriteLine("Content: ");
            entry.Content = "";
            string content = Console.ReadLine();
            while (!string.IsNullOrWhiteSpace(content))
            {
                entry.Content += content + "\n";
                content = Console.ReadLine();
            }

            entry.CreateDateTime = DateTime.Now;

            _journalRepository.Insert(entry);
        }

        private void Remove()
        {
            Console.WriteLine("Which journal entry would you like to remove?");

            List<Journal> entries = _journalRepository.GetAll();

            for (int i = 0; i < entries.Count; i++)
            {
                Journal entry = entries[i];
                Console.WriteLine($" {i + 1}) {entry.Title}");
            }

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                Journal entryToDelete = entries[choice - 1];
                _journalRepository.Delete(entryToDelete.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection. Won't remove any journal entries.");
            }
        }
    }
}