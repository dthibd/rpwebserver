using AutoMapper;
using WebComponentServer.AutoMapping;
using WebComponentServer.Models.RouteConfig;
using WebComponentServer.Services.ReverseProxy.Config.Route;

namespace WebComponentServerTest.AutoMapping;

public class RouteConfigProfileTest
{
    public MapperConfiguration MapperConfig { get; } = new(cfg => cfg.AddProfile<RouteConfigProfile>());
    public IMapper Mapper => MapperConfig.CreateMapper();

    [Fact]
    public void Configuration_IsValid()
    {
        MapperConfig.AssertConfigurationIsValid();
    }

    [Fact]
    public void MatchDtoToMatchConfig()
    {
        var dto = new RouteMatchDto()
        {
            Path = "some-path"
        };
        
        var matchConfig = Mapper.Map<MutableRouteMatch>(dto);

        Assert.NotNull(matchConfig);
        Assert.NotNull(matchConfig.Path);
    }

    [Fact]
    public void TransformsDtoToTransformsConfig()
    {
        var dto = new List<Dictionary<string, string>>
        {
            new()
            {
                {"PathRemovePrefix", "dto-path-remove-prefix"}
            }
        };
        var transformConfig = Mapper.Map<MutableRouteTransforms>(dto);
    
        Assert.NotNull(transformConfig);
    }
    
    [Fact]
    public void RouteDtoToRouteConfig()
    {
        var dto = new RouteConfigDto()
        {
            RouteId = "routeId",
            ClusterId = "clusterId",
            Match = new RouteMatchDto()
            {
                Path = "dto-path-match"
            },
            Transforms = new List<Dictionary<string, string>>()
            {
                new()
                {
                    {"PathRemovePrefix", "dto-path-remove-prefix"}
                }
            }
        };
        
        var config = Mapper.Map<MutableRouteConfig>(dto);

        Assert.Equal("routeId", config.RouteId);
        Assert.Equal("clusterId", config.ClusterId);
        Assert.NotNull(config.Match);
        Assert.NotNull(config.Transforms);

        var match = config.Match;
        Assert.NotNull(match.Path);
        Assert.Equal("dto-path-match", match.Path);

        var transforms = config.Transforms;

        Assert.Single(transforms.Transforms);
        var transformDocItem = transforms.Transforms.First();

        Assert.Single(transformDocItem);
        Assert.Equal("PathRemovePrefix", transformDocItem.Keys.First());
        Assert.Equal("dto-path-remove-prefix", transformDocItem.Values.First());
    }

    [Fact]
    public void MutableRouteMatchToDto()
    {
        var config = new MutableRouteMatch()
        {
            Path = "some-path"
        };
        
        var dto = Mapper.Map<RouteMatchDto>(config);

        Assert.NotNull(dto);
        Assert.NotNull(dto.Path);
        Assert.Equal("some-path", dto.Path);
    }

    [Fact]
    public void MutableRouteTransformToDto()
    {
        var config = new MutableRouteTransforms();
        config.AddPathRemovePrefix("path-remove-prefix-value");
        
        var dto = Mapper.Map<List<Dictionary<string, string>>>(config);

        Assert.NotNull(dto);
        Assert.Single(dto);

        var dictValue = dto.First();
        Assert.Equal("PathRemovePrefix", dictValue.Keys.First());
        Assert.Equal("path-remove-prefix-value", dictValue.Values.First());
    }

    [Fact]
    public void MutableRouteToDto()
    {
        var config = new MutableRouteConfig("routeId");
        config
            .Set
            .ClusterId("clusterId")
            .Match_CatchAllPath("match-path")
            .Transforms_PathRemovePrefix("transform-value");
        
        var dto = Mapper.Map<RouteConfigDto>(config);

        Assert.NotNull(dto);
        Assert.Equal("routeId", dto.RouteId);
        Assert.Equal("clusterId", dto.ClusterId);

        var match = dto.Match;
        Assert.NotNull(match);
        Assert.NotNull(match.Path);
        Assert.Equal("match-path/{**catch-all}", match.Path);
        
        var tranformsDict = dto.Transforms;
        Assert.NotNull(tranformsDict);
        Assert.Single(tranformsDict);
        
        var firstDict = tranformsDict.First();
        Assert.Single(firstDict);
        Assert.Equal("PathRemovePrefix", firstDict.Keys.First());
        Assert.Equal("transform-value", firstDict.Values.First());
    }
}
