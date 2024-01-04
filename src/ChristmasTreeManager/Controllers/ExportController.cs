using ChristmasTreeManager.Infrastructure;
using ChristmasTreeManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChristmasTreeManager.Controllers;

public partial class ExportController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ApplicationDbService _service;
    private readonly ExportService _exportService;

    public ExportController(ApplicationDbContext context, ApplicationDbService service, ExportService exportService)
    {
        _service = service;
        _context = context;
        _exportService = exportService;
    }

    [HttpGet("/export/collectiontours/csv")]
    [HttpGet("/export/collectiontours/csv(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportCollectionToursToCSV(string? fileName = null)
    {
        return _exportService.ToCSV(_exportService.ApplyQuery(await _service.GetCollectionTours(), Request.Query, false), fileName);
    }

    [HttpGet("/export/collectiontours/excel(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportCollectionToursToExcel(string? fileName = null)
    {
        return _exportService.ToExcel(_exportService.ApplyQuery(await _service.GetCollectionTours(), Request.Query, false), fileName);
    }

    [HttpGet("/export/distributiontours/csv")]
    [HttpGet("/export/distributiontours/csv(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportDistributionToursToCSV(string? fileName = null)
    {
        return _exportService.ToCSV(_exportService.ApplyQuery(await _service.GetDistributionTours(), Request.Query, false), fileName);
    }

    [HttpGet("/export/distributiontours/excel")]
    [HttpGet("/export/distributiontours/excel(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportDistributionToursToExcel(string? fileName = null)
    {
        return _exportService.ToExcel(_exportService.ApplyQuery(await _service.GetDistributionTours(), Request.Query, false), fileName);
    }

    [HttpGet("/export/registrationpoints/csv")]
    [HttpGet("/export/registrationpoints/csv(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportRegistrationPointsToCSV(string? fileName = null)
    {
        return _exportService.ToCSV(_exportService.ApplyQuery(await _service.GetRegistrationPoints(), Request.Query, false), fileName);
    }

    [HttpGet("/export/registrationpoints/excel")]
    [HttpGet("/export/registrationpoints/excel(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportRegistrationPointsToExcel(string? fileName = null)
    {
        return _exportService.ToExcel(_exportService.ApplyQuery(await _service.GetRegistrationPoints(), Request.Query, false), fileName);
    }

    [HttpGet("/export/registrations/csv")]
    [HttpGet("/export/registrations/csv(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportRegistrationsToCSV(string? fileName = null)
    {
        return _exportService.ToCSV(_exportService.ApplyQuery(await _service.GetRegistrations(), Request.Query, false), fileName);
    }

    [HttpGet("/export/registrations/excel")]
    [HttpGet("/export/registrations/excel(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportRegistrationsToExcel(string? fileName = null)
    {
        return _exportService.ToExcel(_exportService.ApplyQuery(await _service.GetRegistrations(), Request.Query, false), fileName);
    }

    [HttpGet("/export/streets/csv")]
    [HttpGet("/export/streets/csv(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportStreetsToCSV(string? fileName = null)
    {
        return _exportService.ToCSV(_exportService.ApplyQuery(await _service.GetStreets(), Request.Query, false), fileName);
    }

    [HttpGet("/export/streets/excel")]
    [HttpGet("/export/streets/excel(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportStreetsToExcel(string? fileName = null)
    {
        return _exportService.ToExcel(_exportService.ApplyQuery(await _service.GetStreets(), Request.Query, false), fileName);
    }
}
