using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using WebApp;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Moq;
using Core.ReportingData;
using System;

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
}
