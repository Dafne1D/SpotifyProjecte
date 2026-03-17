using System;

namespace AppSpotifyWPF.Classes
{
    public class PermissionResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}