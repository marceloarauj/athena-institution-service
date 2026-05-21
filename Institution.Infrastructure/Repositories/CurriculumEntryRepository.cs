using Institution.Application.Interfaces.Repositories;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Institution.Infrastructure.Repositories
{
    public class CurriculumEntryRepository(AppDbContext dbContext) : ICurriculumEntryRepository
    {
        public async Task UpsertAsync(CurriculumEntryEntity entry)
        {
            var existing = await dbContext.CurriculumEntries
                .FirstOrDefaultAsync(x =>
                    x.ProgramEditionId == entry.ProgramEditionId &&
                    x.SubjectId == entry.SubjectId &&
                    x.GradeOrYear == entry.GradeOrYear &&
                    x.PeriodNumber == entry.PeriodNumber);

            if (existing == null)
            {
                await dbContext.CurriculumEntries.AddAsync(entry);
            }
            else
            {
                existing.WeeklyHours = entry.WeeklyHours;
                existing.TotalHours = entry.TotalHours;
                dbContext.CurriculumEntries.Update(existing);
            }
        }

        public async Task BulkUpsertAsync(List<CurriculumEntryEntity> entries)
        {
            foreach (var entry in entries)
                await UpsertAsync(entry);
        }

        public async Task<List<CurriculumEntryEntity>> GetByEditionAsync(Guid programEditionId, int? gradeOrYear = null)
        {
            var query = dbContext.CurriculumEntries
                .Include(x => x.Subject)
                .Where(x => x.ProgramEditionId == programEditionId);

            if (gradeOrYear.HasValue)
                query = query.Where(x => x.GradeOrYear == gradeOrYear.Value);

            return await query.OrderBy(x => x.GradeOrYear).ThenBy(x => x.PeriodNumber).ToListAsync();
        }
    }
}
