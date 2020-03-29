using BLL.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Request
{
   public class StudentRequest
    {
        public string Name { get; set; }
        public string Roll { get; set; }
        public string Email { get; set; }
    }

    public class StudentRequestValidator : AbstractValidator<StudentRequest>
    {
        private readonly IServiceProvider _serviceProvider;
        public StudentRequestValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RuleFor(x=>x.Roll).NotEmpty().NotNull().MustAsync(RollExits).WithMessage("Roll Aleady exits.");
            RuleFor(x => x.Name).NotEmpty().NotNull().MustAsync(NameExits).WithMessage("Name Aleady exits.");
        }

        private async Task<bool> NameExits(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            var studentService = _serviceProvider.GetRequiredService<IStudentService>();
            return await studentService.IsNameExit(name);

        }

        private async Task<bool> RollExits(string roll, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(roll))
            {
                return false;
            }
            var studentService = _serviceProvider.GetRequiredService<IStudentService>();
            return await studentService.IsRollExit(roll);
        }
    }
}
