﻿namespace Watch.WebApi.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public string UserId { get; }
        public UserNotFoundException(string userId) : base($"User {userId} not found")
        {
            UserId = userId;
        }
    }
}
