using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage.Binary;

public class ReportService(IDbAuditStorage<ReportView> storage, IMetricsService counterService) : IReportService
{
    public List<ReportView> All(Guid dbId) => storage.Where(x => x.DatabaseId == dbId);
    public void Add(ReportView report) => storage.SaveItem(report);
    public Option<ReportView> GetByEnvId(Guid envId) => storage.Find(x => x.EnvId == envId);

    public void Remove(Guid dbId) => storage.Find(x => x.DatabaseId == dbId).IfSome(r =>
    {
        counterService.Remove(r.Counters.Select(c => c.Id).ToArray());
        storage.RemoveByKey(v => v.DatabaseId == dbId);
    });

    public void AddCounter(Guid id, (string Title, string Value, Guid Id) counter) => storage.Update(r => r.Counters.Add(counter), x => x.Id == id);
    public Option<ReportView> GetByDbId(Guid id) => storage.Find(x => x.DatabaseId == id);
}