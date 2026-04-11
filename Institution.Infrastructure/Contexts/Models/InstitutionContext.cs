using Institution.Application.Interfaces;

namespace Institution.Infrastructure.Contexts.Models
{
    public class InstitutionContext : IInstitutionContext
    {
        public string? Alias { get; set; }
    }
}