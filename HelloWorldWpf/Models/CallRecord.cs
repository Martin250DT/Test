using System;
using HelloWorldWpf.ViewModels;

namespace HelloWorldWpf.Models
{
    public sealed class CallRecord : ObservableObject
    {
        private string note;
        private bool isCompleted;
        private DateTime? completedAt;

        public int Id { get; set; }
        public string CallerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Topic { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Note
        {
            get => note;
            set => SetProperty(ref note, value);
        }

        public bool IsCompleted
        {
            get => isCompleted;
            set => SetProperty(ref isCompleted, value);
        }

        public DateTime? CompletedAt
        {
            get => completedAt;
            set => SetProperty(ref completedAt, value);
        }
    }
}
