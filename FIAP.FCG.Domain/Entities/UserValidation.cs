using FluentValidation;

namespace FIAP.FCG.Domain.Entities
{
    public class UserValidation : AbstractValidator<UserProfile>
    {
        public UserValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Nome tem que ser preenchido")
                .Length(0, 100).WithMessage("Tamanho do campo nome excedido");

            //RuleFor(c => c.LastName)
            //    .NotEmpty().WithMessage("Sobrenome tem que ser preenchido")
            //    .Length(0, 100).WithMessage("Tamanho do campo sobrenome excedido");

            RuleFor(c => c.Email)
                    .NotEmpty().WithMessage("Email tem que ser preenchido")
                    .Length(0, 80).WithMessage("Tamanho do campo email excedido");


            RuleFor(c => c.Password)
                    .NotEmpty().WithMessage("Senha tem que ser preenchido")
                    .Length(10, 150).WithMessage("Tamanho do campo senha excedido");

            RuleFor(c => c.ConfirmPassword)
                    .NotEmpty().WithMessage("Senha tem que ser preenchido")
                    .Length(10, 150).WithMessage("Tamanho do campo senha excedido");
        }
    }
}
