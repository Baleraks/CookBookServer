﻿namespace CookBookBase.Models
{
    public class TokenResponse
    {
        public string? jwttoken { get; set; }
        public string? refreshtoken { get; set; }
        public int? Id { get; set; }
    }
}
