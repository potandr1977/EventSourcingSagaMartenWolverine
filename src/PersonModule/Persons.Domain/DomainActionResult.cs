namespace Persons.Domain;

public enum DomainActionResultTypeEnum
{ 
    Failed = 0,
    Success = 1,
}
public record DomainActionResult(DomainActionResultTypeEnum DomainActionResultType, string? Description = default);
