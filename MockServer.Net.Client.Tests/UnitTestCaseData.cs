namespace MockServer.Net.Client.Tests
{
    using System.Collections;
    using NUnit.Framework;
    using SimpleFixture;

    public class UnitTestCaseData
    {

        public static IEnumerable ExceptionTestData
        {
            get
            {
                yield return new TestCaseData("Reset")
                    .SetName("ResetRequest_ShouldThrowException")
                    .SetCategory("Reset");
                yield return new TestCaseData("Retrieve")
                    .SetName("RetrieveRequest_ShouldThrowException")
                    .SetCategory("Retrieve");
                yield return new TestCaseData("Status")
                    .SetName("StatusRequest_ShouldThrowException")
                    .SetCategory("Status");
                yield return new TestCaseData("Stop")
                    .SetName("StopRequest_ShouldThrowException")
                    .SetCategory("Stop");
                yield return new TestCaseData("Expectation")
                    .SetName("CreateExpectation_ShouldThrowException")
                    .SetCategory("Expectation");
                yield return new TestCaseData("Verify")
                    .SetName("VerifyRequest_ShouldThrowException")
                    .SetCategory("Verify");
                yield return new TestCaseData("VerifySequence")
                    .SetName("VerifySequenceRequest_ShouldThrowException")
                    .SetCategory("VerifySequence");
                yield return new TestCaseData("Clear")
                    .SetName("ClearRequest_ShouldThrowException")
                    .SetCategory("Clear");
                yield return new TestCaseData("Bind")
                    .SetName("BindRequest_ShouldThrowException")
                    .SetCategory("Bind");
            }
        }
    }
}