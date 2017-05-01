using System;

namespace LicenseService.Models
{
    public class TimeStamped<T> 
    {
        public DateTime DateTime { get; set; }
        public T Model { get; set; }
    }
}