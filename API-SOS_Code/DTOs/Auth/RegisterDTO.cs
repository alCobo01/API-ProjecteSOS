﻿namespace API_SOS_Code.DTOs.Auth
{
    public class RegisterDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int SecretKey { get; set; }
    }
}
