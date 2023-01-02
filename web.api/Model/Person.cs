using System.ComponentModel.DataAnnotations;

namespace web.api.Model
{
    public record Person([Required]string Name, string FirstName, string LastName);
    
}
