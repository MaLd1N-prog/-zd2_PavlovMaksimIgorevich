using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; 
using System.Text;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Win32;

namespace zad2_PavlovMaksim
{
    public partial class MainWindow : Window
    {
        private PhoneBook phoneBook = new PhoneBook();
        private string fileName = "contacts.csv";

        public MainWindow()
        {
            InitializeComponent();
            LoadContacts();
        }

        // Загрузка контактов из файла
        private void LoadContacts()
        {
            PhoneBookLoader.Load(phoneBook, fileName);
            ContactsListBox.ItemsSource = null;
            ContactsListBox.Items.Clear();
            ContactsListBox.ItemsSource = phoneBook.GetAllContacts();
        }


        private void ShowAllContacts()
        {
            ContactsListBox.ItemsSource = null;
            ContactsListBox.Items.Clear();

            var allContacts = phoneBook.GetAllContacts()
                .OrderBy(c => c.Name); 

            foreach (var contact in allContacts)
            {
                ContactsListBox.Items.Add(contact);
            }
        }

        // Вывод всех контактов 
        private void Menu_Click_AddAllCon(object sender, RoutedEventArgs e)
        {
            ShowAllContacts();
        }

        // Поиск по имени 
        private void Menu_Click_Name(object sender, RoutedEventArgs e)
        {
            string search = SearchTextBox.Text.Trim();

            if (string.IsNullOrEmpty(search))
            {
                ShowAllContacts();
                return;
            }

            var results = phoneBook.SearchByName(search)
                .OrderBy(c => c.Name)  
                .ToList();             

            ContactsListBox.Items.Clear();
            foreach (var contact in results)
            {
                ContactsListBox.Items.Add(contact);
            }

            if (!results.Any())  
            {
                MessageBox.Show("Ничего не найдено");
            }
            else
            {
                MessageBox.Show($"Найдено: {results.Count}");
            }
        }

        // Добавление контакта
        private void Menu_Click_AddCon(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();

         
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите имя");
                return;
            }

           
            if (name.Any(char.IsDigit))  
            {
                MessageBox.Show("Имя не должно содержать цифры");
                return;
            }

         
            if (phone.Any(char.IsLetter))  
            {
                MessageBox.Show("Телефон не должен содержать буквы");
                return;
            }

            phoneBook.AddContact(name, phone);

            NameTextBox.Text = "";
            PhoneTextBox.Text = "";
            ShowAllContacts();
        }

        // Удаление контакта 
        private void Menu_Click_DeleteCon(object sender, RoutedEventArgs e)
        {
            if (ContactsListBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите контакт для удаления");
                return;
            }

            var selectedContact = ContactsListBox.SelectedItem as Contact;
            if (selectedContact != null)
            {
                
                var contactToRemove = phoneBook.GetAllContacts()
                    .FirstOrDefault(c => c.Name.Equals(selectedContact.Name, StringComparison.OrdinalIgnoreCase));

                if (contactToRemove != null)
                {
                    phoneBook.RemoveContact(contactToRemove);
                    ShowAllContacts();
                }
            }
        }

        // Сохранение в файл
        private void Menu_Click_Save(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "CSV files (*.csv)|*.csv";
                dialog.FileName = "contacts.csv";

                if (dialog.ShowDialog() == true)
                {
                    PhoneBookLoader.Save(phoneBook, dialog.FileName);
                    MessageBox.Show($"Сохранено в: {dialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        // Выход 
        private void Menu_Click_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}