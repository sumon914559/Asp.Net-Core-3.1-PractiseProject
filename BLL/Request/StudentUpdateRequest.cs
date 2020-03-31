using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Request
{
  public class StudentUpdateRequest
    {
        public string Name { get; set; }
        public string Roll { get; set; }
        public string Email { get; set; }
    }

  public class StudentUpdateRequestValidator : AbstractValidator<StudentUpdateRequest>
  {
        private readonly IServiceProvider _serviceProvider;
        public StudentUpdateRequestValidator(IServiceProvider serviceProvider)
        {
          _serviceProvider = serviceProvider;
            RuleFor(x => x.Name).NotEmpty().NotNull().MinimumLength(4);
            RuleFor(x => x.Roll).NotEmpty().NotNull();
            RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
        }
  }
}
