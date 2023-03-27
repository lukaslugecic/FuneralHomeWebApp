using BaseLibrary;

namespace FuneralHome.Commons;
public static class Validation
{
    public static Result Validate(params (Func<bool> validationFunc, string errorMessage)[] validations)
    {
        foreach (var validation in validations)
        {
            if (!validation.validationFunc())
            {
                return Results.OnFailure(validation.errorMessage);
            }
        }
        return Results.OnSuccess("Model is valid");
    }
}