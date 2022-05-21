using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ValidationException = CrimeSyndicate.Util.Exception.ValidationException;

namespace CrimeSyndicate.Models;

public class User
{
    private class PasswordMismatchException : ValidationException
    {
        public override string Message => "The passwords entered do not match.";
    }
    
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    [ValidateNever] public string PasswordHash { get; set; }

    [NotMapped] [ValidateNever] public string PasswordEntry { get; set; }
    [NotMapped] [ValidateNever] public string PasswordValidation { get; set; }

    public async Task GeneratePassword()
    {
        await Task.Run(() =>
        {
            Console.WriteLine($"Password entry: {PasswordEntry}");
            Console.WriteLine($"Password validation: {PasswordValidation}");
            CheckPasswordValidation(PasswordEntry, PasswordValidation);

            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(PasswordEntry);
        });
    }

    public async Task<bool> VerifyPassword(string password)
    {
        return await Task.Run(() => BCrypt.Net.BCrypt.EnhancedVerify(password, PasswordHash));
    }
    
    private static void CheckPasswordValidation(string password, string validation)
    {
        if (password != validation)
        {
            throw new PasswordMismatchException();
        }
    }
}
