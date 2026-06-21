using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class SaveReportCardLayoutHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<SaveReportCardLayoutCommand, AthenaApiResponse<ReportCardLayoutResponseDto>>
    {
        public async Task<AthenaApiResponse<ReportCardLayoutResponseDto>> Handle(
            SaveReportCardLayoutCommand request,
            CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<ReportCardLayoutResponseDto>.NotFound("Institution not found.");

            var existing = await unitOfWork.ReportCardLayoutRepository.FindByInstitutionIdAsync(institution.Id);

            if (existing == null)
            {
                var newLayout = new ReportCardLayoutEntity
                {
                    InstitutionId = institution.Id,
                    LayoutJson = request.Dto.LayoutJson
                };
                await unitOfWork.ReportCardLayoutRepository.AddAsync(newLayout);
                await unitOfWork.CommitAsync();
                return AthenaApiResponse<ReportCardLayoutResponseDto>.Created(new ReportCardLayoutResponseDto(newLayout));
            }

            existing.LayoutJson = request.Dto.LayoutJson;
            unitOfWork.ReportCardLayoutRepository.Update(existing);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<ReportCardLayoutResponseDto>.Ok(new ReportCardLayoutResponseDto(existing));
        }
    }
}
