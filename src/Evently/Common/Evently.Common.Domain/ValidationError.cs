namespace Evently.Common.Domain;

public sealed record ValidationError : Error
{
    public Error[] Errors { get; }
    
    public ValidationError(Error[] errors) 
        : base("General.Validation", "One or more validation errors occured", ErrorType.Validation)
    {
        Errors = errors;
    }
    
    public static ValidationError FromResults(IEnumerable<Result> results) =>
        new(results
            .Where(result => result.IsFailure)
            .Select(result => result.Error)
            .ToArray());
}