using FIAP.FCG.Domain.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace FIAP.FCG.Domain.Entities
{
    public class UserProfile : BaseEntity
    {
        public string? Name { get; private set; }
        public string? Email { get; private set; }
        public string? Cpf { get; private set; }
        public DateTime Birthday { get; private set; }
        public string? Password { get; private set; }
        public string? ConfirmPassword { get; private set; }
        public string? ImageURL { get; private set; }

        protected UserProfile() { }

        public UserProfile(string name, string email, string cpf, DateTime birthday, string password, string confirmPassword)
        {
            Name = name;
            Email = email;
            Cpf = cpf;
            Birthday = birthday;
            Password = password;
            ConfirmPassword = confirmPassword;
            DateCreated = DateTime.Now;
        }

        public override bool IsValid()
        {
            ValidationResult = new UserValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public void ValidationClass()
        {
            if (this.Name.Length == 0)            
                throw new Exception("");            
        }

        public void ChangeName(string name)
        {
            Name = name;
            ValidationClass();
        }

        public void UpdateUser(string name, string email, string cpf, DateTime birthday)
        {
            Name = name;
            Email = email;
            Cpf = cpf;
            Birthday = birthday;
            DateUpdated = DateTime.Now;
        }

        public void UpdatePassword(string password, string confirmPassword)
        {
            Password = password;
            ConfirmPassword = confirmPassword;
            DateUpdated = DateTime.Now;
        }

        public bool ForgotPassword(string email, DateTime birthday, string cpf)
        {
            if (this.Email != email && this.Birthday != birthday && this.Cpf != cpf)            
                return false;
            
            DateUpdated = DateTime.Now;

            return true;
        }


        public bool Login(string email, string password)
        {
            if (this.Email != email || this.Password != password)            
                return false;
            
            DateUpdated = DateTime.Now;

            return true;
        }

        public void UpdateImage(string imageURL)
        {
            ImageURL = imageURL;
        }
    }
}
