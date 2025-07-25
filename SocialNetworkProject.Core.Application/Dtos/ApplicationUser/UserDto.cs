﻿namespace SocialNetworkProject.Core.Application.Dtos.ApplicationUser
{
    public class UserDto
    {
        public string Id { get; set; } 
        public string UserName { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
