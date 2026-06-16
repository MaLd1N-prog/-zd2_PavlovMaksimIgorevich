using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zad2_PavlovMaksim
{
    class PhoneBookLoader
    {
  
        public static void Save(PhoneBook phoneBook, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (var contact in phoneBook.GetAllContacts())
            {
                lines.Add($"{contact.Name},{contact.Phone}");
            }

            System.IO.File.WriteAllLines(fileName, lines);
        }
    }
}
