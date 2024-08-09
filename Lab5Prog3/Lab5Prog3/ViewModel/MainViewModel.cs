using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Lab5Prog3.Model;

namespace Lab5Prog3.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _search;
        public string Search
        {
            get => _search;
            set => SetProperty(ref _search, value);
        }

        private ObservableCollection<User> _user;
        public ObservableCollection<User> Users
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private ObservableCollection<User> _usersFilters;
        public ObservableCollection<User> UsersFilters
        {
            get => _usersFilters;
            set => SetProperty(ref _usersFilters, value);
        }

        private Stack<string> _errorMessages;
        public Stack<string> ErrorMessages 
        { 
            get => _errorMessages; 
            set => SetProperty(ref _errorMessages, value);
        }

        public ICommand AddCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand SortAscCommand { get; }
        public ICommand SortDescCommand { get; }
        public ICommand ShowErrorsCommand { get; }

        public MainViewModel()
        {
            Users = new ObservableCollection<User>();
            UsersFilters = new ObservableCollection<User>();
            ErrorMessages = new Stack<string>();

            AddCommand = new RelayCommand(AddUser);
            SearchCommand = new RelayCommand(SearchUser);
            SortAscCommand = new RelayCommand(SortUsersAsc);
            SortDescCommand = new RelayCommand(SortUsersDesc);
            ShowErrorsCommand = new RelayCommand(ShowErrors);

            UpdateSum();
        }

        private void AddUser()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(Email))
                    throw new ArgumentException("Todos os campos são obrigatórios!");

                var user = new User { FirstName = FirstName, LastName = LastName, Email = Email };
                Users.Add(user);
                UsersFilters.Add(user);

                UpdateSum();
                CleanFields();
            }
            catch (Exception ex)
            {
                ErrorMessages.Push($"{DateTime.Now}: {ex.Message}");
            }
        }

        private void SearchUser()
        {
            try
            {
                var query = Search?.Trim().ToLower();
                UsersFilters = new ObservableCollection<User>(
                    Users.Where(u => u.Email.ToLower().Contains(query))
                );
            }
            catch (Exception ex)
            {
                ErrorMessages.Push($"{DateTime.Now}: {ex.Message}");
            }
        }

        private void SortUsersAsc()
        {
            try
            {
                UsersFilters = new ObservableCollection<User>(
                    Users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName)
                );
            }
            catch (Exception ex)
            {
                ErrorMessages.Push($"{DateTime.Now}: {ex.Message}");
            }
        }

        private void SortUsersDesc()
        {
            try
            {
                UsersFilters = new ObservableCollection<User>(
                    Users.OrderByDescending(u => u.FirstName).ThenByDescending(u => u.LastName)
                );
            }
            catch (Exception ex)
            {
                ErrorMessages.Push($"{DateTime.Now}: {ex.Message}");
            }
        }

        private void ShowErrors()
        {
            if (ErrorMessages.Any())
            {
                MessageBox.Show(string.Join(Environment.NewLine, ErrorMessages), "Erros Ocorridos");
            }
            else
            {
                MessageBox.Show("Nenhum erro registrado.", "Erros Ocorridos");
            }
        }

        private void UpdateSum()
        {
            Sum = $"Total users: {Users.Count}";
        }

        private string _sum;
        public string Sum
        {
            get => _sum;
            set => SetProperty(ref _sum, value);
        }

        private void CleanFields()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
        }
    }
}
