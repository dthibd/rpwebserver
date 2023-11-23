using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.SmartEnum;
using AutoMapper;
using WebComponentServer.AutoMapping;
using WebComponentServer.Models.ClusterConfig;
using WebComponentServer.Services.ReverseProxy.Config.Cluster;
using Yarp.ReverseProxy.LoadBalancing;

namespace WebComponentServerTest.AutoMapping
{
    public class ClusterConfigProfileTest
    {
        public MapperConfiguration MapperConfig { get; } = new(cfg => cfg.AddProfile<ClusterConfigProfile>());

        public IMapper Mapper => MapperConfig.CreateMapper();

        [Fact]
        public void Configuration_IsValid()
        {
            MapperConfig.AssertConfigurationIsValid();
        }

        [Fact]
        public void LoadBalancingValueToString()
        {
            var name = Mapper.Map<string>(LoadBalancingValue.RoundRobin);

            Assert.Equal(LoadBalancingPolicies.RoundRobin, name);
        }

        [Fact]
        public void ValidLoadBalancingPolicyStringToLoadBalancingValue()
        {
            var value = Mapper.Map<LoadBalancingValue>(LoadBalancingPolicies.RoundRobin);

            Assert.Equal(LoadBalancingValue.RoundRobin, value);
        }

        [Fact]
        public void InvalidLoadBalancingPolicyStringToLoadBalancingValue()
        {
            var value = Record.Exception(() => Mapper.Map<LoadBalancingValue>("invalid"));

            Assert.NotNull(value);
            Assert.IsType<SmartEnumNotFoundException>(value);
        }

        [Fact]
        public void MutableDestinationConfigToDestinationConfigDto()
        {
            var config = new MutableDestinationConfig("address");

            var dto = Mapper.Map<DestinationConfigDto>(config);

            Assert.NotNull(dto);
            Assert.Equal(config.Address, dto.Address);
        }

        [Fact]
        public void DestinationConfigDtoToMutableDestinationConfig()
        {
            var dto = new DestinationConfigDto()
            {
                Address = "address"
            };

            var config = Mapper.Map<MutableDestinationConfig>(dto);

            Assert.NotNull(config);
            Assert.Equal(dto.Address, config.Address);
        }

        [Fact]
        public void MutableClusterConfigToClusterConfigDto()
        {
            var config = new MutableClusterConfig("id");
            config.Set
                .LoadBalancingPolicy(LoadBalancingValue.RoundRobin)
                .AddDefaultDestination("address");

            var dto = Mapper.Map<ClusterConfigDto>(config);

            Assert.NotNull(dto);
            Assert.Equal(config.Id, dto.ClusterId);
            Assert.NotNull(config.LoadBalancingPolicy);
            Assert.NotNull(dto.LoadBalancingPolicy);
            Assert.Equal(config.LoadBalancingPolicy.Name, dto.LoadBalancingPolicy);
            Assert.NotNull(dto.Destinations);
            Assert.Equal(config.Destinations.Count, dto.Destinations.Count);
            Assert.Equal(config.Destinations.First().Key, dto.Destinations.First().Key);
        }

        [Fact]
        public void ClusterConfigDtoToMutableClusterConfig()
        {
            var dto = new ClusterConfigDto()
            {
                ClusterId = "id",
                LoadBalancingPolicy = LoadBalancingPolicies.RoundRobin,
                Destinations = new Dictionary<string, DestinationConfigDto>()
                {
                    {"default", new DestinationConfigDto() {Address = "address"}}
                }
            };

            var config = Mapper.Map<MutableClusterConfig>(dto);

            Assert.NotNull(config);
            Assert.Equal(dto.ClusterId, config.Id);
            Assert.NotNull(dto.LoadBalancingPolicy);
            Assert.NotNull(config.LoadBalancingPolicy);
            Assert.Equal(dto.LoadBalancingPolicy, config.LoadBalancingPolicy.Name);
            Assert.NotNull(config.Destinations);
            Assert.Equal(dto.Destinations.Count, config.Destinations.Count);
            Assert.Equal(dto.Destinations.First().Key, config.Destinations.First().Key);
        }

    }
}