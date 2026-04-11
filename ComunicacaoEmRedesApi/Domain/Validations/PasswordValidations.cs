namespace ComunicacaoEmRedesApi.Domain.Validations;

public static class PasswordValidations
{
    public static bool IsPasswordLengthCorrect(string password) 
        => password.Length is >= 8 and <= 15;
        
    public static bool IsPasswordStructureValid(string password)
    {
        bool hasLetter = false, hasNumber = false, hasSpecialChar = false;
        const string allowedSpecialChars = "._-@!?,:;()";
            
        foreach (var digit in password)
        {
            if (char.IsLetter(digit)) hasLetter = true;
            else if (char.IsNumber(digit)) hasNumber = true;
            else if (allowedSpecialChars.Contains(digit)) hasSpecialChar = true;
            else return false;
        }
            
        return hasLetter && hasNumber && hasSpecialChar;
    }
}