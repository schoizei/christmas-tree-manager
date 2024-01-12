using ChristmasTreeManager.Data.Application;
using ChristmasTreeManager.Infrastructure;
using ChristmasTreeManager.Models;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Data;
using System.Linq.Dynamic.Core;

namespace ChristmasTreeManager.Services;

public class ApplicationDbService(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public void Reset() => _context.ChangeTracker.Entries().Where(e => e.Entity is not null).ToList().ForEach(e => e.State = EntityState.Detached);

    public void ApplyQuery<T>(ref IQueryable<T> items, Query? query = null)
    {
        if (query is null) return;

        if (!string.IsNullOrEmpty(query.Filter))
        {
            if (query.FilterParameters is not null)
            {
                if (query.FilterParameters[0] is string stringParam && string.IsNullOrEmpty(stringParam))
                {
                    // can not apply invalid filter param
                }
                else
                {
                    items = items.Where(query.Filter, query.FilterParameters);
                }
            }
            else
            {
                items = items.Where(query.Filter);
            }
        }

        if (!string.IsNullOrEmpty(query.OrderBy))
        {
            items = items.OrderBy(query.OrderBy);
        }

        if (query.Skip.HasValue)
        {
            items = items.Skip(query.Skip.Value);
        }

        if (query.Top.HasValue)
        {
            items = items.Take(query.Top.Value);
        }
    }

    public IQueryable<RegistrationEntity> GetRegistrationEntities()
    {
        return _context.Registrations.AsQueryable();
    }

    public async Task<IQueryable<Registration>> GetRegistrations(Query? query = null)
    {
        var items = GetRegistrationEntities();

        if (query is not null)
        {
            if (!string.IsNullOrEmpty(query.Expand))
            {
                var propertiesToExpand = query.Expand.Split(',');
                foreach (var p in propertiesToExpand)
                {
                    items = items.Include(p.Trim());
                }
            }

            ApplyQuery(ref items, query);
        }

        return await Task.FromResult(items.Select(Registration.FromEntity).AsQueryable());
    }

    public async Task<IQueryable<RegistrationExport>> GetRegistrationsForCollectionTour(string collectionTourId)
    {
        var items = GetRegistrationEntities()
            .Join(GetStreetEntities(),
                registration => registration.StreetId, street => street.Id,
                (registration, street) => new { Registration = registration, Street = street })
            .Where(joinResult => joinResult.Street.CollectionTourId == collectionTourId)
            .Select(joinResult => joinResult.Registration)
            .Include(x => x.Street)
            .Include(x => x.RegistrationPoint)
            .OrderBy(x => x.Street.CollectionTourOrderNumber)
            .ThenBy(x => x.Street.Name)
            .ThenBy(x => x.Housenumber);

        return await Task.FromResult(items.Select(RegistrationExport.FromEntity).AsQueryable());
    }

    public async Task<Registration?> GetRegistrationById(string id)
    {
        var items = _context.Registrations
            .Include(i => i.Street)
            .Include(i => i.RegistrationPoint)
            .Where(i => i.Id == id)
            .AsNoTracking();

        var itemToReturn = items.FirstOrDefault();
        if (itemToReturn is null) return null;

        return await Task.FromResult(Registration.FromEntity(itemToReturn));
    }

    public async Task<Registration> CreateRegistration(string user, Registration registration)
    {
        var entity = registration.ToEntity();
        entity.CreatedBy = user;
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedBy = string.Empty;
        entity.UpdatedAt = DateTime.UtcNow;

        try
        {
            _context.Registrations.Add(entity);
            _context.SaveChanges();
        }
        catch
        {
            _context.Entry(entity).State = EntityState.Detached;
            throw;
        }

        return Registration.FromEntity(entity);
    }

    public async Task<Registration> UpdateRegistration(string user, string id, Registration registration)
    {
        var itemToUpdate = _context.Registrations
            .Where(i => i.Id == id)
            .FirstOrDefault();
        if (itemToUpdate is null)
        {
            throw new Exception("Item no longer available");
        }

        var entryToUpdate = _context.Entry(itemToUpdate);
        entryToUpdate.Entity.RegistrationPointId = registration.RegistrationPointId!;
        entryToUpdate.Entity.RegistrationDate = registration.RegistrationDate;
        entryToUpdate.Entity.Customer = registration.Customer!;
        entryToUpdate.Entity.StreetId = registration.StreetId!;
        entryToUpdate.Entity.Housenumber = registration.Housenumber!.Value;
        entryToUpdate.Entity.HousenumberPostfix = registration.HousenumberPostfix;
        entryToUpdate.Entity.Phone = registration.Phone!;
        entryToUpdate.Entity.Mail = registration.Mail!;
        entryToUpdate.Entity.TreeCount = registration.TreeCount;
        entryToUpdate.Entity.Donation = registration.Donation;
        entryToUpdate.Entity.Comment = registration.Comment;
        entryToUpdate.Entity.UpdatedBy = user;
        entryToUpdate.Entity.UpdatedAt = DateTime.UtcNow;
        entryToUpdate.State = EntityState.Modified;
        _context.SaveChanges();

        return Registration.FromEntity(entryToUpdate.Entity);
    }

    public async Task<Registration> DeleteRegistration(string id)
    {
        var itemToDelete = _context.Registrations
            .FirstOrDefault(i => i.Id == id);

        if (itemToDelete is null)
        {
            throw new Exception("Item no longer available");
        }

        _context.Registrations.Remove(itemToDelete);
        try
        {
            _context.SaveChanges();
        }
        catch
        {
            _context.Entry(itemToDelete).State = EntityState.Unchanged;
            throw;
        }

        return Registration.FromEntity(itemToDelete);
    }

    public IQueryable<CollectionTourEntity> GetCollectionTourEntities()
    {
        return _context.CollectionTours.AsQueryable();
    }

    public async Task<IQueryable<CollectionTour>> GetCollectionTours(Query? query = null)
    {
        var items = GetCollectionTourEntities();

        if (query is not null)
        {
            if (!string.IsNullOrEmpty(query.Expand))
            {
                var propertiesToExpand = query.Expand.Split(',');
                foreach (var p in propertiesToExpand)
                {
                    items = items.Include(p.Trim());
                }
            }

            ApplyQuery(ref items, query);
        }

        return await Task.FromResult(items.Select(CollectionTour.FromEntity).AsQueryable());
    }

    public async Task<CollectionTour?> GetCollectionTourById(string id)
    {
        var items = _context.CollectionTours
            .Where(i => i.Id == id)
            .AsNoTracking();

        var itemToReturn = items.FirstOrDefault();
        if (itemToReturn is null) return null;

        return await Task.FromResult(CollectionTour.FromEntity(itemToReturn));
    }

    public async Task<CollectionTour> CreateCollectionTour(string user, CollectionTour collectionTour)
    {
        var entity = collectionTour.ToEntity();
        entity.CreatedBy = user;
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedBy = string.Empty;
        entity.UpdatedAt = DateTime.UtcNow;

        try
        {
            _context.CollectionTours.Add(entity);
            _context.SaveChanges();
        }
        catch
        {
            _context.Entry(entity).State = EntityState.Detached;
            throw;
        }

        return CollectionTour.FromEntity(entity);
    }

    public async Task<CollectionTour> UpdateCollectionTour(string user, string id, CollectionTour collectionTour)
    {
        var itemToUpdate = _context.CollectionTours.FirstOrDefault(i => i.Id == id);
        if (itemToUpdate is null)
        {
            throw new Exception("Item no longer available");
        }

        var entryToUpdate = _context.Entry(itemToUpdate);
        entryToUpdate.Entity.Name = collectionTour.Name!;
        entryToUpdate.Entity.Vehicle = collectionTour.Vehicle;
        entryToUpdate.Entity.Driver = collectionTour.Driver;
        entryToUpdate.Entity.TeamLeader = collectionTour.TeamLeader;
        entryToUpdate.Entity.Staff = collectionTour.Staff;
        entryToUpdate.Entity.UpdatedBy = user;
        entryToUpdate.Entity.UpdatedAt = DateTime.UtcNow;
        entryToUpdate.State = EntityState.Modified;
        _context.SaveChanges();

        return CollectionTour.FromEntity(entryToUpdate.Entity);
    }

    public async Task<CollectionTour> DeleteCollectionTour(string id)
    {
        var itemToDelete = _context.CollectionTours.FirstOrDefault(i => i.Id == id);
        if (itemToDelete is null)
        {
            throw new Exception("Item no longer available");
        }

        _context.CollectionTours.Remove(itemToDelete);
        try
        {
            _context.SaveChanges();
        }
        catch
        {
            _context.Entry(itemToDelete).State = EntityState.Unchanged;
            throw;
        }

        return CollectionTour.FromEntity(itemToDelete);
    }

    public IQueryable<DistributionTourEntity> GetDistributionTourEntities()
    {
        return _context.DistributionTours.AsQueryable();
    }

    public async Task<IQueryable<DistributionTour>> GetDistributionTours(Query? query = null)
    {
        var items = GetDistributionTourEntities();

        if (query is not null)
        {
            if (!string.IsNullOrEmpty(query.Expand))
            {
                var propertiesToExpand = query.Expand.Split(',');
                foreach (var p in propertiesToExpand)
                {
                    items = items.Include(p.Trim());
                }
            }

            ApplyQuery(ref items, query);
        }

        return await Task.FromResult(items.Select(DistributionTour.FromEntity).AsQueryable());
    }


    public async Task<DistributionTour?> GetDistributionTourById(string id)
    {
        var items = _context.DistributionTours
            .Where(i => i.Id == id)
            .AsNoTracking();

        var itemToReturn = items.FirstOrDefault();
        if (itemToReturn is null) return null;

        return await Task.FromResult(DistributionTour.FromEntity(itemToReturn));
    }

    public async Task<DistributionTour> CreateDistributionTour(string user, DistributionTour distributionTour)
    {
        var entity = distributionTour.ToEntity();
        entity.CreatedBy = user;
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedBy = string.Empty;
        entity.UpdatedAt = DateTime.UtcNow;

        try
        {
            _context.DistributionTours.Add(entity);
            _context.SaveChanges();
        }
        catch
        {
            _context.Entry(entity).State = EntityState.Detached;
            throw;
        }

        return DistributionTour.FromEntity(entity);
    }

    public async Task<DistributionTour> UpdateDistributionTour(string user, string id, DistributionTour distributionTour)
    {
        var itemToUpdate = _context.DistributionTours.FirstOrDefault(i => i.Id == id);
        if (itemToUpdate is null)
        {
            throw new Exception("Item no longer available");
        }

        var entryToUpdate = _context.Entry(itemToUpdate);
        entryToUpdate.Entity.Name = distributionTour.Name!;
        entryToUpdate.Entity.Staff = distributionTour.Staff;
        entryToUpdate.Entity.UpdatedBy = user;
        entryToUpdate.Entity.UpdatedAt = DateTime.UtcNow;
        entryToUpdate.State = EntityState.Modified;
        _context.SaveChanges();

        return DistributionTour.FromEntity(entryToUpdate.Entity);
    }

    public async Task<DistributionTour> DeleteDistributionTour(string id)
    {
        var itemToDelete = _context.DistributionTours.FirstOrDefault(i => i.Id == id);
        if (itemToDelete is null)
        {
            throw new Exception("Item no longer available");
        }

        _context.DistributionTours.Remove(itemToDelete);
        try
        {
            _context.SaveChanges();
        }
        catch
        {
            _context.Entry(itemToDelete).State = EntityState.Unchanged;
            throw;
        }

        return DistributionTour.FromEntity(itemToDelete);
    }

    public IQueryable<RegistrationPointEntity> GetRegistrationPointEntities()
    {
        return _context.RegistrationPoints.AsQueryable();
    }

    public async Task<IQueryable<RegistrationPoint>> GetRegistrationPoints(Query? query = null)
    {
        var items = GetRegistrationPointEntities();

        if (query is not null)
        {
            if (!string.IsNullOrEmpty(query.Expand))
            {
                var propertiesToExpand = query.Expand.Split(',');
                foreach (var p in propertiesToExpand)
                {
                    items = items.Include(p.Trim());
                }
            }

            ApplyQuery(ref items, query);
        }

        return await Task.FromResult(items.Select(RegistrationPoint.FromEntity).AsQueryable());
    }


    public async Task<RegistrationPoint?> GetRegistrationPointById(string id)
    {
        var items = _context.RegistrationPoints
            .Where(i => i.Id == id)
            .AsNoTracking();

        var itemToReturn = items.FirstOrDefault();
        if (itemToReturn is null) return null;

        return await Task.FromResult(RegistrationPoint.FromEntity(itemToReturn));
    }

    public async Task<RegistrationPoint> CreateRegistrationPoint(string user, RegistrationPoint registrationPoint)
    {
        var entity = registrationPoint.ToEntity();
        entity.CreatedBy = user;
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedBy = string.Empty;
        entity.UpdatedAt = DateTime.UtcNow;

        try
        {
            _context.RegistrationPoints.Add(entity);
            _context.SaveChanges();
        }
        catch
        {
            _context.Entry(entity).State = EntityState.Detached;
            throw;
        }

        return RegistrationPoint.FromEntity(entity);
    }

    public async Task<RegistrationPoint> UpdateRegistrationPoint(string user, string id, RegistrationPoint registrationPoint)
    {
        var itemToUpdate = _context.RegistrationPoints
            .Where(i => i.Id == id)
            .FirstOrDefault();
        if (itemToUpdate is null)
        {
            throw new Exception("Item no longer available");
        }

        var entryToUpdate = _context.Entry(itemToUpdate);
        entryToUpdate.Entity.Name = registrationPoint.Name!;
        entryToUpdate.Entity.Address = registrationPoint.Address!;
        entryToUpdate.Entity.UpdatedBy = user;
        entryToUpdate.Entity.UpdatedAt = DateTime.UtcNow;
        entryToUpdate.State = EntityState.Modified;
        _context.SaveChanges();

        return RegistrationPoint.FromEntity(entryToUpdate.Entity);
    }

    public async Task<RegistrationPoint> DeleteRegistrationPoint(string id)
    {
        var itemToDelete = _context.RegistrationPoints.FirstOrDefault(i => i.Id == id);
        if (itemToDelete is null)
        {
            throw new Exception("Item no longer available");
        }

        _context.RegistrationPoints.Remove(itemToDelete);
        try
        {
            _context.SaveChanges();
        }
        catch
        {
            _context.Entry(itemToDelete).State = EntityState.Unchanged;
            throw;
        }

        return RegistrationPoint.FromEntity(itemToDelete);
    }

    public IQueryable<StreetEntity> GetStreetEntities()
    {
        return _context.Streets.AsQueryable();
    }

    public async Task<IQueryable<Street>> GetStreets(Query? query = null)
    {
        var items = GetStreetEntities();

        if (query is not null)
        {
            if (!string.IsNullOrEmpty(query.Expand))
            {
                var propertiesToExpand = query.Expand.Split(',');
                foreach (var p in propertiesToExpand)
                {
                    items = items.Include(p.Trim());
                }
            }

            ApplyQuery(ref items, query);
        }

        return await Task.FromResult(items.Select(Street.FromEntity).AsQueryable());
    }

    public async Task<Street?> GetStreetById(string id)
    {
        var items = _context.Streets
            .Include(i => i.DistributionTour)
            .Include(i => i.CollectionTour)
            .Include(i => i.Registrations)
            .Where(i => i.Id == id)
            .AsNoTracking();

        var itemToReturn = items.FirstOrDefault();
        if (itemToReturn is null) return null;

        return await Task.FromResult(Street.FromEntity(itemToReturn));
    }

    public async Task<Street> CreateStreet(string user, Street street)
    {
        var entity = street.ToEntity();
        entity.CreatedBy = user;
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedBy = string.Empty;
        entity.UpdatedAt = DateTime.UtcNow;

        try
        {
            _context.Streets.Add(entity);
            _context.SaveChanges();
        }
        catch
        {
            _context.Entry(entity).State = EntityState.Detached;
            throw;
        }

        return Street.FromEntity(entity);
    }

    public async Task<Street> UpdateStreet(string user, string id, Street street)
    {
        var itemToUpdate = _context.Streets.FirstOrDefault(i => i.Id == id);
        if (itemToUpdate is null)
        {
            throw new Exception("Item no longer available");
        }

        var entryToUpdate = _context.Entry(itemToUpdate);
        entryToUpdate.Entity.ZipCode = street.ZipCode!;
        entryToUpdate.Entity.City = street.City!;
        entryToUpdate.Entity.District = street.District!;
        entryToUpdate.Entity.Name = street.Name!;
        entryToUpdate.Entity.LowestHouseNumber = street.LowestHouseNumber;
        entryToUpdate.Entity.HighestHouseNumber = street.HighestHouseNumber;
        entryToUpdate.Entity.DistributionTourId = street.DistributionTourId;
        entryToUpdate.Entity.DistributionTourFormCount = street.DistributionTourFormCount;
        entryToUpdate.Entity.CollectionTourId = street.CollectionTourId;
        entryToUpdate.Entity.CollectionTourOrderNumber = street.CollectionTourOrderNumber;
        entryToUpdate.Entity.UpdatedBy = user;
        entryToUpdate.Entity.UpdatedAt = DateTime.UtcNow;
        entryToUpdate.State = EntityState.Modified;
        _context.SaveChanges();

        return Street.FromEntity(entryToUpdate.Entity);
    }

    public async Task<Street> DeleteStreet(string id)
    {
        var itemToDelete = _context.Streets.FirstOrDefault(i => i.Id == id);
        if (itemToDelete is null)
        {
            throw new Exception("Item no longer available");
        }

        _context.Streets.Remove(itemToDelete);
        try
        {
            _context.SaveChanges();
        }
        catch
        {
            _context.Entry(itemToDelete).State = EntityState.Unchanged;
            throw;
        }

        return Street.FromEntity(itemToDelete);
    }
}