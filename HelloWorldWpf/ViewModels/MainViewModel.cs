using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using HelloWorldWpf.Commands;
using HelloWorldWpf.Models;

namespace HelloWorldWpf.ViewModels
{
    public sealed class MainViewModel : ObservableObject
    {
        private readonly ObservableCollection<CallRecord> allCalls;
        private string searchText;
        private CallRecord selectedCall;
        private string newCallerName;
        private string newPhoneNumber;
        private string newTopic;
        private int nextId = 1000;

        public MainViewModel()
        {
            allCalls = new ObservableCollection<CallRecord>
            {
                new CallRecord
                {
                    Id = 1001,
                    CallerName = "Jana Nováková",
                    PhoneNumber = "+421 905 111 222",
                    Topic = "Objednávka fakturácie",
                    Note = "Poslať potvrdenie e-mailom.",
                    CreatedAt = DateTime.Now.AddMinutes(-52),
                    IsCompleted = false
                },
                new CallRecord
                {
                    Id = 1002,
                    CallerName = "Peter Kováč",
                    PhoneNumber = "+421 903 333 444",
                    Topic = "Technická podpora",
                    Note = "Reset hesla dokončený.",
                    CreatedAt = DateTime.Now.AddHours(-3),
                    IsCompleted = true,
                    CompletedAt = DateTime.Now.AddHours(-2).AddMinutes(-35)
                }
            };

            nextId = 1003;

            CallsView = CollectionViewSource.GetDefaultView(allCalls);
            CallsView.Filter = FilterCall;

            AddCallCommand = new RelayCommand(AddCall, CanAddCall);
            MarkCompletedCommand = new RelayCommand(MarkSelectedCallCompleted, CanMarkSelectedCallCompleted);

            NewCallerName = string.Empty;
            NewPhoneNumber = string.Empty;
            NewTopic = string.Empty;
        }

        public ICollectionView CallsView { get; }

        public ICommand AddCallCommand { get; }

        public ICommand MarkCompletedCommand { get; }

        public string SearchText
        {
            get => searchText;
            set
            {
                if (SetProperty(ref searchText, value))
                {
                    CallsView.Refresh();
                }
            }
        }

        public CallRecord SelectedCall
        {
            get => selectedCall;
            set
            {
                if (SetProperty(ref selectedCall, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string NewCallerName
        {
            get => newCallerName;
            set
            {
                if (SetProperty(ref newCallerName, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string NewPhoneNumber
        {
            get => newPhoneNumber;
            set
            {
                if (SetProperty(ref newPhoneNumber, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string NewTopic
        {
            get => newTopic;
            set
            {
                if (SetProperty(ref newTopic, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private bool FilterCall(object obj)
        {
            if (!(obj is CallRecord call))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                return true;
            }

            var q = SearchText.Trim();
            return Contains(call.CallerName, q)
                || Contains(call.PhoneNumber, q)
                || Contains(call.Topic, q)
                || Contains(call.Note, q)
                || call.Id.ToString().Contains(q);
        }

        private static bool Contains(string source, string query)
        {
            return !string.IsNullOrEmpty(source)
                   && source.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool CanAddCall()
        {
            return !string.IsNullOrWhiteSpace(NewCallerName)
                && !string.IsNullOrWhiteSpace(NewPhoneNumber)
                && !string.IsNullOrWhiteSpace(NewTopic);
        }

        private void AddCall()
        {
            var call = new CallRecord
            {
                Id = nextId++,
                CallerName = NewCallerName.Trim(),
                PhoneNumber = NewPhoneNumber.Trim(),
                Topic = NewTopic.Trim(),
                CreatedAt = DateTime.Now,
                Note = string.Empty,
                IsCompleted = false,
                CompletedAt = null
            };

            allCalls.Insert(0, call);
            SelectedCall = call;

            NewCallerName = string.Empty;
            NewPhoneNumber = string.Empty;
            NewTopic = string.Empty;

            CallsView.Refresh();
        }

        private bool CanMarkSelectedCallCompleted()
        {
            return SelectedCall != null && !SelectedCall.IsCompleted;
        }

        private void MarkSelectedCallCompleted()
        {
            if (SelectedCall == null || SelectedCall.IsCompleted)
            {
                return;
            }

            SelectedCall.IsCompleted = true;
            SelectedCall.CompletedAt = DateTime.Now;
            CallsView.Refresh();
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
