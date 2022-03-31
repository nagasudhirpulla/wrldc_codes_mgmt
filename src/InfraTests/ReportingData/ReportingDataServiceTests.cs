using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using WebApp;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Moq;
using Core.ReportingData;
using System;
using Core.ReportingData.ElementsForDisplay;

namespace Infra.ReportingData.Tests;

[TestClass()]
public class ReportingDataServiceTests
{
    private readonly IConfiguration _config;
    private readonly ReportingDataService _reportingDataService;

    public ReportingDataServiceTests()
    {
        _config = new ConfigurationBuilder().AddUserSecrets(typeof(Startup).GetTypeInfo().Assembly).Build();
        _reportingDataService = new ReportingDataService(_config, Mock.Of<ILogger<ReportingDataService>>());
    }

    [TestMethod()]
    public void GetReportingStakeHoldersTest()
    {
        List<ReportingStakeholder> stakeholders = _reportingDataService.GetReportingStakeHolders();
        Assert.IsTrue(stakeholders.Count > 0);
    }

    [TestMethod()]
    public void GetReportingOwnersTest()
    {
        List<ReportingOwner> owners = _reportingDataService.GetReportingOwners();
        Assert.IsTrue(owners.Count > 0);
    }

    [TestMethod()]
    public void GetApprovedOutageRequestsForDateTest()
    {
        DateTime inpDate = DateTime.Now;
        List<ReportingOutageRequest> outageRequests = _reportingDataService.GetApprovedOutageRequestsForDate(inpDate);
        Assert.IsTrue(outageRequests.Count > 0);
    }

    [TestMethod()]
    public void GetUnrevivedOutagesTest()
    {
        List<ReportingOutage> unrevOutages = _reportingDataService.GetLatestUnrevivedOutages();
        Assert.IsTrue(unrevOutages.Count > 0);
    }

    [TestMethod()]
    public void GetAllBusesTest()
    {
        List<ReportingBus> allBuses = _reportingDataService.GetAllBuses();
        Assert.IsTrue(allBuses.Count > 0);
    }

    [TestMethod()]
    public void GetAllBaysTest()
    {
        List<ReportingBay> allBays = _reportingDataService.GetAllBays();
        Assert.IsTrue(allBays.Count > 0);
    }

    [TestMethod()]
    public void GetAllBusReactorsTest()
    {
        List<ReportingBusReactor> allBusReactors = _reportingDataService.GetAllBusReactors();
        Assert.IsTrue(allBusReactors.Count > 0);
    }

    [TestMethod()]
    public void GetAllFSCsTest()
    {
        List<ReportingFsc> allFSCs = _reportingDataService.GetAllFscs();
        Assert.IsTrue(allFSCs.Count > 0);
    }

    [TestMethod()]
    public void GetAllGeneratingUnitsTest()
    {
        List<ReportingGeneratingUnit> allGeneratingUnits = _reportingDataService.GetAllGeneratingUnits();
        Assert.IsTrue(allGeneratingUnits.Count > 0);
    }

    [TestMethod()]
    public void GetAllHVDCLineCrktsTest()
    {
        List<ReportingHvdcLineCkt> allHVDCLineCrkts = _reportingDataService.GetAllHvdcLineCkts();
        Assert.IsTrue(allHVDCLineCrkts.Count > 0);
    }

    [TestMethod()]
    public void GetAllHVDCPolesTest()
    {
        List<ReportingHvdcPole> allHVDCPoles = _reportingDataService.GetAllHvdcPoles();
        Assert.IsTrue(allHVDCPoles.Count > 0);
    }

    [TestMethod()]
    public void GetAllLineReactorsTest()
    {
        List<ReportingLineReactor> allLineReactors = _reportingDataService.GetAllLineReactors();
        Assert.IsTrue(allLineReactors.Count > 0);
    }

    [TestMethod()]
    public void GetAllTransformersTest()
    {
        List<ReportingTransformer> allTransformers = _reportingDataService.GetAllTransformers();
        Assert.IsTrue(allTransformers.Count > 0);
    }

    [TestMethod()]
    public void GetAllTransmissionLineCktsTest()
    {
        List<ReportingTransmissionLineCkt> allTransmissionLineCkts = _reportingDataService.GetAllTransmissionLineCkts();
        Assert.IsTrue(allTransmissionLineCkts.Count > 0);
    }

    [TestMethod()]
    public void GetAllCompensatorsTest()
    {
        List<ReportingCompensator> allCompensators = _reportingDataService.GetAllCompensators();
        Assert.IsTrue(allCompensators.Count > 0);
    }

    [TestMethod()]
    public void GetElementTypesTest()
    {
        List<ElementType> elementtypes = _reportingDataService.GetElementTypes();
        Assert.IsTrue(elementtypes.Count > 0);
    }
}
