using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GetReportCardLayoutHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<GetReportCardLayoutCommand, AthenaApiResponse<ReportCardLayoutResponseDto>>
    {
        public async Task<AthenaApiResponse<ReportCardLayoutResponseDto>> Handle(
            GetReportCardLayoutCommand request,
            CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<ReportCardLayoutResponseDto>.NotFound("Institution not found.");

            var layout = await unitOfWork.ReportCardLayoutRepository.FindByInstitutionIdAsync(institution.Id);

            if (layout == null)
            {
                var defaultLayout = new ReportCardLayoutEntity
                {
                    InstitutionId = institution.Id,
                    LayoutJson = DefaultLayoutJson
                };
                return AthenaApiResponse<ReportCardLayoutResponseDto>.Ok(new ReportCardLayoutResponseDto(defaultLayout));
            }

            return AthenaApiResponse<ReportCardLayoutResponseDto>.Ok(new ReportCardLayoutResponseDto(layout));
        }

        private const string DefaultLayoutJson = """
            {"pageBackground":"#ffffff","components":[{"id":"logo","type":"logo","label":"Logotipo","x":20,"y":20,"color":"#1a1a1a","visible":true,"fontSize":14,"fontWeight":"normal"},{"id":"institution_name","type":"institution_name","label":"Nome da Instituição","x":90,"y":28,"color":"#1a1a1a","visible":true,"fontSize":20,"fontWeight":"bold"},{"id":"student_name","type":"student_name","label":"Nome do Aluno","x":20,"y":120,"color":"#1a1a1a","visible":true,"fontSize":14,"fontWeight":"bold"},{"id":"class_info","type":"class_info","label":"Informações da Turma","x":20,"y":145,"color":"#555555","visible":true,"fontSize":12,"fontWeight":"normal"},{"id":"grades_table","type":"grades_table","label":"Tabela de Notas","x":20,"y":180,"color":"#1a1a1a","visible":true,"fontSize":12,"fontWeight":"normal"}]}
            """;
    }
}
