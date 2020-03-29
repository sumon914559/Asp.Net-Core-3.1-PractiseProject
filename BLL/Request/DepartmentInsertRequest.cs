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
    public class DepartmentInsertRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }


    public class DepartmentInsertRequestValidator : AbstractValidator<DepartmentInsertRequest>
    {
        private readonly IServiceProvider _serviceProvider;
        public DepartmentInsertRequestValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(4).MustAsync(NameExist).WithMessage("Already Name Exit.");
            RuleFor(x => x.Code).NotNull().NotEmpty().MaximumLength(10).MustAsync(CodeExist).WithMessage("Aleady Code Exit.");

        }

        private async Task<bool> CodeExist(string code, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }
            var deptService = _serviceProvider.GetRequiredService<IDepartmentServices>();
            return await deptService.IsCodeExit(code);
        }

        private async Task<bool> NameExist(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            var depService = _serviceProvider.GetRequiredService<IDepartmentServices>();
            return await depService.IsNameExit(name);
        }
        
    }
}
