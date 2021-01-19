using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CrmUpdateIP
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly HttpClient _httpClient;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    var ipUrl = "https://ip.tool.lu";
                    var ipInfo = await _httpClient.GetStringAsync(ipUrl, stoppingToken);
                    if (string.IsNullOrEmpty(ipInfo))
                    {
                        _logger.LogInformation("未获取到公网IP信息");
                    }
                    Console.WriteLine($"获取到Ip信息{ipInfo}");
                    var ip = ipInfo.Replace(Convert.ToChar(10).ToString(), " ").Replace(Convert.ToChar(13).ToString(), "").Split(" ")[1];
                    Console.WriteLine($"获取到的IP地址为：{ip}");
                    _logger.LogInformation($"{ip}");

                    // var updateIpUrl = "http://yingyan.xiongying.com/";
                    // var result = await _httpClient.GetStringAsync(updateIpUrl, stoppingToken);

                    //Console.WriteLine($"更新Ip到CRM结果：{result}");
                    GetMacAddress();

                    await Task.Delay(1000 * 120, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"更新CRM IP地址出错，Message:{ex.Message}");
                }
            }
        }

        private static void GetMacAddress()
        {
            var adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && adapter.OperationalStatus == OperationalStatus.Up)
                {
                    Console.WriteLine($"无线 Mac 地址：{adapter.GetPhysicalAddress()}");
                }

                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet && adapter.OperationalStatus == OperationalStatus.Up)
                {
                    Console.WriteLine($"有线 Mac 地址：{adapter.GetPhysicalAddress()}");
                }
            }
        }
    }
}
