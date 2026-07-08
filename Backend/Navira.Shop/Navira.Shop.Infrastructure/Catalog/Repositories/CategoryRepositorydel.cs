using Microsoft.EntityFrameworkCore;
using Navira.Shop.Core.Domain;
using Navira.Shop.Domain.Catalog.Entities;

namespace Navira.Shop.Infrastructure.Catalog.Repositories
{
    /// <summary>
    /// Write-side repository for the Category aggregate. Implements
    /// IAggregateRepository&lt;Category,int&gt; from Core.Domain — one repository per
    /// aggregate root, never per entity.
    ///
    /// GetByIdAsync always loads the complete aggregate. Category currently has no
    /// child entity collections (no nested owned types), so a single FirstOrDefaultAsync
    /// is sufficient. If subcategory navigation is added later, add .Include() here.
    ///
    /// There is no Update() method — EF's change tracker detects mutations made through
    /// the aggregate's methods (Rename, ChangeTaxCategory, etc.) automatically, and
    /// WriteFrameworkDbContext.Commit() persists them.
    ///
    /// Remove() marks the aggregate for deletion — AuditableShadowProperties intercepts
    /// EntityState.Deleted and converts it to a soft-delete (IsDeleted = true) rather
    /// than issuing a physical DELETE statement.
    /// </summary>
    public class CategoryRepositorydel : IAggregateRepository<Category, int>
    {
        private readonly DbContext _db;

        public CategoryRepositorydel(DbContext db)
        {
            _db = db;
        }

        public async Task<Category?> GetByIdAsync(int id, CancellationToken ct = default) => await _db.Set<Category>()
             .FirstOrDefaultAsync(c => c.Id == id, ct);

        public async Task AddAsync(Category aggregate, CancellationToken ct = default) =>
            await _db.Set<Category>().AddAsync(aggregate, ct);

        public void Remove(Category aggregate) =>
            _db.Set<Category>().Remove(aggregate);


    }

}
