using System.IO;
using FluentValidation;

namespace FSyncCli.Validation
{
    public class SourceDirInfoValidator : AbstractValidator<DirectoryInfo>
    {
        public SourceDirInfoValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.Exists).Must(x => x == true);
        }
    }
}