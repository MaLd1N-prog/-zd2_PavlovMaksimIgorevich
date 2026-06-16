using System;
using System.Collections.Generic;
using System.IO;
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
            try
            {
                if (File.Exists(fileName))
                {
                    string[] lines = File.ReadAllLines(fileName);

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        string[] parts = line.Split(';');

                        if (parts.Length >= 2)
                        {
                            string name = parts[0].Trim();
                            string phone = parts[1].Trim();

                            if (!string.IsNullOrWhiteSpace(name))
                            {
                                phoneBook.AddContact(name, phone);
                            }
                        }
                    }

                    ShowAllContacts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        // Показать все контакты
        private void ShowAllContacts()
        {
            ContactsListBox.Items.Clear();

            var allContacts = phoneBook.GetAllContacts();

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

            var results = phoneBook.SearchByName(search);

            ContactsListBox.Items.Clear();
            foreach (var contact in results)
            {
                ContactsListBox.Items.Add(contact);
            }

            if (results.Count == 0)
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

           
            if (name == "")
            {
                MessageBox.Show("Введите имя");
                return;
            }

            
            foreach (char c in name)
            {
                if (char.IsDigit(c))
                {
                    MessageBox.Show("Имя не должно содержать цифры");
                    return;
                }
            }

           
            foreach (char c in phone)
            {
                if (char.IsLetter(c))
                {
                    MessageBox.Show("Телефон не должен содержать буквы");
                    return;
                }
            }


            phoneBook.AddContact(name, phone);

        
            NameTextBox.Text = "";
            PhoneTextBox.Text = "";
            ShowAllContacts();
            MessageBox.Show($"Контакт '{name}' добавлен");
        }

        // Удаление контакта
        private void Menu_Click_DeleteCon(object sender, RoutedEventArgs e)
        {
            if (ContactsListBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите контакт для удаления");
                return;
            }
                ShowAllContacts();
            
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