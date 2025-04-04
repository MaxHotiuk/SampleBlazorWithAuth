using System;
using Microsoft.AspNetCore.Components;

namespace Sample.Client.Services
{
    public class ProfilePictureService
    {
        // Event that components can subscribe to
        public event Action? OnProfilePictureChanged;
        
        private string? _currentProfilePictureUrl;
        
        public string? CurrentProfilePictureUrl 
        { 
            get => _currentProfilePictureUrl;
            set
            {
                _currentProfilePictureUrl = value;
                NotifyProfilePictureChanged();
            }
        }
        
        // Method to notify subscribers that the profile picture has changed
        public void NotifyProfilePictureChanged()
        {
            OnProfilePictureChanged?.Invoke();
        }
        
        // Method to clear the current profile picture
        public void ClearProfilePicture()
        {
            _currentProfilePictureUrl = null;
            NotifyProfilePictureChanged();
        }
    }
}