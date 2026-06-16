using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace zad2_PavlovMaksim
{
    class PhoneBook
    {

        private List<Contact> contacts = new List<Contact>();

        public List<Contact> GetAllContacts()
        {
            return contacts;
        }
        public void ListBookClear()
        {
            contacts.Clear();
        }
        public List<Contact> SearchByName(string name)
        {
            List<Contact> results = new List<Contact>();
            string searchLower = name.ToLower();

            foreach (var contact in contacts)
            {
                if (contact.Name.ToLower().Contains(searchLower))
                {
                    results.Add(contact);
                }
            }
            return results;
        }

        public void AddContact(Contact contact)
        {
            if (contact != null && !string.IsNullOrWhiteSpace(contact.Name))
            {
                contacts.Add(contact);
            }
        }

        public void AddContact(string name, string phone)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                contacts.Add(new Contact { Name = name, Phone = phone });
            }
        }

        public bool RemoveContact(Contact contact)
        {
            return contacts.Remove(contact);
        }

        public bool RemoveContact(string name)
        {
            var contact = contacts.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (contact != null)
            {
                return contacts.Remove(contact);
            }
            return false;
        }

        public int Count
        {
            get { return contacts.Count; }
        }
    }
}