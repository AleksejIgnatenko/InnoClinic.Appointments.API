using InnoClinic.Appointments.Application.Services;
using InnoClinic.Appointments.Infrastructure.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Testcontainers.RabbitMq;

namespace InnoClinic.Appointments.TestSuiteNUnit.ServiceTests;

class RabbitMQServiceTests
{
    private RabbitMqContainer _rabbitMqContainer;
    private IRabbitMQService _rabbitMQService;
    private RabbitMQSetting rabbitMqSettings;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _rabbitMqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3.11-management")
            .WithUsername("guest")
            .WithPassword("guest")
            .WithPortBinding(5672) 
            .Build();

        await _rabbitMqContainer.StartAsync(); 
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _rabbitMqContainer.DisposeAsync();
    }

    [SetUp]
    public void SetUp()
    {
        rabbitMqSettings = new RabbitMQSetting
        {
            HostName = _rabbitMqContainer.Hostname,
            UserName = "guest",
            Password = "guest",
        };

        var services = new ServiceCollection();
        services.Configure<RabbitMQSetting>(options =>
        {
            options.HostName = rabbitMqSettings.HostName;
            options.UserName = rabbitMqSettings.UserName;
            options.Password = rabbitMqSettings.Password;
        });
        services.AddSingleton<IRabbitMQService, RabbitMQService>();

        var serviceProvider = services.BuildServiceProvider();
        _rabbitMQService = serviceProvider.GetRequiredService<IRabbitMQService>();
    }

    [Test]
    public async Task CreateQueuesAsync_ShouldCreateQueues()
    {
        // Arrange
        var connectionFactory = new ConnectionFactory
        {
            HostName = _rabbitMqContainer.Hostname,
            UserName = rabbitMqSettings.UserName,
            Password = rabbitMqSettings.Password,
        };

        var optionsMock = new Mock<IOptions<RabbitMQSetting>>();
        optionsMock.Setup(o => o.Value).Returns(rabbitMqSettings);

        var service = new RabbitMQService(optionsMock.Object);

        // Act
        await service.CreateQueuesAsync();

        using var connection = connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        // Assert
        Assert.IsTrue(channel.QueueDeclarePassive(RabbitMQQueues.ADD_DOCTOR_QUEUE) != null);
        Assert.IsTrue(channel.QueueDeclarePassive(RabbitMQQueues.UPDATE_DOCTOR_QUEUE) != null);
        Assert.IsTrue(channel.QueueDeclarePassive(RabbitMQQueues.DELETE_DOCTOR_QUEUE) != null);
        Assert.IsTrue(channel.QueueDeclarePassive(RabbitMQQueues.ADD_PATIENT_QUEUE) != null);
        Assert.IsTrue(channel.QueueDeclarePassive(RabbitMQQueues.UPDATE_PATIEN_QUEUE) != null);
        Assert.IsTrue(channel.QueueDeclarePassive(RabbitMQQueues.DELETE_PATIENT_QUEUE) != null);
    }

    [Test]
    public async Task PublishMessageAsync_ShouldPublishMessage()
    {
        // Arrange
        var message = new { Name = "Test Doctor", Id = 1 };
        var queueName = RabbitMQQueues.ADD_DOCTOR_QUEUE;

        // Act
        await _rabbitMQService.PublishMessageAsync(message, queueName);

        using var connection = new ConnectionFactory
        {
            HostName = _rabbitMqContainer.Hostname,
            UserName = rabbitMqSettings.UserName,
            Password = rabbitMqSettings.Password,
        }.CreateConnection();

        using var channel = connection.CreateModel();
        var result = channel.BasicGet(queueName, autoAck: true);

        Assert.IsNotNull(result, "A message should have been published to the queue.");
        var body = Encoding.UTF8.GetString(result.Body.ToArray());
        var receivedMessage = JsonConvert.DeserializeObject<dynamic>(body);

        Assert.AreEqual(message.Name, receivedMessage.Name.ToString());
        Assert.AreEqual(message.Id, (int)receivedMessage.Id);
    }
}