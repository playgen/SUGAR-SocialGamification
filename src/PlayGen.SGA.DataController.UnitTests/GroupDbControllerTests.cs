using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataModel;
using Xunit;

namespace PlayGen.SGA.DataController.UnitTests
{
    public class GroupDbControllerTests : TestDbController
    {
        #region Configuration
    private readonly GroupDbController _groupDbController;

    public GroupDbControllerTests()
    {
            _groupDbController = new GroupDbController(_nameOrConnectionString);
    }
    #endregion


    #region Tests
    [Fact]
    public void CreateAndGetGroup()
    {
        string groupName = "CreateGroup";

        CreateGroup(groupName);

        var groups = _groupDbController.Get(new string[] { groupName });

        int matches = groups.Count(g => g.Name == groupName);

        Assert.Equal(matches, 1);
    }

    [Fact]
    public void CreateDuplicateGroup()
    {
        string groupName = "CreateDuplicateGroup";

        CreateGroup(groupName);

        bool hadDuplicateException = false;

        try
        {
            CreateGroup(groupName);
        }
        catch (DuplicateRecordException)
        {
            hadDuplicateException = true;
        }

        Assert.True(hadDuplicateException);
    }

    [Fact]
    public void GetMultipleGroups()
    {
        string[] groupNames = new[]
        {
                "GetMultipleGroups1",
                "GetMultipleGroups2",
                "GetMultipleGroups3",
                "GetMultipleGroups4",
            };

        foreach (var groupName in groupNames)
        {
            CreateGroup(groupName);
        }

        CreateGroup("GetMultipleGroups_DontGetThis");

        var groups = _groupDbController.Get(groupNames);

        var matchingGroups = groups.Select(g => groupNames.Contains(g.Name));

        Assert.Equal(matchingGroups.Count(), groupNames.Length);
    }

    [Fact]
    public void GetNonExistingGroups()
    {
        var groups = _groupDbController.Get(new string[] { "GetNonExsitingGroups" });

        Assert.Empty(groups);
    }

    [Fact]
    public void DeleteExistingGroup()
    {
        string groupName = "DeleteExistingGroup";

        var group = CreateGroup(groupName);

        var groups = _groupDbController.Get(new string[] { groupName });
        Assert.Equal(groups.Count(), 1);
        Assert.Equal(groups.ElementAt(0).Name, groupName);

        _groupDbController.Delete(new[] { group.Id });
        groups = _groupDbController.Get(new string[] { groupName });

        Assert.Empty(groups);
    }

    [Fact]
    public void DeleteNonExistingGroup()
    {
        bool hadException = false;

        try
        {
            _groupDbController.Delete(new int[] { -1 });
        }
        catch (Exception)
        {
            hadException = true;
        }

        Assert.False(hadException);
    }
    #endregion

    #region Helpers
    private Group CreateGroup(string name)
    {
        var newGroup = new Group
        {
            Name = name,
        };

        return _groupDbController.Create(newGroup);
    }
    #endregion
}
}
