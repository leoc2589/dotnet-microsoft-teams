using App.Models;

namespace App.Interfaces;

public interface IMSGraphService
{
    Task<(bool Success, string Token, string Message)> GetTokenAsync();
    Task<(bool Success, Event Result, string Message)> CreateEventAsync(string token, Event item);
}