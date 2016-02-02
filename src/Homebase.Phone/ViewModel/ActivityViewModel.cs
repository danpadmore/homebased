using Homebase.Business.Model;
using System;
using System.Collections.Generic;
using static System.Globalization.CultureInfo;

namespace Homebase.Phone.ViewModel
{
    public class ActivityViewModel
    {
        public DateTimeOffset Timestamp { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public List<string> Actions { get; set; }
        public bool IsHome { get { return Type == ActionTrigger.Home.ToString(); } }
        public bool IsAway { get { return Type == ActionTrigger.Away.ToString(); } }
        public bool IsDefault { get { return !IsHome && !IsAway; } }

        public string Title
        {
            get
            {
                var date = IsThisWeek()
                    ? $"{Timestamp.LocalDateTime.DayOfWeek.ToString()}"
                    : $"{Timestamp.LocalDateTime.Date.ToString(CurrentCulture.DateTimeFormat.ShortDatePattern)}";

               
                return $"{date} {Timestamp.LocalDateTime.ToString(CurrentCulture.DateTimeFormat.ShortTimePattern)}";
            }
        }

        public DateTime FirstDayOfCurrentWeek { get; private set; }

        public ActivityViewModel(DateTimeOffset timestamp, string type)
        {
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentNullException(nameof(type));

            Timestamp = timestamp;
            Type = type;

            Actions = new List<string>();

            FirstDayOfCurrentWeek = DateTimeOffset.UtcNow.Date
                .AddDays((int)CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)DateTimeOffset.UtcNow.DayOfWeek);
        }

        private bool IsThisWeek()
        {
            var daysSinceFirstDayOfCurrentWeek = (FirstDayOfCurrentWeek - Timestamp.Date);

            return (daysSinceFirstDayOfCurrentWeek.TotalDays < 7);
        }
    }
}
