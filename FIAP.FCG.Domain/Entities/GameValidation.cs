using FluentValidation;

namespace FIAP.FCG.Domain.Entities
{
    public class GameValidation : AbstractValidator<Game>
    {
        public GameValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Nome tem que ser preenchido")
                .Length(0, 100).WithMessage("Tamanho do campo nome excedido");

            RuleFor(c => c.Category)
                    .NotEmpty().WithMessage("Categoria tem que ser preenchido");

            RuleFor(c => c.Censorship)
                    .NotEmpty().WithMessage("Censura tem que ser preenchido");

            RuleFor(c => c.Price)
                    .NotEmpty().WithMessage("Preço tem que ser preenchido");

            RuleFor(c => c.DateRelease)
                    .NotEmpty().WithMessage("Data tem que ser preenchido");
        }
    }
}
