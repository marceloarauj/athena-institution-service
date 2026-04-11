using Institution.Application.Dtos.Input;
using Institution.Application.Interfaces.Services;
using NCalc;

namespace Institution.Infrastructure.Services
{
    public class GradeService : IGradeService
    {
        public async Task<bool> IsApproved(List<TestGradeDto> grades, string formula)
        {
            var expression = new Expression(formula);

            foreach(var grade in grades)
            {
                expression.Parameters[grade.Key] = grade.Value;
            }

            var result = expression.Evaluate();

            if (result is bool boolResult)
                return boolResult;

            throw new InvalidOperationException("A expressão não retornou um valor booleano.");
        }
    }
}
