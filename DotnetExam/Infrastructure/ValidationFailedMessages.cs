namespace DotnetExam.Infrastructure;

public static class ValidationFailedMessages
{
    public static string EmptyField => "Field is empty";
    public static string TooShortField => "Field is too short";
    public static string TooLongField => "Field is too long";
    public static string WrongSymbols => "Field contains wrong symbols";
    public static string AlreadyUsed => "Already used";
    public static string RequiresLowercase => "Field requires lowercase";
    public static string RequiresUppercase => "Field requires uppercase";
    public static string RequiresDigit => "Field requires digit";
    public static string RequiresNonAlphanumeric => "Field requires non alphanumeric";
}