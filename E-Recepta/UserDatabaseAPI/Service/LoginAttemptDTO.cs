using System;

namespace UserDatabaseAPI.Service
{
    public class LoginAttemptDTO
    {
        public string Username { get; set; }
        public DateTime LoginTime { get; set; }
        public bool IsSuccessful { get; set; }
    }
}