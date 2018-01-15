namespace MockServer.Net.Client.UnitTests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using FluentAssertions;
    using MockServer.Net.Client.RunConfiguration;
    using NUnit.Framework;
    using SimpleFixture;

    [TestFixture]
    [Category("Mock Server Runner")]
    public class MockServerRunnerUnitTests
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            this._fixture = new Fixture();
        }

        [Test]
        public void MockServerRunner_NullConfiguration_ShouldThrowException()
        {
            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => new MockServerRunner(null));

            //Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Contain("configuration");
        }

        [Test]
        public void MockServerRunner_JavaConfiguration_ShouldBeOk()
        {
            //Arrange
            var jarPath = this._fixture.Generate<string>();
            var configuration = new JavaConfiguration(jarPath);

            //Act
            var runner = new MockServerRunner(configuration);

            //Assert
            runner.Should().NotBeNull();
            runner.RestApiUrl.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void MockServerRunner_Start_ShouldStartProcess()
        {
            //Arrange
            var configuration = new JavaConfiguration(ResolveJarPath());
            var runner = new MockServerRunner(configuration);

            //Act
            runner.Start();

            //Assert
            var process = Process.GetProcessById(runner.ProcessId);
            process.Should().NotBeNull();
            runner.ProcessId.Should().Be(process.Id);
        }

        [Test]
        public void MockServerRunner_Start_InvalidConfiguration_ShouldThrowException()
        {
            //Arrange
            var jarPath = this._fixture.Generate<string>();
            var configuration = new JavaConfiguration(jarPath);
            var runner = new MockServerRunner(configuration);

            //Act
            var exception = Assert.Throws<Exception>(() => runner.Start());

            //Assert
            exception.Message.Should().Contain("Unable to start mockserver with ");
        }

        [Test]
        public void MockServerRunner_Kill_ShouldKillProcess()
        {
            //Arrange
            var configuration = new JavaConfiguration(ResolveJarPath());
            var runner = new MockServerRunner(configuration);
            runner.Start();
            var processId = runner.ProcessId;

            //Act
            runner.Kill();

            //Assert
            var process = Process.GetProcessById(processId);
            process.Should().BeNull();
            //var exception = Assert.Throws<Exception>(() => Process.GetProcessById(processId));
            //exception.Should().NotBeNull();
            //exception.Message.Should().Be("No process is associated with this object");
        }

        private string ResolveJarPath() => Path.Combine(
            Directory.GetCurrentDirectory(),
            "mockserver-netty-5.2.3-jar-with-dependencies.jar");
    }
}