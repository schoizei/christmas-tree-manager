using ChristmasTreeManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChristmasTreeManager.Controllers;

public partial class ExportController : Controller
{
    private readonly ApplicationDbService _applicationDbService;
    private readonly ExportService _exportService;

    public ExportController(ApplicationDbService service, ExportService exportService)
    {
        _applicationDbService = service;
        _exportService = exportService;
    }

    [HttpGet("/export/collectiontours/csv")]
    [HttpGet("/export/collectiontours/csv(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportCollectionToursToCSV(string? fileName = null)
    {
        return _exportService.ToCSV(await _applicationDbService.GetCollectionTours(_exportService.ExtractQuery(Request.Query)), fileName);
    }

    [HttpGet("export/collectiontours/{collectionTourId}/registrations/csv")]
    [HttpGet("export/collectiontours/{collectionTourId}/registrations/csv(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportCollectionToursToCSV(string collectionTourId, string? fileName = null)
    {
        return _exportService.ToCSV(await _applicationDbService.GetRegistrationsForCollectionTour(collectionTourId), fileName);
    }

    [HttpGet("export/collectiontours/{collectionTourId}/registrations/pdf")]
    [HttpGet("export/collectiontours/{collectionTourId}/registrations/pdf(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportCollectionToursToPDF(string collectionTourId, string? fileName = null)
    {
        return _exportService.CollectionToursToPDF(await _applicationDbService.GetCollectionTourExport(collectionTourId), fileName);
    }

    [HttpGet("/export/collectiontours/excel")]
    [HttpGet("/export/collectiontours/excel(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportCollectionToursToExcel(string? fileName = null)
    {
        return _exportService.ToExcel(await _applicationDbService.GetCollectionTours(_exportService.ExtractQuery(Request.Query)), fileName);
    }

    [HttpGet("export/collectiontours/{collectionTourId}/registrations/excel")]
    [HttpGet("export/collectiontours/{collectionTourId}/registrations/excel(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportCollectionToursToExcel(string collectionTourId, string? fileName = null)
    {
        return _exportService.ToExcel(await _applicationDbService.GetRegistrationsForCollectionTour(collectionTourId), fileName);
    }

    [HttpGet("/export/distributiontours/csv")]
    [HttpGet("/export/distributiontours/csv(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportDistributionToursToCSV(string? fileName = null)
    {
        return _exportService.ToCSV(await _applicationDbService.GetDistributionTours(_exportService.ExtractQuery(Request.Query)), fileName);
    }

    [HttpGet("/export/distributiontours/excel")]
    [HttpGet("/export/distributiontours/excel(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportDistributionToursToExcel(string? fileName = null)
    {
        return _exportService.ToExcel(await _applicationDbService.GetDistributionTours(_exportService.ExtractQuery(Request.Query)), fileName);
    }

    [HttpGet("export/distributiontours/{distributionTourId}/streets/pdf")]
    [HttpGet("export/distributiontours/{distributionTourId}/streets/pdf(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportDistributionToursToPDF(string distributionTourId, string? fileName = null)
    {
        return _exportService.DistributionToursToPDF(await _applicationDbService.GetDistributionTourExport(distributionTourId), fileName);
    }

    [HttpGet("/export/registrationpoints/csv")]
    [HttpGet("/export/registrationpoints/csv(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportRegistrationPointsToCSV(string? fileName = null)
    {
        return _exportService.ToCSV(await _applicationDbService.GetRegistrationPoints(_exportService.ExtractQuery(Request.Query)), fileName);
    }

    [HttpGet("/export/registrationpoints/excel")]
    [HttpGet("/export/registrationpoints/excel(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportRegistrationPointsToExcel(string? fileName = null)
    {
        return _exportService.ToExcel(await _applicationDbService.GetRegistrationPoints(_exportService.ExtractQuery(Request.Query)), fileName);
    }

    [HttpGet("/export/registrations/csv")]
    [HttpGet("/export/registrations/csv(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportRegistrationsToCSV(string? fileName = null)
    {
        return _exportService.ToCSV(await _applicationDbService.GetRegistrations(_exportService.ExtractQuery(Request.Query)), fileName);
    }

    [HttpGet("/export/registrations/excel")]
    [HttpGet("/export/registrations/excel(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportRegistrationsToExcel(string? fileName = null)
    {
        return _exportService.ToExcel(await _applicationDbService.GetRegistrations(_exportService.ExtractQuery(Request.Query)), fileName);
    }

    [HttpGet("/export/streets/csv")]
    [HttpGet("/export/streets/csv(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportStreetsToCSV(string? fileName = null)
    {
        return _exportService.ToCSV(await _applicationDbService.GetStreets(_exportService.ExtractQuery(Request.Query)), fileName);
    }

    [HttpGet("/export/streets/excel")]
    [HttpGet("/export/streets/excel(fileName='{fileName}')")]
    public async Task<FileStreamResult> ExportStreetsToExcel(string? fileName = null)
    {
        return _exportService.ToExcel(await _applicationDbService.GetStreets(_exportService.ExtractQuery(Request.Query)), fileName);
    }
}
