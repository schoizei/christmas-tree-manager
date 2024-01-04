using ChristmasTreeManager.Infrastructure;
using ChristmasTreeManager.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Text.Encodings.Web;

namespace ChristmasTreeManager.Services;

public class ApplicationDbService
{
    private readonly ApplicationDbContext _context;
    private readonly NavigationManager _navigationManager;

    public ApplicationDbService(ApplicationDbContext context, NavigationManager navigationManager)
    {
        _context = context;
        _navigationManager = navigationManager;
    }

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

    public async Task ExportRegistrationsToExcel(Query? query = null, string? fileName = null)
    {
        _navigationManager.NavigateTo(query is not null ? query.ToUrl($"export/registrations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/registrations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
    }

    public async Task ExportRegistrationsToCSV(Query? query = null, string? fileName = null)
    {
        _navigationManager.NavigateTo(query is not null ? query.ToUrl($"export/registrations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/registrations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
    }

    public async Task<IQueryable<Registration>> GetRegistrations(Query? query = null)
    {
        var items = _context.Registrations.AsQueryable();
        items = items.Include(i => i.Street);
        items = items.Include(i => i.RegistrationPoint);

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

        var result = items.ToList()
            .Select(Registration.FromEntity)
            .AsQueryable();
        return await Task.FromResult(result);
    }

    public async Task<Registration?> GetRegistrationById(string id)
    {
        var items = _context.Registrations
            .Include(i => i.Street)
            .Include(i => i.RegistrationPoint)
            .AsNoTracking()
            .Where(i => i.Id == id);

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
            .Where(i => i.Id == id)
            .FirstOrDefault();
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

    public async Task ExportCollectionToursToExcel(Query? query = null, string? fileName = null)
    {
        _navigationManager.NavigateTo(query is not null ? query.ToUrl($"export/collectiontours/excel(fileName='{(!string.IsNullOrEmpty(fileName) ?
            UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/collectiontours/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
    }

    public async Task ExportCollectionToursToCSV(Query? query = null, string? fileName = null)
    {
        _navigationManager.NavigateTo(query is not null ? query.ToUrl($"export/collectiontours/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/collectiontours/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
    }

    public async Task<IQueryable<CollectionTour>> GetCollectionTours(Query? query = null)
    {
        var items = _context.CollectionTours.AsQueryable();
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

        var result = items.ToList()
            .Select(CollectionTour.FromEntity)
            .AsQueryable();
        return await Task.FromResult(result);
    }

    public async Task<CollectionTour?> GetCollectionTourById(string id)
    {
        var items = _context.CollectionTours
            .AsNoTracking()
            .Where(i => i.Id == id);

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
        var itemToUpdate = _context.CollectionTours
            .Where(i => i.Id == id)
            .FirstOrDefault();
        if (itemToUpdate is null)
        {
            throw new Exception("Item no longer available");
        }

        var entryToUpdate = _context.Entry(itemToUpdate);
        entryToUpdate.Entity.Name = collectionTour.Name!;
        entryToUpdate.Entity.Vehicle = collectionTour.Vehicle;
        entryToUpdate.Entity.Driver = collectionTour.Driver;
        entryToUpdate.Entity.Staff = collectionTour.Staff;
        entryToUpdate.Entity.UpdatedBy = user;
        entryToUpdate.Entity.UpdatedAt = DateTime.UtcNow;
        entryToUpdate.State = EntityState.Modified;
        _context.SaveChanges();

        return CollectionTour.FromEntity(entryToUpdate.Entity);
    }

    public async Task<CollectionTour> DeleteCollectionTour(string id)
    {
        var itemToDelete = _context.CollectionTours
            .Where(i => i.Id == id)
            .FirstOrDefault();
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

    public async Task ExportDistributionToursToExcel(Query? query = null, string? fileName = null)
    {
        _navigationManager.NavigateTo(query is not null ? query.ToUrl($"export/distributiontours/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributiontours/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
    }

    public async Task ExportDistributionToursToCSV(Query? query = null, string? fileName = null)
    {
        _navigationManager.NavigateTo(query is not null ? query.ToUrl($"export/distributiontours/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributiontours/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
    }

    public async Task<IQueryable<DistributionTour>> GetDistributionTours(Query? query = null)
    {
        var items = _context.DistributionTours.AsQueryable();
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

        var result = items.ToList()
            .Select(DistributionTour.FromEntity)
            .AsQueryable();
        return await Task.FromResult(result);
    }


    public async Task<DistributionTour?> GetDistributionTourById(string id)
    {
        var items = _context.DistributionTours
            .AsNoTracking()
            .Where(i => i.Id == id);

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
        var itemToUpdate = _context.DistributionTours
            .Where(i => i.Id == id)
            .FirstOrDefault();
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
        var itemToDelete = _context.DistributionTours
            .Where(i => i.Id == id)
            .FirstOrDefault();
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

    public async Task ExportRegistrationPointsToExcel(Query? query = null, string? fileName = null)
    {
        _navigationManager.NavigateTo(query is not null ? query.ToUrl($"export/registrationpoints/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/registrationpoints/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
    }

    public async Task ExportRegistrationPointsToCSV(Query? query = null, string? fileName = null)
    {
        _navigationManager.NavigateTo(query is not null ? query.ToUrl($"export/registrationpoints/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/registrationpoints/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
    }


    public async Task<IQueryable<RegistrationPoint>> GetRegistrationPoints(Query? query = null)
    {
        var items = _context.RegistrationPoints.AsQueryable();
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

        var result = items.ToList()
            .Select(RegistrationPoint.FromEntity)
            .AsQueryable();
        return await Task.FromResult(result);
    }


    public async Task<RegistrationPoint?> GetRegistrationPointById(string id)
    {
        var items = _context.RegistrationPoints
            .AsNoTracking()
            .Where(i => i.Id == id);

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
        var itemToDelete = _context.RegistrationPoints
            .Where(i => i.Id == id)
            .FirstOrDefault();
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

    public async Task ExportStreetsToExcel(Query? query = null, string? fileName = null)
    {
        _navigationManager.NavigateTo(query is not null ? query.ToUrl($"export/streets/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/streets/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
    }

    public async Task ExportStreetsToCSV(Query? query = null, string? fileName = null)
    {
        _navigationManager.NavigateTo(query is not null ? query.ToUrl($"export/streets/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/streets/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
    }


    public async Task<IQueryable<Street>> GetStreets(Query? query = null)
    {
        var items = _context.Streets.AsQueryable();
        items = items.Include(i => i.CollectionTour);
        items = items.Include(i => i.DistributionTour);

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

        var result = items.ToList()
            .Select(Street.FromEntity)
            .AsQueryable();
        return await Task.FromResult(result);
    }

    public async Task<Street?> GetStreetById(string id)
    {
        var items = _context.Streets
            .AsNoTracking()
            .Include(i => i.DistributionTour)
            .Include(i => i.CollectionTour)
            .Include(i => i.Registrations)
            .Where(i => i.Id == id);

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
        var itemToUpdate = _context.Streets
            .Where(i => i.Id == id)
            .FirstOrDefault();
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
        entryToUpdate.Entity.DistributionTourOrderNumber = street.DistributionTourOrderNumber;
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
        var itemToDelete = _context.Streets
            .Where(i => i.Id == id)
            .FirstOrDefault();
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