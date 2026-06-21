using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Auth;
using Institution.Application.Commands;
using Institution.Application.Interfaces;
using Institution.Application.Interfaces.Services;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GenerateReportCardHandler(
        IUnitOfWork unitOfWork,
        IInstitutionContext institutionContext,
        IReportCardService reportCardService,
        IUser user)
        : IMessageHandler<GenerateReportCardCommand, AthenaApiResponse<byte[]>>
    {
        public async Task<AthenaApiResponse<byte[]>> Handle(
            GenerateReportCardCommand request,
            CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<byte[]>.NotFound("Institution not found.");

            var layout = await unitOfWork.ReportCardLayoutRepository.FindByInstitutionIdAsync(institution.Id);
            var layoutJson = layout?.LayoutJson ?? DefaultLayoutJson;

            var pdfBytes = await reportCardService.GeneratePdfAsync(institution, user.UserId, layoutJson, cancellationToken);

            if (pdfBytes.Length == 0)
                return AthenaApiResponse<byte[]>.NotFound("Nenhuma nota encontrada para este aluno.");

            return AthenaApiResponse<byte[]>.Ok(pdfBytes);
        }

        private const string DefaultLayoutJson = """{"pageBackground":"#ffffff","components":[{"id":"institution_name","type":"institution_name","visible":true,"color":"#1a1a1a","fontSize":20,"fontWeight":"bold"},{"id":"student_name","type":"student_name","visible":true,"color":"#1a1a1a","fontSize":14,"fontWeight":"bold"},{"id":"grades_table","type":"grades_table","visible":true,"color":"#1a1a1a","fontSize":12,"fontWeight":"normal"}]}""";
    }
}
